using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp5.Models.Cards
{
    internal class AddSimpleBreadCardModel:CardModel
    {
        public AddSimpleBreadCardModel()
            {
                Name = "Зробити простий хліб";
                ImagePath = "Images/AddSimpleBread.png"; // Укажи реальный путь к картинке в проекте
            }
            public override void UseCard(PlayerModel player)
            {
                if (player.Grain >= 2 && player.Water >= 1)
                {

                    player.Bread += 2;
                    player.Grain -= 2;
                    player.Water -= 1;
                }
                else
                {
                    MessageBox.Show("Недостатньо ресурсів");
                }
            }
    }
}
