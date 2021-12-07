using LcrGame.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace LcrGame
{
    public interface ICurrentTurnPlayers
    {
        IPlayer[] Players { get; }
        IPlayer CurrnetPlayer { get; }
        IPlayer RightPlayer { get; }
        IPlayer LeftPlayer { get; }
        int CenterTokens { get; }
        string DieRolls { get; }
        string WinnerIs{ get; }

        void ClearDieRolls();
        void AddDieRoll(resultEnum dieRoll);
        void NextTurn();
        void AddCenterTokens();
    }

    public class CurrentTurnPlayers : ICurrentTurnPlayers, INotifyPropertyChanged
    {
        private IPlayer _currnetPlayer;
        private IPlayer _rightPlayer;
        private IPlayer _leftPlayer;
        private int _centerTokens;
        private List<string> _dieRolls = new List<string>();
        private string _winnerIs;

        public CurrentTurnPlayers(IEnumerable<IPlayer> players)
        {
            Players = players.ToArray();
            RightPlayer = Players[Players.Length - 1];
            CurrnetPlayer = Players[0];
            LeftPlayer = Players[1];
        }

        public IPlayer[] Players { get; }

        public IPlayer CurrnetPlayer
        {
            get => _currnetPlayer;
            private set
            {
                if (_currnetPlayer != value)
                {
                    _currnetPlayer = value;
                    OnPropertyChanged();
                }
            }
        }

        public IPlayer RightPlayer
        {
            get => _rightPlayer;
            private set
            {
                if (_rightPlayer != value)
                {
                    _rightPlayer = value;
                    OnPropertyChanged();
                }
            }
        }

        public IPlayer LeftPlayer
        {
            get => _leftPlayer;
            private set
            {
                if (_leftPlayer != value)
                {
                    _leftPlayer = value;
                    OnPropertyChanged();
                }
            }
        }

        public int CenterTokens
        {
            get => _centerTokens;
            private set
            {
                if (_centerTokens != value)
                {
                    _centerTokens = value;
                    OnPropertyChanged();
                }
            }
        }

        public string WinnerIs
        {
            get => _winnerIs;
            private set
            {
                if (_winnerIs != value)
                {
                    _winnerIs = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DieRolls
        {
            get => _dieRolls.Any() ? "Die Rolls: " + string.Join(", ", _dieRolls) : string.Empty ;
        }

        public void AddDieRoll(resultEnum dieRoll)
        {
            _dieRolls.Add(dieRoll.ToString());
            OnPropertyChanged(nameof(DieRolls));
        }

        public void ClearDieRolls()
        {
            _dieRolls.Clear();
            OnPropertyChanged(nameof(DieRolls));
        }

        public void AddCenterTokens()
        {
            CenterTokens++;
        }

        public void NextTurn()
        {
            var playersWithTokens = Players.Where(i => i.Tokens > 0);
            if (playersWithTokens.Count() == 1)
            {
                WinnerIs = "The Winner: " + playersWithTokens.First().Name;
                CurrnetPlayer = playersWithTokens.First();
            }
            else
            {
                RightPlayer = CurrnetPlayer;
                CurrnetPlayer = LeftPlayer;
                var leftPlayerIndex = Array.FindIndex(Players, i => i == CurrnetPlayer);
                if (++leftPlayerIndex == Players.Length) leftPlayerIndex = 0;
                LeftPlayer = Players[leftPlayerIndex];
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
