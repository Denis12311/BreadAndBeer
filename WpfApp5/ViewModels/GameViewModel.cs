using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using WpfApp5.Models;
using WpfApp5.Services;
using WpfApp5.Utilities;

namespace WpfApp5.ViewModels
{
    internal class GameViewModel : INotifyPropertyChanged
    {
        private readonly GameModel _gameModel;
        private readonly PlayerService _playerService;
        private readonly CardService _cardService;
        private readonly GameStateService _gameStateService;

        public ObservableCollection<CardModel> Hand => _playerService.CurrentPlayer.Inventory;

        public PlayerModel CurrentPlayer => _gameModel.CurrentPlayer;

        public ICommand UseCardCommand { get; }
        public ICommand EndTurnCommand { get; }

        public int Turns
        {
            get => _gameModel.Turns;
        }





        public GameViewModel(
            GameModel gameModel,
            PlayerService playerService,
            CardService cardService,
            GameStateService gameStateService)
        {
            _gameModel = gameModel;
            _playerService = playerService;
            _cardService = cardService;
            _gameStateService = gameStateService;



            //Роздача ресурсів

            _gameStateService.GiveStartResources();
            // Начальная раздача карт
            _cardService.Shuffle();
            // Раздаём 5 карт первому игроку
            var hand1 = _cardService.DrawHand(5);
            foreach (var card in hand1)
            {
                _playerService.Player1.Inventory.Add(card);
            }

            // Раздаём 5 карт второму игроку
            var hand2 = _cardService.DrawHand(5);
            foreach (var card in hand2)
            {
                _playerService.Player2.Inventory.Add(card);
            }

            _gameModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(GameModel.Turns))
                    OnPropertyChanged(nameof(Turns));
            };



            // Подписка на смену игрока для обновления UI
            _gameModel.PropertyChanged += OnGameModelPropertyChanged;

            UseCardCommand = new RelayCommand(
                ExecuteUseCard,
                param => param is CardModel);

            EndTurnCommand = new RelayCommand(
                _ => _gameStateService.ChangeSeasons(),
                _ => true);
        }

        private void OnGameModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GameModel.CurrentPlayer))
            {
                OnPropertyChanged(nameof(CurrentPlayer));
                OnPropertyChanged(nameof(Hand));
            }
        }

        private void ExecuteUseCard(object parameter)
        {
            if (parameter is CardModel card)
            {
                _playerService.UseCard(card);
                _playerService.SwitchPlayer();
                _gameModel.Hod++;
                _gameStateService.EndTurnCheck();
                OnPropertyChanged(nameof(Hand));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}