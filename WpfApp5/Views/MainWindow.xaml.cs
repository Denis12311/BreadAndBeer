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
using WpfApp5.Models;
using WpfApp5.Services;
using WpfApp5.ViewModels;
using System.Windows.Forms;

namespace WpfApp5
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Создание экземпляров сервисов и моделей
            var gameModel = new GameModel();
            var playerService = new PlayerService(gameModel); // Предполагается, что он создаёт и Player1, и Player2, и устанавливает CurrentPlayer
            var cardService = new CardService();

            gameModel.CurrentPlayer = playerService.Player1;

            var navigationService = new WpfApp5.Services.NavigationService(this); // Если NavigationService работает с Window

            var gameStateService = new GameStateService(playerService, gameModel,navigationService,cardService);

          

            // Создание ViewModel и установка DataContext
            DataContext = new GameViewModel(gameModel, playerService, cardService, gameStateService);
        }
    }
}








