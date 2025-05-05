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
    internal class AddHeartyBreadCardModel: CardModel
    {
        public AddHeartyBreadCardModel()
        {
            Name = "Зробити ситний хліб";
            ImagePath = "Images/AddHeartyBread.png"; // Укажи реальный путь к картинке в проекте
        }
        public override void UseCard(PlayerModel player)
        {
            if (player.Grain >= 3 && player.Starter >= 1 && player.Water >= 1)
            {


                player.Bread += 5;
                player.Grain -= 3;
                player.Starter -= 1;
                player.Water -= 1;
            }
            else
            {
                MessageBox.Show("Недостатньо ресурсів");
            }
        }


    }
}
