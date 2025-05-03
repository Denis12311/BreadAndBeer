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
        private readonly DataBaseService _dbService;

        public ObservableCollection<CardModel> Hand => _playerService.CurrentPlayer.Inventory;
        public PlayerModel CurrentPlayer => _gameModel.CurrentPlayer;
        public int Turns => _gameModel.Turns;

        public ICommand UseCardCommand { get; }
        public ICommand EndTurnCommand { get; }
        public ICommand SaveGameCommand { get; }
        public ICommand LoadGameCommand { get; }
        public ICommand ClearDatabaseCommand { get; }

        public GameViewModel(
            GameModel gameModel,
            PlayerService playerService,
            CardService cardService,
            GameStateService gameStateService,
            DataBaseService dbService)
        {
            _gameModel = gameModel;
            _playerService = playerService;
            _cardService = cardService;
            _gameStateService = gameStateService;
            _dbService = dbService;

            // 1. Prepare DB + load
            _dbService.CreateDatabaseIfNotExists();
            _dbService.InitializeDatabase();
            bool loaded = _dbService.LoadGame();

            // 2. Если не было сохранения — новая игра
            if (!loaded)
            {
                _gameStateService.GiveStartResources();
                _cardService.Shuffle();
                DealInitialHands();
                _dbService.SaveGame();
            }

            // 3. Подписки
            _gameModel.PropertyChanged += OnGameModelPropertyChanged;

            // 4. Команды
            UseCardCommand = new RelayCommand(ExecuteUseCard, _ => true);
            EndTurnCommand = new RelayCommand(_ =>
            {
                _gameStateService.ChangeSeasons();
                _dbService.SaveGame();
                RaiseAllProperties();
            });

            SaveGameCommand = new RelayCommand(_ => _dbService.SaveGame());
            LoadGameCommand = new RelayCommand(_ =>
            {
                _dbService.LoadGame();
                RaiseAllProperties();
            });
            ClearDatabaseCommand = new RelayCommand(_ => _dbService.ClearDatabase());
        }

        private void DealInitialHands()
        {
            _playerService.Player1.Inventory.Clear();
            foreach (var c in _cardService.DrawHand(5))
                _playerService.Player1.Inventory.Add(c);

            _playerService.Player2.Inventory.Clear();
            foreach (var c in _cardService.DrawHand(5))
                _playerService.Player2.Inventory.Add(c);

            RaiseAllProperties();
        }

        private void ExecuteUseCard(object param)
        {
            if (param is CardModel card)
            {
                _playerService.UseCard(card);
                _playerService.SwitchPlayer();
                _gameModel.Hod++;
                _gameStateService.EndTurnCheck();
                _playerService.CheckAndSwapCards();
                _dbService.SaveGame();
                RaiseAllProperties();
            }
        }

        private void OnGameModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GameModel.CurrentPlayer))
            {
                OnPropertyChanged(nameof(CurrentPlayer));
                OnPropertyChanged(nameof(Hand));
            }
            else if (e.PropertyName == nameof(GameModel.Turns))
            {
                OnPropertyChanged(nameof(Turns));
            }
        }

        private void RaiseAllProperties()
        {
            OnPropertyChanged(nameof(CurrentPlayer));
            OnPropertyChanged(nameof(Hand));
            OnPropertyChanged(nameof(Turns));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
