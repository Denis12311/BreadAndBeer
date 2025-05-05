using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp5.Models;
using WpfApp5.Models.Cards;
using static WpfApp5.Models.GameModel;

namespace WpfApp5.Services
{
    internal class DataBaseService
    {

        private readonly PlayerService _playerService;
        private readonly GameModel _gameModel;

        public DataBaseService(PlayerService playerService, GameModel gameModel)
        {
            _playerService = playerService;
            _gameModel = gameModel;
        }

        public void CreateDatabaseIfNotExists()
        {
            string masterConnectionString = "Server=DESKTOP-HREKGJO\\SQLEXPRESS;Database=master;Integrated Security=True;";
            using (SqlConnection connection = new SqlConnection(masterConnectionString))
            {
                connection.Open();
                string checkDbQuery = "IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'GameDB') CREATE DATABASE GameDB;";
                using (SqlCommand command = new SqlCommand(checkDbQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            InitializeDatabase();
        }
        public void InitializeDatabase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["GameDBConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Створення таблиці GameState
                string createGameStateTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'GameState')
                BEGIN
                    CREATE TABLE GameState (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        Player1Name NVARCHAR(50),
                        Player1Grain INT,
                        Player1Hops INT,
                        Player1Water INT,
                        Player1Starter INT,
                        Player1FireWood INT,
                        Player1Bread INT,
                        Player1Beer INT,
                        Player2Name NVARCHAR(50),
                        Player2Grain INT,
                        Player2Hops INT,
                        Player2Water INT,
                        Player2Starter INT,
                        Player2FireWood INT,
                        Player2Bread INT,
                        Player2Beer INT,
                        CurrentSeason NVARCHAR(20),
                        Turns INT,
                        Hod INT,
                        Score INT,
                        CurrentPlayer NVARCHAR(20),
                        LastSaved DATETIME DEFAULT GETDATE()
                    )
                END";
                using (SqlCommand command = new SqlCommand(createGameStateTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Створення таблиці PlayerCards
                string createPlayerCardsTableQuery = @"
                IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PlayerCards')
                BEGIN
                    CREATE TABLE PlayerCards (
                        Id INT IDENTITY(1,1) PRIMARY KEY,
                        PlayerName NVARCHAR(50),
                        CardType NVARCHAR(50), 
                        ImagePath NVARCHAR(255),
                        LastSaved DATETIME DEFAULT GETDATE()
                    )
                END";
                using (SqlCommand command = new SqlCommand(createPlayerCardsTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void SaveGame()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["GameDBConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Збереження стану гри в таблиці GameState
                string queryGameState = @"
                INSERT INTO GameState 
                (Player1Name, Player1Grain, Player1Hops, Player1Water, Player1Starter, Player1FireWood, Player1Bread, Player1Beer,
                 Player2Name, Player2Grain, Player2Hops, Player2Water, Player2Starter, Player2FireWood, Player2Bread, Player2Beer,
                 CurrentSeason, Turns, Hod, Score, CurrentPlayer)
                VALUES 
                (@Player1Name, @Player1Grain, @Player1Hops, @Player1Water, @Player1Starter, @Player1FireWood, @Player1Bread, @Player1Beer,
                 @Player2Name, @Player2Grain, @Player2Hops, @Player2Water, @Player2Starter, @Player2FireWood, @Player2Bread, @Player2Beer,
                 @CurrentSeason, @Turns, @Hod, @Score, @CurrentPlayer)";

                using (SqlCommand command = new SqlCommand(queryGameState, connection))
                {
                    // Параметри для Player1
                    command.Parameters.AddWithValue("@Player1Name", _playerService.Player1.Name);
                    command.Parameters.AddWithValue("@Player1Grain", _playerService.Player1.Grain);
                    command.Parameters.AddWithValue("@Player1Hops", _playerService.Player1.Hops);
                    command.Parameters.AddWithValue("@Player1Water", _playerService.Player1.Water);
                    command.Parameters.AddWithValue("@Player1Starter", _playerService.Player1.Starter);
                    command.Parameters.AddWithValue("@Player1FireWood", _playerService.Player1.FireWood);
                    command.Parameters.AddWithValue("@Player1Bread", _playerService.Player1.Bread);
                    command.Parameters.AddWithValue("@Player1Beer", _playerService.Player1.Beer);

                    // Параметри для Player2
                    command.Parameters.AddWithValue("@Player2Name", _playerService.Player2.Name);
                    command.Parameters.AddWithValue("@Player2Grain", _playerService.Player2.Grain);
                    command.Parameters.AddWithValue("@Player2Hops", _playerService.Player2.Hops);
                    command.Parameters.AddWithValue("@Player2Water", _playerService.Player2.Water);
                    command.Parameters.AddWithValue("@Player2Starter", _playerService.Player2.Starter);
                    command.Parameters.AddWithValue("@Player2FireWood", _playerService.Player2.FireWood);
                    command.Parameters.AddWithValue("@Player2Bread", _playerService.Player2.Bread);
                    command.Parameters.AddWithValue("@Player2Beer", _playerService.Player2.Beer);

                    // Параметри для гри
                    command.Parameters.AddWithValue("@CurrentSeason", _gameModel.CurrentSeason.ToString());
                    command.Parameters.AddWithValue("@Turns", _gameModel.Turns);
                    command.Parameters.AddWithValue("@Hod", _gameModel.Hod);
                    command.Parameters.AddWithValue("@Score", _gameModel.Score);
                    command.Parameters.AddWithValue("@CurrentPlayer", _gameModel.CurrentPlayer == _playerService.Player1 ? "Player1" : "Player2");

                    command.ExecuteNonQuery();
                }

                // Збереження карт для кожного гравця
                SavePlayerCards(connection, _playerService.Player1);
                SavePlayerCards(connection, _playerService.Player2);
            }
        }

        private void SavePlayerCards(SqlConnection connection, PlayerModel player)
        {
            // Видалення старих карт
            string deleteQuery = "DELETE FROM PlayerCards WHERE PlayerName = @PlayerName";
            using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
            {
                deleteCommand.Parameters.AddWithValue("@PlayerName", player.Name);
                deleteCommand.ExecuteNonQuery();
            }

            // Додавання нових карт
            foreach (var card in player.Inventory)
            {
                string queryCard = @"
                INSERT INTO PlayerCards (PlayerName, CardType, ImagePath)
                VALUES (@PlayerName, @CardType, @ImagePath)";
                using (SqlCommand command = new SqlCommand(queryCard, connection))
                {
                    command.Parameters.AddWithValue("@PlayerName", player.Name);
                    command.Parameters.AddWithValue("@CardType", card.Name);
                    command.Parameters.AddWithValue("@ImagePath", card.ImagePath);
                    command.ExecuteNonQuery();
                }
            }
        }

        public bool LoadGame()
        {
            string connStr = ConfigurationManager.ConnectionStrings["GameDBConnection"].ConnectionString;
            using (var conn = new SqlConnection(connStr))
            {
                conn.Open();
                string sql = "SELECT TOP 1 * FROM GameState ORDER BY LastSaved DESC";
                using (var cmd = new SqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                        return false;  // сохранения нет

                    // восстанавливаем состояние
                    _gameModel.CurrentSeason = (SeasonType)Enum.Parse(
                        typeof(SeasonType), reader["CurrentSeason"].ToString());
                    _gameModel.Turns = Convert.ToInt32(reader["Turns"]);
                    _gameModel.Hod = Convert.ToInt32(reader["Hod"]);
                    _gameModel.Score = Convert.ToInt32(reader["Score"]);

                    string cp = reader["CurrentPlayer"].ToString();
                    _gameModel.CurrentPlayer = cp == "Player1"
                        ? _playerService.Player1
                        : _playerService.Player2;

                    // Восстановление имён (если нужно)
                    _playerService.Player1.Name = reader["Player1Name"].ToString();
                    _playerService.Player2.Name = reader["Player2Name"].ToString();
                }

                // отдельно подгружаем карты каждого игрока из своей БД
                LoadPlayerCards(_playerService.Player1);
                LoadPlayerCards(_playerService.Player2);
                return true;
            }
        }

        private void LoadPlayerCards(PlayerModel player)
        {
            // открываем собственное соединение, чтобы не мешаться с внешним reader
            string connectionString = ConfigurationManager.ConnectionStrings["GameDBConnection"].ConnectionString;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                player.Inventory.Clear();

                string query = "SELECT CardType, ImagePath FROM PlayerCards WHERE PlayerName = @PlayerName";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PlayerName", player.Name);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string cardType = reader.GetString(0);
                            string imagePath = reader.GetString(1);

                            var card = CreateCard(cardType, imagePath);
                            if (card != null)
                                player.Inventory.Add(card);
                        }
                    }
                }
            }
        }


        public void ClearDatabase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["GameDBConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                new SqlCommand("DELETE FROM GameState", connection).ExecuteNonQuery();
                new SqlCommand("DELETE FROM PlayerCards", connection).ExecuteNonQuery();
            }
            MessageBox.Show("Дані з бази очищено.");
        }



        private CardModel CreateCard(string cardType, string imagePath)
        {
            CardModel card = null;

            switch (cardType)
            {
                case "Зробити звичайний хліб":
                    card = new AddNormBreadCardModel();
                    break;
                case "Зробити ситний хліб":
                    card = new AddHeartyBreadCardModel();
                    break;
                case "Зробити урожайний хліб":
                    card = new AddHarvestBreadCardModel();
                    break;
                case "Зробити простий хліб":
                    card = new AddSimpleBreadCardModel();
                    break;
                case "Зробити пиво":
                    card = new AddNormBeerCardModel();
                    break;
                case "Зробити лагер":
                    card = new AddLagerBeerCardModel();
                    break;
                case "Зробити темне пиво":
                    card = new AddDarkBeerCardModel();
                    break;
                case "Зробити святкове пиво":
                    card = new AddHolBeerCardModel();
                    break;
                case "Поміняти зерно на воду":
                    card = new ExchangeGrainForWaterCardModel();
                    break;
                case "Поміняти воду на зерно":
                    card = new ExchangeWaterForGrainCardModel();
                    break;
                case "Поміняти закваску на хміль":
                    card = new ExchangeStarterForHopsCardModel();
                    break;
                case "Поміняти хміль на закваску":
                    card = new ExchangeHopsForStarterCardModel();
                    break;
                default:
                    // Если тип карты не распознан, оставляем card = null
                    break;
            }

            if (card != null)
            {
                card.ImagePath = imagePath;
            }

            return card;
        }

    }
}
