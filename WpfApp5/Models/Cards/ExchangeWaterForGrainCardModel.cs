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
    internal class ExchangeWaterForGrainCardModel:CardModel
    {

        public ExchangeWaterForGrainCardModel()
        {
            Name = "Поміняти воду на зерно";
            ImagePath = "Images/WaterForGrain.png";
        }
        public override void UseCard(PlayerModel player)
        {
            if (player.Water >= 2)
            {
                player.Water -= 2;
                player.Grain += 2;
            }
            else
            {
                MessageBox.Show("Недостатньо води");
            }
        }

    }
}
