using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace WpfApp5.Services
{
    public class NavigationService
    {
        private readonly Frame _mainframe;

        public NavigationService(Frame mainframe)
        {
            _mainframe = mainframe;
        }

        public void NavigationToMainMenu()
        {
            var mainMenu = new StartMenu();
            mainMenu.Show();

            Application.Current.MainWindow.Close();
            Application.Current.MainWindow = mainMenu;
        }
    }
}
