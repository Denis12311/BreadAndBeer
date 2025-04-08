using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;
using static WpfApp5.MainWindow;
using Microsoft.IdentityModel.Protocols;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace WpfApp5
{


    public class Player : INotifyPropertyChanged
    {
        private ObservableCollection<Card> _inventory = new ObservableCollection<Card>();
        public ObservableCollection<Card> Inventory
        {
            get { return _inventory; }
            set
            {
                _inventory = value;
                OnPropertyChanged(nameof(Inventory));
            }
        }




        private int _grain;
        public int Grain
        {
            get { return _grain; }
            set
            {
                _grain = value;
                OnPropertyChanged(nameof(Grain));
            }
        }

        private int _hops;
        public int Hops
        {
            get { return _hops; }
            set
            {
                _hops = value; OnPropertyChanged(nameof(Hops));
            }
        }
        private int _water;
        public int Water
        {
            get { return _water; }
            set
            {
                _water = value; OnPropertyChanged(nameof(Water));

            }

        }
        private int _starter;
        public int Starter
        {
            get { return _starter; }
            set
            {
                _starter = value; OnPropertyChanged(nameof(Starter));
            }
        }
        private int _firewood;
        public int FireWood
        {
            get { return _firewood; }
            set
            {
                _firewood = value; OnPropertyChanged(nameof(FireWood));
            }
        }
        private int _bread;
        public int Bread
        {
            get { return _bread; }
            set
            {
                _bread = value; OnPropertyChanged(nameof(Bread));
            }
        }
        private int _beer;
        public int Beer
        {
            get { return _beer; }
            set
            {
                _beer = value; OnPropertyChanged(nameof(Beer));
            }
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    //переменная для ходов

    public enum SeasonType
    {
        Zasuha,
        NeZasuha
    }


    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private Player Player1;
        private Player Player2;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }



        private SeasonType _currentSeason;
        public SeasonType CurrentSeason
        {
            get => _currentSeason;
            set
            {
                _currentSeason = value;
                OnPropertyChanged(nameof(CurrentSeason));
            }
        }
        public int _turns;
        public int Turns
        {
            get => _turns;
            set
            {
                _turns = value;
                OnPropertyChanged(nameof(Turns));
            }
        }

        public int _hod;
        public int Hod
        {
            get => _hod;
            set
            {
                _hod = value;
                OnPropertyChanged(nameof(Hod));
            }
        }

        public int _score;

        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                OnPropertyChanged(nameof(Score));
            }
        }


        private static readonly Random rand = new Random();
        public ObservableCollection<Card> PlayerCards { get; set; } = new ObservableCollection<Card>();

        private StackPanel cardPanel;


        private Player _currentPlayer;
        public Player CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                _currentPlayer = value;
                OnPropertyChanged(nameof(CurrentPlayer));
            }
        }


        //Card Interface

        public interface IUseCard
        {
            void UseCard(Player player);
        }

        // Абстрактный базовый класс карты, реализующий IUseCard
        public abstract class Card : IUseCard
        {
            public string Name { get; set; }
            public string ImagePath { get; set; } // Путь к изображению карты

            public abstract void UseCard(Player player);
        }

        // Карти які роблять хліб
        public class AddNormBreadCard : Card
        {
            public AddNormBreadCard()
            {
                Name = "Зробити хліб";
                ImagePath = "Images/AddNormBread.png"; // Укажи реальный путь к картинке в проекте
            }
            public override void UseCard(Player player)
            {
                if (player.Grain >= 2 && player.Starter >= 1 && player.Water >= 1)
                {


                    player.Bread += 3;
                    player.Grain -= 2;
                    player.Starter -= 1;
                    player.Water -= 1;
                }
                else
                {
                    MessageBox.Show("Недостатньо ресурсів");
                }
            }
        }
        public class AddHeartyBreadCard : Card
        {
            public AddHeartyBreadCard()
            {
                Name = "Зробити ситний хліб";
                ImagePath = "Images/AddHeartyBread.png"; // Укажи реальный путь к картинке в проекте
            }
            public override void UseCard(Player player)
            {
                if (player.Grain >= 3 && player.Starter >= 1 && player.Water >= 1)
                {


                    player.Bread += 5;
                    player.Grain -= 3;
                    player.Starter -= 1;
                    player.Water -= 1;
                }
                else
                {
                    MessageBox.Show("Недостатньо ресурсів");
                }
            }
        }


        public class AddHarvestBreadCard : Card
        {
            public AddHarvestBreadCard()
            {
                Name = "Зробити урожайний хліб";
                ImagePath = "Images/AddHarvestBread.png"; // Укажи реальный путь к картинке в проекте
            }
            public override void UseCard(Player player)
            {
                if (player.Grain >= 3 && player.Starter >= 1 && player.Water >= 1)
                { 
                    player.Bread += 5;
                    player.Grain -= 3;
                    player.Starter -= 1;
                    player.Water -= 1;
                }
                else
                {
                    MessageBox.Show("Недостатньо ресурсів");
                }

            }
        }
        public class AddSimpleBreadCard : Card
        {
            public AddSimpleBreadCard()
            {
                Name = "Зробити простий хліб";
                ImagePath = "Images/AddSimpleBread.png"; // Укажи реальный путь к картинке в проекте
            }
            public override void UseCard(Player player)
            {
                if (player.Grain >= 2 && player.Water >= 1)
                {

                    player.Bread += 2;
                    player.Grain -= 2;
                    player.Water -= 1;
                }
                else
                {
                    MessageBox.Show("Недостатньо ресурсів");
                }
            }
        }

        // Карти які роблять пиво
        public class AddLagerBeerCard : Card
        {
            public AddLagerBeerCard()
            {
                Name = "Зробити лагер";
                ImagePath = "pack://application:,,,/Images/AddLagerBeer.png";
            }
            public override void UseCard(Player player)
            {
                if (player.Grain >= 1 && player.Hops >= 1 && player.Water >= 1)
                {

                    player.Beer += 3;
                    player.Grain -= 1;
                    player.Hops -= 1;
                    player.Water -= 1;
                }
                else
                {
                    MessageBox.Show("Недостатньо ресурсів");
                }
            }
        }
        //Темне пиво
        public class AddDarkBeerCard : Card
        {
            public AddDarkBeerCard()
            {
                Name = "Зробити темне пиво";
                ImagePath = "pack://application:,,,/Images/AddDarkBeer.png";
            }
            public override void UseCard(Player player)
            {
                if (player.Grain >= 2 && player.Hops >= 2 && player.Water >= 1 && player.FireWood >= 1)
                {
                    player.Beer += 5;
                    player.Grain -= 2;
                    player.Hops -= 2;
                    player.Water -= 1;
                    player.FireWood -= 1;
                }
                else
                {
                    MessageBox.Show("Недостатньо ресурсів");
                }
            }
        }
        public class AddHolBeerCard : Card
        {
            public AddHolBeerCard()
            {
                Name = "Зробити святкове пиво";
                ImagePath = "pack://application:,,,/Images/AddHolBeer.png";
            }
            public override void UseCard(Player player)
            {
                if (player.Grain >= 3 && player.Hops >= 2 && player.Water >= 2 && player.FireWood >= 1)
                {
                    player.Beer += 6;
                    player.Grain -= 3;
                    player.Hops -= 2;
                    player.Water -= 2;
                    player.FireWood -= 1;
                }
                else
                {
                    MessageBox.Show("Недостатньо ресурсів");
                }
            }
        }
        public class AddNormBeerCard : Card
        {
            public AddNormBeerCard()
            {
                Name = "Зробити пиво";
                ImagePath = "pack://application:,,,/Images/AddNormBeer.png";
            }
            public override void UseCard(Player player)
            {
                if (player.Hops >= 1 && player.Water >= 1)
                {

                    player.Beer += 2;
                    player.Hops -= 1;
                    player.Water -= 1;
                }
                else
                {
                    MessageBox.Show("Недостатньо ресурсів");
                }
            }
        }

        // Карти обміну ресурсів
        public class ExchangeGrainForWaterCard : Card
        {
            public ExchangeGrainForWaterCard()
            {
                Name = "Поміняти зерно на воду";
                ImagePath = "Images/GrainForWater.png";
            }
            public override void UseCard(Player player)
            {
                if (player.Grain >= 2)
                {
                    player.Grain -= 2;
                    player.Water += 2;
                }
                else
                {
                    MessageBox.Show("Недостатньо зерна");
                }
            }
        }

        public class ExchangeWaterForGrainCard : Card
        {
            public ExchangeWaterForGrainCard()
            {
                Name = "Поміняти воду на зерно";
                ImagePath = "Images/WaterForGrain.png";
            }
            public override void UseCard(Player player)
            {
                if (player.Water >= 2)
                {
                    player.Water -= 2;
                    player.Grain += 2;
                }
                else
                {
                    MessageBox.Show("Недостатньо води");
                }
            }
        }
        public class ExchangeStarterForHopsCard : Card
        {
            public ExchangeStarterForHopsCard()
            {
                Name = "Поміняти закваску на хміль";
                ImagePath = "Images/StarterForHops.png";
            }
            public override void UseCard(Player player)
            {
                if (player.Starter >= 2)
                {
                    player.Starter -= 2;
                    player.Hops += 2;
                }
                else
                {
                    MessageBox.Show("Недостатньо Закваски");
                }
            }
        }
        public class ExchangeHopsForStarterCard : Card
        {
            public ExchangeHopsForStarterCard()
            {
                Name = "Поміняти хміль на закваску";
                ImagePath = "Images/HopsForStarter.png";
            }
            public override void UseCard(Player player)
            {
                if (player.Hops >= 2)
                {
                    player.Hops -= 2;
                    player.Starter += 2;
                }
                else
                {
                    MessageBox.Show("Недостатньо хмілю");
                }
            }
        }
        //Main Window


        public MainWindow()
        {
            InitializeComponent();
            CreateDatabaseIfNotExists();
            InitializeDatabase();
            Player1 = new Player { Grain = 0, Hops = 0, Water = 0, Starter = 0, FireWood = 0, Beer = 0, Bread = 0, Name = "Player 1" };

            Player2 = new Player { Grain = 0, Hops = 0, Water = 0, Starter = 0, FireWood = 0, Beer = 0, Bread = 0, Name = "Player 2" };
            CurrentPlayer = Player1;
            StartGame();
            LoadGame();
            GiveCardAtSeasonStart2();
            DataContext = this;
            Console.WriteLine($"Кількість карт у гравця: {CurrentPlayer.Inventory.Count}");

        }




        //Test Stats Button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CurrentPlayer.Hops += 1;
            CurrentPlayer.Beer += 1;
            CurrentPlayer.Bread += 1;
            CurrentPlayer.Water += 1;
            CurrentPlayer.FireWood += 1;
            CurrentPlayer.Grain += 1;
            CurrentPlayer.Starter += 1;
        }

        //Cards Methods
        private void AddCard_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPlayer.Inventory.Count >= 5) return;

            Card newCard = GetRandomCard();
            CurrentPlayer.Inventory.Add(newCard);
        }

        //Use Card
        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Card card)
            {
                card.UseCard(CurrentPlayer);

                // Видалення карти з інвентаря
                Dispatcher.Invoke(() => CurrentPlayer.Inventory.Remove(card));
                SwitchPlayer();
                Hod++;
                CheckAndSwapCards();
                MessageBox.Show($"Ход Передається гравцю{CurrentPlayer.Name}");
                EndTurnCheck();
                SaveGame();

            }
        }


        private Card GetRandomCard()
        {
            var cardList = new Card[]
            {
                new AddNormBreadCard(),
                new AddHeartyBreadCard(),
                new AddHarvestBreadCard(),
                new AddSimpleBreadCard(),
                new AddNormBeerCard(),
                new AddDarkBeerCard(),
                new AddLagerBeerCard(),
                new AddHolBeerCard(),
                new ExchangeGrainForWaterCard(),
                new ExchangeWaterForGrainCard(),
                new ExchangeStarterForHopsCard(),
                new ExchangeHopsForStarterCard()
            };

            return cardList[rand.Next(cardList.Length)];
        }


        public void GiveCardAtSeasonStart()
        {
            CurrentPlayer.Inventory.Clear();
            while (Player1.Inventory.Count < 5)
            {
                Card newCard = GetRandomCard();
                Player1.Inventory.Add(newCard);
            }
            while(Player2.Inventory.Count<5)
            {
                Card newCard = GetRandomCard();
                Player2.Inventory.Add(newCard);
            }
        }
        public void GiveCardAtSeasonStart2()
        {
            if (Player1.Inventory.Count==0 && Player1.Inventory.Count == 0)
            {
                for (int i = 0; i < 5; i++)
                {
                    Card newCard = GetRandomCard();
                    Player1.Inventory.Add(newCard);
                    Player2.Inventory.Add(newCard);
                }
            }
        }
        private void StartGame()
        {
            Turns = 0;
            CurrentSeason = SeasonType.NeZasuha;

            GiveSeasonResources();
            GiveCardAtSeasonStart();
        }
        public void EndTurnCheck()
        {
            if (Player1.Inventory.Count == 0 && Player2.Inventory.Count==0)
            {
                ChangeSeasons();
            }
        }

        private void ChangeSeasons()
        {
            Turns++;

            if (Turns > 6)
            {
                int scorePlayer1 = CalculateScore(Player1);
                int scorePlayer2 = CalculateScore(Player2);
                string resultMessage;

                if (scorePlayer1 > scorePlayer2)
                    resultMessage = $"Гра закінчена! Переміг {Player1.Name} з очками: {scorePlayer1}.";
                else if (scorePlayer2 > scorePlayer1)
                    resultMessage = $"Гра закінчена! Переміг {Player2.Name} з очками: {scorePlayer2}.";
                else
                    resultMessage = $"Гра закінчена! Нічия між {Player1.Name} та {Player2.Name}. Очки: {scorePlayer1}.";

                MessageBox.Show(resultMessage);
                ReturnToStartMenu();
                return;
            }

            CurrentSeason = (CurrentSeason == SeasonType.Zasuha) ? SeasonType.NeZasuha : SeasonType.Zasuha;
            GiveSeasonResources();
            GiveCardAtSeasonStart();
        }
        private void GiveSeasonResources()
        {
            if (CurrentSeason == SeasonType.NeZasuha)
            {
                Player1.Grain += 6;
                Player1.Hops += 4;
                Player1.Water += 5;
                Player1.Starter += 3;
                Player1.FireWood += 4;

                Player2.Grain += 6;
                Player2.Hops += 4;
                Player2.Water += 5;
                Player2.Starter += 3;
                Player2.FireWood += 4;
            }
            if (CurrentSeason == SeasonType.Zasuha)
            {
                Player1.Grain += 3;
                Player1.Hops += 2;
                Player1.Water += 3;
                Player1 .Starter += 2;
                Player1.FireWood += 2;

                Player2.Grain += 3;
                Player2.Hops += 2;
                Player2.Water += 3;
                Player2.Starter += 2;
                Player2.FireWood += 2;
            }
        }

        public int CalculateScore(Player player)
        {
            return Math.Min(player.Bread, player.Beer);
        }

        public void SwitchPlayer()
        {
            if (CurrentPlayer == Player1)
            {
                CurrentPlayer = Player2;
            }
            else
            {
                CurrentPlayer = Player1;
            }
        }

        private void SwapPlayerCards()
        {
            var tempInventory = Player1.Inventory;
            Player1.Inventory = Player2.Inventory;
            Player2.Inventory = tempInventory;
        }
        private void CheckAndSwapCards()
        {
            if (Hod % 2 == 0)
            {
                SwapPlayerCards();
            }
        }
        private void ReturnToStartMenu()
        {
            StartMenu startMenu = new StartMenu();
            startMenu.Show();
            this.Close();
        }


        // Databaze
        public void InitializeDatabase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["GameDBConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                // Создание таблицы GameState, если не существует
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

                // Создание таблицы для карт игроков, если не существует
                string createPlayerCardsTableQuery = @"
            IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'PlayerCards')
            BEGIN
                CREATE TABLE PlayerCards (
                    Id INT IDENTITY(1,1) PRIMARY KEY,
                    PlayerName NVARCHAR(50),
                    CardType NVARCHAR(50), -- имя или тип карты
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
                // Сохраняем состояние игры в таблицу GameState (как раньше)
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
                    // Добавление параметров для GameState (как раньше)
                    command.Parameters.AddWithValue("@Player1Name", Player1.Name);
                    command.Parameters.AddWithValue("@Player1Grain", Player1.Grain);
                    command.Parameters.AddWithValue("@Player1Hops", Player1.Hops);
                    command.Parameters.AddWithValue("@Player1Water", Player1.Water);
                    command.Parameters.AddWithValue("@Player1Starter", Player1.Starter);
                    command.Parameters.AddWithValue("@Player1FireWood", Player1.FireWood);
                    command.Parameters.AddWithValue("@Player1Bread", Player1.Bread);
                    command.Parameters.AddWithValue("@Player1Beer", Player1.Beer);

                    command.Parameters.AddWithValue("@Player2Name", Player2.Name);
                    command.Parameters.AddWithValue("@Player2Grain", Player2.Grain);
                    command.Parameters.AddWithValue("@Player2Hops", Player2.Hops);
                    command.Parameters.AddWithValue("@Player2Water", Player2.Water);
                    command.Parameters.AddWithValue("@Player2Starter", Player2.Starter);
                    command.Parameters.AddWithValue("@Player2FireWood", Player2.FireWood);
                    command.Parameters.AddWithValue("@Player2Bread", Player2.Bread);
                    command.Parameters.AddWithValue("@Player2Beer", Player2.Beer);

                    command.Parameters.AddWithValue("@CurrentSeason", CurrentSeason.ToString());
                    command.Parameters.AddWithValue("@Turns", Turns);
                    command.Parameters.AddWithValue("@Hod", Hod);
                    command.Parameters.AddWithValue("@Score", Score);

                    // Сохраняем, какой игрок является текущим:
                    string currentPlayerStr = (CurrentPlayer == Player1) ? "Player1" : "Player2";
                    command.Parameters.AddWithValue("@CurrentPlayer", currentPlayerStr);

                    command.ExecuteNonQuery();
                }
                SavePlayerCards(connection, Player1);
                SavePlayerCards(connection, Player2);
            }
            MessageBox.Show("Гру збережено!");
        }


        private void SavePlayerCards(SqlConnection connection, Player player)
        {
            // Видаляємо старі записи для даного гравця
            string deleteQuery = "DELETE FROM PlayerCards WHERE PlayerName = @PlayerName";
            using (SqlCommand deleteCommand = new SqlCommand(deleteQuery, connection))
            {
                deleteCommand.Parameters.AddWithValue("@PlayerName", player.Name);
                deleteCommand.ExecuteNonQuery();
            }

            // Додаємо поточні карти з інвентарю
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

        public void LoadGame()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["GameDBConnection"].ConnectionString;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT TOP 1 * FROM GameState ORDER BY LastSaved DESC";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (Player1 == null) Player1 = new Player();
                            if (Player2 == null) Player2 = new Player();

                            // Чтение состояния игры
                            string seasonStr = reader["CurrentSeason"] as string ?? "NeZasuha";
                            CurrentSeason = (SeasonType)Enum.Parse(typeof(SeasonType), seasonStr);
                            Turns = reader["Turns"] != DBNull.Value ? Convert.ToInt32(reader["Turns"]) : 0;
                            Hod = reader["Hod"] != DBNull.Value ? Convert.ToInt32(reader["Hod"]) : 0;
                            Score = reader["Score"] != DBNull.Value ? Convert.ToInt32(reader["Score"]) : 0;

                            // Восстановление данных для Player1
                            Player1.Name = reader["Player1Name"].ToString();
                            Player1.Grain = reader["Player1Grain"] != DBNull.Value ? Convert.ToInt32(reader["Player1Grain"]) : 0;
                            Player1.Hops = reader["Player1Hops"] != DBNull.Value ? Convert.ToInt32(reader["Player1Hops"]) : 0;
                            Player1.Water = reader["Player1Water"] != DBNull.Value ? Convert.ToInt32(reader["Player1Water"]) : 0;
                            Player1.Starter = reader["Player1Starter"] != DBNull.Value ? Convert.ToInt32(reader["Player1Starter"]) : 0;
                            Player1.FireWood = reader["Player1FireWood"] != DBNull.Value ? Convert.ToInt32(reader["Player1FireWood"]) : 0;
                            Player1.Bread = reader["Player1Bread"] != DBNull.Value ? Convert.ToInt32(reader["Player1Bread"]) : 0;
                            Player1.Beer = reader["Player1Beer"] != DBNull.Value ? Convert.ToInt32(reader["Player1Beer"]) : 0;

                            // Восстановление данных для Player2
                            Player2.Name = reader["Player2Name"].ToString();
                            Player2.Grain = reader["Player2Grain"] != DBNull.Value ? Convert.ToInt32(reader["Player2Grain"]) : 0;
                            Player2.Hops = reader["Player2Hops"] != DBNull.Value ? Convert.ToInt32(reader["Player2Hops"]) : 0;
                            Player2.Water = reader["Player2Water"] != DBNull.Value ? Convert.ToInt32(reader["Player2Water"]) : 0;
                            Player2.Starter = reader["Player2Starter"] != DBNull.Value ? Convert.ToInt32(reader["Player2Starter"]) : 0;
                            Player2.FireWood = reader["Player2FireWood"] != DBNull.Value ? Convert.ToInt32(reader["Player2FireWood"]) : 0;
                            Player2.Bread = reader["Player2Bread"] != DBNull.Value ? Convert.ToInt32(reader["Player2Bread"]) : 0;
                            Player2.Beer = reader["Player2Beer"] != DBNull.Value ? Convert.ToInt32(reader["Player2Beer"]) : 0;

                            // Определяем, какой игрок является текущим
                            string currentPlayerStr = reader["CurrentPlayer"].ToString();
                            if (currentPlayerStr == "Player1")
                                CurrentPlayer = Player1;
                            else if (currentPlayerStr == "Player2")
                                CurrentPlayer = Player2;
                            else
                                CurrentPlayer = Player1;

                            MessageBox.Show("Гру завантажено!");
                        }
                        else
                        {
                            MessageBox.Show("Збережень не знайдено!");
                        }
                    }
                }

                LoadPlayerCards(connection, Player1);
                LoadPlayerCards(connection, Player2);
            }
        }


        private void LoadPlayerCards(SqlConnection connection, Player player)
        {
            player.Inventory.Clear();

            string query = "SELECT CardType, ImagePath FROM PlayerCards WHERE PlayerName = @PlayerName";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@PlayerName", player.Name);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string cardType = reader["CardType"].ToString();
                        string imagePath = reader["ImagePath"].ToString();

                        // Создаем объект карты по типу. 
                        // Здесь пример сопоставления типа карты со строковым значением.
                        Card card = null;
                        switch (cardType)
                        {
                            case "Зробити хліб":
                                card = new AddNormBreadCard();
                                break;
                            case "Зробити ситний хліб":
                                card = new AddHeartyBreadCard();
                                break;
                            case "Зробити урожайний хліб":
                                card = new AddHarvestBreadCard();
                                break;
                            case "Зробити простий хліб":
                                card = new AddSimpleBreadCard();
                                break;
                            case "Зробити пиво":
                                card = new AddNormBeerCard();
                                break;
                            case "Зробити лагер":
                                card = new AddLagerBeerCard();
                                break;
                            case "Зробити темне пиво":
                                card = new AddDarkBeerCard();
                                break;
                            case "Зробити святкове пиво":
                                card = new AddHolBeerCard();
                                break;
                            case "Поміняти зерно на воду":
                                card = new ExchangeGrainForWaterCard();
                                break;
                            case "Поміняти воду на зерно":
                                card = new ExchangeWaterForGrainCard();
                                break;
                            case "Поміняти закваску на хміль":
                                card = new ExchangeStarterForHopsCard();
                                break;
                            case "Поміняти хміль на закваску":
                                card = new ExchangeHopsForStarterCard();
                                break;
                            default:
                                break;
                        }
                        if (card != null)
                        {
                            // Если требуется, можно также обновить ImagePath (если путь хранится в БД)
                            card.ImagePath = imagePath;
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
                string deleteGameState = "DELETE FROM GameState";
                using (SqlCommand command = new SqlCommand(deleteGameState, connection))
                {
                    command.ExecuteNonQuery();
                }
                string deletePlayerCards = "DELETE FROM PlayerCards";
                using (SqlCommand command = new SqlCommand(deletePlayerCards, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            MessageBox.Show("Данные в базе данных очищены.");
        }
    }
}