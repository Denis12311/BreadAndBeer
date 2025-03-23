using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static WpfApp5.MainWindow;

namespace WpfApp5
{


    public class Player : INotifyPropertyChanged
    {
        private ObservableCollection<Card> _inventory = new ObservableCollection<Card>();
        public ObservableCollection<Card> Inventory
        {
            get { return _inventory; }
            set
            {
                _inventory = value;
                OnPropertyChanged(nameof(Inventory));
            }
        }




        private int _grain;
        public int Grain
        {
            get { return _grain; }
            set
            {
                _grain = value;
                OnPropertyChanged(nameof(Grain));
            }
        }

        private int _hops;
        public int Hops
        {
            get { return _hops; }
            set
            {
                _hops = value; OnPropertyChanged(nameof(Hops));
            }
        }
        private int _water;
        public int Water
        {
            get { return _water; }
               set 
                {
                _water = value;OnPropertyChanged(nameof(Water));

                }

        }
        private int _starter;
        public int Starter
        {
            get { return _starter; }
            set
            {
                _starter = value; OnPropertyChanged(nameof(Starter));
            }
        }
        private int _firewood;
        public int FireWood
        {
            get { return _firewood; }
            set
            {
                _firewood = value; OnPropertyChanged(nameof(FireWood));
            }
        }
        private int _bread;
        public int Bread
        {
            get { return _bread; }
            set
            {
                _bread = value; OnPropertyChanged(nameof(Bread));
            }
        }
        private int _beer;
        public int Beer
        {
            get { return _beer; }
            set
            {
                _beer = value; OnPropertyChanged(nameof(Beer));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
        
    //переменная для ходов

    public enum SeasonType
    {
        Zasuha,
        NeZasuha
    }


    public partial class MainWindow : Window
    {
        private SeasonType _currentSeason;
        public SeasonType CurrentSeason
        {
            get => _currentSeason;
            set
            {
                _currentSeason = value;
                OnPropertyChanged(nameof(CurrentSeason));
            }
        }
        public int _turns;
        public int Turns
        {
            get => _turns;
            set
            {
                _turns = value;
                OnPropertyChanged(nameof(Turns));
            }
        }



        private static readonly Random rand = new Random();
        public ObservableCollection<Card> PlayerCards { get; set; } = new ObservableCollection<Card>();

        private StackPanel cardPanel;

        public Player CurrentPlayer { get; set; }

        public interface IUseCard
        {
            void UseCard(Player player);
        }

        // Абстрактный базовый класс карты, реализующий IUseCard
        public abstract class Card : IUseCard
        {
            public string Name { get; set; }
            public string ImagePath { get; set; } // Путь к изображению карты

            public abstract void UseCard(Player player);
        }

        // Конкретная карта "Добавить хлеб"
        public class AddBreadCard : Card
        {
            public AddBreadCard()
            {
                Name = "Додати хліб";
                ImagePath = "Images/AddBread.png"; // Укажи реальный путь к картинке в проекте
            }
            public override void UseCard(Player player)
            {
                player.Bread += 1;
            }
        }

        // Конкретная карта "Добавить пиво"
        public class AddBeerCard : Card
        {
            public AddBeerCard()
            {
                Name = "Додати пиво";
                ImagePath = "Images/AddBeer.png";
            }
            public override void UseCard(Player player)
            {
                player.Beer += 1;
            }
        }

        // Конкретная карта "Обмен зерна на воду"
        public class ExchangeGrainForWaterCard : Card
        {
            public ExchangeGrainForWaterCard()
            {
                Name = "Поміняти зерно на воду";
                ImagePath = "Images/GrainForWater.png";
            }
            public override void UseCard(Player player)
            {
                if (player.Grain > 0)
                {
                    player.Grain--;
                    player.Water++;
                }
            }
        }

        // Конкретная карта "Обмен воды на зерно"
        public class ExchangeWaterForGrainCard : Card
        {
            public ExchangeWaterForGrainCard()
            {
                Name = "Поміняти воду на зерно";
                ImagePath = "Images/WaterForGrain.png";
            }
            public override void UseCard(Player player)
            {
                if (player.Water > 0)
                {
                    player.Water--;
                    player.Grain++;
                }
            }
        }

        public MainWindow()
        {
            InitializeComponent();

            // Ініціалізація об'єкта гравця
            CurrentPlayer = new Player
            {
                Grain = 10,
                Hops = 5,
                Water = 20,
                Starter = 3,
                FireWood = 7,
                Beer = 0
            };

            DataContext = this;
            Console.WriteLine($"Кількість карт у гравця: {CurrentPlayer.Inventory.Count}");
        }
        //Test Stats Button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CurrentPlayer.Hops+=1;
            CurrentPlayer.Beer += 1;
            CurrentPlayer.Bread += 1;
            CurrentPlayer.Water += 1;
            CurrentPlayer.FireWood += 1;
            CurrentPlayer.Grain += 1;
            CurrentPlayer.Starter += 1;
        }

        //Cards Methods
        private void AddCard_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPlayer.Inventory.Count >= 5) return; // Обмеження на 5 карт

            Card newCard = GetRandomCard();
            CurrentPlayer.Inventory.Add(newCard);
        }


        private void CardButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is Card card)
            {
                card.UseCard(CurrentPlayer);

                // Видалення карти з інвентаря
                Dispatcher.Invoke(() => CurrentPlayer.Inventory.Remove(card));
            }
        }


        private Card GetRandomCard()
        {
            var cardList = new Card[]
            {
                new AddBreadCard(),
                new AddBeerCard(),
                new ExchangeGrainForWaterCard(),
                new ExchangeWaterForGrainCard()
            };

            return cardList[rand.Next(cardList.Length)];
        }


        public void GiveCardAtSeasonStart()
        {
            CurrentPlayer.Inventory.Clear();
            for (int i = 0; i < 5; i++)
            {
                Card newCard = GetRandomCard();
                CurrentPlayer.Inventory.Add(newCard);
            }
        }


    }


}
