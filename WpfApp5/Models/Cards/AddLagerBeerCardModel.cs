using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace WpfApp5.Models.Cards
{
    internal class AddLagerBeerCardModel: CardModel
    {

        public AddLagerBeerCardModel()
        {
            Name = "Зробити лагер";
            ImagePath = "pack://application:,,,/Images/AddLagerBeer.png";
        }
        public override void UseCard(PlayerModel player)
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
}
