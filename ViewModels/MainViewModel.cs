using LcrGame.Assets;
using System.Collections.Generic;
using System.Windows.Input;

namespace LcrGame.ViewModels
{
    public class MainViewModel
    {
        private readonly IPlayTurn _playTurn;
        private RelayCommand _ExectueCommand;

        public MainViewModel()
        {
            var players = new List<IPlayer>
            {
                new PlayerViewModel("Player 1"),
                new PlayerViewModel("Player 2"),
                new PlayerViewModel("Player 3"),
            };

            GameState = new CurrentTurnPlayersViewModel(players);

            _playTurn = new PlayTurn(GameState);
        }

        public ICurrentTurnPlayers GameState { get; }

        public ICommand ExectueCommand => _ExectueCommand ??= new RelayCommand(
            () => _playTurn.ExecuteTurn(),
            () => string.IsNullOrWhiteSpace(GameState.WinnerIs));
    }
}
