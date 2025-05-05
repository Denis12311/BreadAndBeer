using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WpfApp5.MainWindow;
using WpfApp5.Models;
using WpfApp5.Services;

namespace WpfApp5.Models
{
    public class PlayerModel: INotifyPropertyChanged
    {
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
                _water = value; OnPropertyChanged(nameof(Water));

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

        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public int _score;

        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                OnPropertyChanged(nameof(Score));
            }
        }


        private ObservableCollection<CardModel> _inventory = new ObservableCollection<CardModel>();
        public ObservableCollection<CardModel> Inventory
        {
            get { return _inventory; }
            set
            {
                _inventory = value;
                OnPropertyChanged(nameof(Inventory));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
