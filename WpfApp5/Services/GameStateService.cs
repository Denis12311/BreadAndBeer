using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp5.Models;
using WpfApp5.Services;


namespace WpfApp5.Services
{
    public class GameStateService
    {

        private readonly PlayerService _playerService;
        private readonly GameModel _gameModel;
        private readonly NavigationService _navigationService;

        public GameStateService(PlayerService playerService, GameModel gameModel, NavigationService navigationService)
        {
            _playerService = playerService;
            _gameModel = gameModel;
            _navigationService = navigationService;
        }

        public void EndTurnCheck()
        {
            if(_playerService.Player1.Inventory.Count==0 && _playerService.Player2.Inventory.Count == 0)
            {
                ChangeSeasons();
            }
        }

        public void ChangeSeasons() 
        {
            _gameModel.Turns++;

            if (_gameModel.Turns > 6)
            {
                int scorePlayer1 = CalculateScore(_playerService.Player1);
                int scorePlayer2 = CalculateScore(_playerService.Player2);
                string resultMessage;

                if (scorePlayer1 > scorePlayer2)
                    resultMessage = $"Гра закінчена! Переміг {_playerService.Player1.Name} з очками: {scorePlayer1}.";
                else if (scorePlayer2 > scorePlayer1)
                    resultMessage = $"Гра закінчена! Переміг {_playerService.Player2.Name} з очками: {scorePlayer2}.";
                else
                    resultMessage = $"Гра закінчена! Нічия між {_playerService.Player1.Name} та {_playerService.Player2.Name}. Очки: {scorePlayer1}.";

                MessageBox.Show(resultMessage);
                _navigationService.NavigationToMainMenu();
                return;
            }

            _gameModel.CurrentSeason = (_gameModel.CurrentSeason == GameModel.SeasonType.Zasuha)
        ? GameModel.SeasonType.NeZasuha
        : GameModel.SeasonType.Zasuha;

            GiveSeasonResources();

            GiveCardAtSeasonStart();
        }



        public int CalculateScore(Player player)
        {
            return Math.Min(player.Bread, player.Beer);
        }

        private void GiveSeasonResources()
        {
            if (_gameModel.CurrentSeason == GameModel.SeasonType.NeZasuha)
            {
                _playerService.Player1.Grain += 6;
                _playerService.Player1.Hops += 4;
                _playerService.Player1.Water += 5;
                _playerService.Player1.Starter += 3;
                _playerService.Player1.FireWood += 4;

                _playerService.Player2.Grain += 6;
                _playerService.Player2.Hops += 4;
                _playerService.Player2.Water += 5;
                _playerService.Player2.Starter += 3;
                _playerService.Player2.FireWood += 4;
            }
            if (_gameModel.CurrentSeason == GameModel.SeasonType.Zasuha)
            {
                _playerService.Player1.Grain += 3;
                _playerService.Player1.Hops += 2;
                _playerService.Player1.Water += 3;
                _playerService.Player1.Starter += 2;
                _playerService.Player1.FireWood += 2;

                _playerService.Player2.Grain += 3;
                _playerService.Player2.Hops += 2;
                _playerService.Player2.Water += 3;
                _playerService.Player2.Starter += 2;
                _playerService.Player2.FireWood += 2;
            }
        }


    }
}
