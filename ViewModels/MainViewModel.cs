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
                new Player("Player 1"),
                new Player("Player 2"),
                new Player("Player 3"),
            };

            GameState = new CurrentTurnPlayers(players);

            _playTurn = new PlayTurn(GameState);
        }

        public ICurrentTurnPlayers GameState { get; }

        public ICommand ExectueCommand => _ExectueCommand ??= new RelayCommand(
            () => _playTurn.Execute(),
            () => string.IsNullOrWhiteSpace(GameState.WinnerIs));
    }
}
