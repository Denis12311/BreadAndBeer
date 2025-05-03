using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp5.Models.Cards
{
    internal class AddNormBeerCardModel:CardModel
    {

        public AddNormBeerCardModel()
        {
            Name = "Зробити пиво";
            ImagePath = "pack://application:,,,/Images/AddNormBeer.png";
        }
        public override void UseCard(PlayerModel player)
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
}
