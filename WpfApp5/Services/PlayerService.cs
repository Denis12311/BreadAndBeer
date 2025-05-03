using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WpfApp5.MainWindow;
using System.Windows.Threading;
using System.Windows;
using System.Windows.Controls;
using WpfApp5.Models;
using WpfApp5.Services;

namespace WpfApp5.Services
{
    public class PlayerService
    {

        private readonly GameModel _gameModel;


        public PlayerModel Player1 = new PlayerModel { Grain = 0, Hops = 0, Water = 0, Starter = 0, FireWood = 0, Beer = 0, Bread = 0, Name = "Player 1" };

        public PlayerModel Player2 = new PlayerModel { Grain = 0, Hops = 0, Water = 0, Starter = 0, FireWood = 0, Beer = 0, Bread = 0, Name = "Player 2" };

        public PlayerModel CurrentPlayer { get; private set; }

        public  PlayerService(GameModel gameModel)
        {
            _gameModel = gameModel;
            CurrentPlayer = Player1;
            _gameModel.CurrentPlayer= CurrentPlayer;
        }

        public void SwitchPlayer()
        {
            CurrentPlayer = (CurrentPlayer == Player1) ? Player2 : Player1;
            _gameModel.CurrentPlayer = CurrentPlayer;
        }

        public void SwapPlayerCards()
        {
            var tempInventory = Player1.Inventory;
            Player1.Inventory = Player2.Inventory;
            Player2.Inventory = tempInventory;
        }

        public void UseCard(CardModel card)
        {
            card.UseCard(CurrentPlayer);
            CurrentPlayer.Inventory.Remove(card);

        }
        
        public void CheckAndSwapCards()
        {
            if (_gameModel.Hod % 2 == 0)
            {
                SwapPlayerCards();
                MessageBox.Show("Обмін картами між гравцями");
            }
        }

    }
}
