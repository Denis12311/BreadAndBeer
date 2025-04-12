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

        public Player Player1 = new Player { Grain = 0, Hops = 0, Water = 0, Starter = 0, FireWood = 0, Beer = 0, Bread = 0, Name = "Player 1" };

        public Player Player2 = new Player { Grain = 0, Hops = 0, Water = 0, Starter = 0, FireWood = 0, Beer = 0, Bread = 0, Name = "Player 2" };

        public Player CurrentPlayer { get; private set; }

        public  PlayerService()
        {
            CurrentPlayer = Player1;
        }

        public void SwitchPlayer()
        {
            CurrentPlayer = (CurrentPlayer == Player1) ? Player2 : Player1;
        }

        private void SwapPlayerCards()
        {
            var tempInventory = Player1.Inventory;
            Player1.Inventory = Player2.Inventory;
            Player2.Inventory = tempInventory;
        }


    }
}
