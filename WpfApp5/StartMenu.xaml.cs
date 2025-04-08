using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Configuration;

namespace WpfApp5
{
    public partial class StartMenu : Window
    {
        public StartMenu()
        {
            InitializeComponent();
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();


            this.Close();

        }


        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            // Очищаем БД перед новой игрой
            var dbService = new DatabaseService();
            dbService.ClearDatabase();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();

            this.Close();
        }



        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }



        //Database

        public class DatabaseService
        {
            // Метод для очистки базы данных
            public void ClearDatabase()
            {
                string connectionString = ConfigurationManager.ConnectionStrings["GameDBConnection"].ConnectionString;
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Удаляем данные из таблицы GameState
                    string deleteGameState = "DELETE FROM GameState";
                    using (SqlCommand command = new SqlCommand(deleteGameState, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                    // Удаляем данные из таблицы PlayerCards
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
}

