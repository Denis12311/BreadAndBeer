using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp5.Models.Cards
{
    internal class ExchangeStarterForHopsCardModel:CardModel
    {
        public ExchangeStarterForHopsCardModel()
        {
            Name = "Поміняти закваску на хміль";
            ImagePath = "Images/StarterForHops.png";
        }
        public override void UseCard(PlayerModel player)
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
}
