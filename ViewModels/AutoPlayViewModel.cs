using LcrGame.Assets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;

namespace LcrGame.ViewModels
{
    public class AutoPlayViewModel : INotifyPropertyChanged
    {
        private readonly Preset _noPrestValue;
        private Preset _selectedPreset;
        private int _numberOfPlayers;
        private int _numberOfGames;
        private RelayCommand _playCommand;
        private RelayCommand _cancelCommand;
        private CancellationTokenSource _cancellationTokenSource;
        private CancellationToken _cancellationToken;
        private bool _isBusy;
        private List<IPlayer> _players;

        public AutoPlayViewModel()
        {
            _noPrestValue = new Preset();
            PresetOptions.Add(_noPrestValue);
            PresetOptions.Add(new Preset(3, 100));
            PresetOptions.Add(new Preset(4, 100));
            PresetOptions.Add(new Preset(5, 100));
            PresetOptions.Add(new Preset(5, 1000));
            PresetOptions.Add(new Preset(5, 10000));
            PresetOptions.Add(new Preset(5, 100000));
            PresetOptions.Add(new Preset(6, 100));
            PresetOptions.Add(new Preset(7, 100));
            SelectedPreset = _noPrestValue;
        }

        public List<Preset> PresetOptions { get; } = new List<Preset>();

        public BarGraphViewModel BarGraph { get; } = new BarGraphViewModel();

        public Preset SelectedPreset
        {
            get => _selectedPreset;
            set
            {
                if (_selectedPreset != value)
                {
                    _selectedPreset = value;
                    if (SelectedPreset.NumberOfGames != 0)
                    {
                        NumberOfGames = value.NumberOfGames;
                        NumberOfPlayers = value.NumberOfPlayers;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public int NumberOfPlayers
        {
            get => _numberOfPlayers;
            set
            {
                if (_numberOfPlayers != value)
                {
                    if (value != SelectedPreset.NumberOfPlayers)
                        SelectedPreset = _noPrestValue;
                    _numberOfPlayers = value;
                    OnPropertyChanged();
                }
            }
        }

        public int NumberOfGames
        {
            get => _numberOfGames;
            set
            {
                if (_numberOfGames != value)
                {
                    if (value != SelectedPreset.NumberOfGames)
                        SelectedPreset = _noPrestValue;
                    _numberOfGames = value;
                    OnPropertyChanged();
                }
            }
        }


        public List<IPlayer> Players
        {
            get => _players;
            set
            {
                if (_players != value)
                {
                    _players = value;
                    OnPropertyChanged();
                }
            }
        }
        public RelayCommand PlayCommand => _playCommand ??= new RelayCommand(
            async () => await ExecuteSetAsync(),
            () => NumberOfGames > 1 && NumberOfPlayers > 2 && !_isBusy);

        public RelayCommand CancelCommand => _cancelCommand ??= new RelayCommand(
            () => { _cancellationTokenSource.Cancel(); },
            () => _isBusy);

        private static List<IPlayer> CreatePlayers(int numberOfPlayers)
        {
            return Enumerable.Range(1, numberOfPlayers).Select(i => new PlayerViewModel("Player " + i)).Cast<IPlayer>().ToList();
        }

        private void ExecuteSet()
        {
            var Games = new GameStats[NumberOfGames];

            for (int gameCounter = 0; gameCounter < NumberOfGames; gameCounter++)
            {
                Players.ForEach(i => i.Reset());
                var game = new CurrentTurnPlayersViewModel(Players);
                var playTurn = new PlayTurn(game);
                playTurn.ExecuteGame();
                Games[gameCounter] = new GameStats(game.Winner.Name, game.TurnCount);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async Task ExecuteSetAsync()
        {
            _isBusy = true;
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            var dispatcher = Dispatcher.CurrentDispatcher;

            CancelCommand.RaiseCanExecuteChanged();
            PlayCommand.RaiseCanExecuteChanged();

            var Games = new GameStats[NumberOfGames];
            BarGraph.Reset(NumberOfGames);
            Players = CreatePlayers(NumberOfPlayers);

            await Task.Factory.StartNew(() =>
            {

                for (int gameCounter = 0; gameCounter < NumberOfGames; gameCounter++)
                {
                    if (_cancellationToken.IsCancellationRequested)
                    {
                        return;
                    }
                    Players.ForEach(i => i.Reset());
                    var game = new CurrentTurnPlayersViewModel(Players);
                    var playTurn = new PlayTurn(game);
                    playTurn.ExecuteGame();
                    dispatcher.Invoke(
                         () =>
                         {
                             Games[gameCounter] = new GameStats(game.Winner.Name, game.TurnCount);
                             BarGraph.AddItem(game.TurnCount);
                         });
                };
                dispatcher.Invoke(
                     () =>
                     {
                         _cancellationTokenSource = null;
                         _isBusy = false;
                         CancelCommand.RaiseCanExecuteChanged();
                         PlayCommand.RaiseCanExecuteChanged();
                     });
            }, _cancellationToken);

        }
    }

    public class Preset
    {
        public Preset() { }

        public Preset(int numberOfPlayers, int numberOfGames)
        {
            NumberOfPlayers = numberOfPlayers;
            NumberOfGames = numberOfGames;
        }

        internal int NumberOfPlayers { get; }
        internal int NumberOfGames { get; }

        public override string ToString()
        {
            return NumberOfGames > 0 ? $"{NumberOfPlayers} players x {NumberOfGames}" : "NONE";
        }
    }
}
