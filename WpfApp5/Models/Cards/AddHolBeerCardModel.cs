using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp5.Models.Cards
{
    internal class AddHolBeerCardModel:CardModel
    {
        public AddHolBeerCardModel()
        {
            Name = "Зробити святкове пиво";
            ImagePath = "pack://application:,,,/Images/AddHolBeer.png";
        }
        public override void UseCard(PlayerModel player)
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
}
