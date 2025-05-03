    using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp5.Models
{
    public class GameModel: INotifyPropertyChanged
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

        private PlayerModel _currentPlayer;
        public PlayerModel CurrentPlayer
        {
            get => _currentPlayer;
            set
            {
                _currentPlayer = value;
                OnPropertyChanged(nameof(CurrentPlayer));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    } 
}
