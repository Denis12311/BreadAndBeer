using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp5.Models
{
    public class GameModel
    {

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

        public int _hod;
        public int Hod
        {
            get => _hod;
            set
            {
                _hod = value;
                OnPropertyChanged(nameof(Hod));
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



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

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

        public enum SeasonType
        {
            Zasuha,
            NeZasuha
        }

        private Player _currentPlayer;
        public Player CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                _currentPlayer = value;
                OnPropertyChanged(nameof(CurrentPlayer));
            }
        }
    } 
}
