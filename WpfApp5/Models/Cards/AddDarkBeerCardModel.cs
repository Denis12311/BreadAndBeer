using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp5.Models.Cards
{
    internal class AddDarkBeerCardModel: CardModel
    {
        public AddDarkBeerCardModel()
        {
            Name = "Зробити темне пиво";
            ImagePath = "pack://application:,,,/Images/AddDarkBeer.png";
        }
        public override void UseCard(PlayerModel player)
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
}
