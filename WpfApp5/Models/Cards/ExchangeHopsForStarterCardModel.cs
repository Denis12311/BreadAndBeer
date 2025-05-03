using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp5.Models.Cards
{
    internal class ExchangeHopsForStarterCardModel:CardModel
    {

        public ExchangeHopsForStarterCardModel()
        {
            Name = "Поміняти хміль на закваску";
            ImagePath = "Images/HopsForStarter.png";
        }
        public override void UseCard(PlayerModel player)
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
}
