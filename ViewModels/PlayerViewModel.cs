using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LcrGame.ViewModels
{
    public interface IPlayer
    {
        bool IsWinnter { get; set; }
        string Name { get; }
        int Tokens { get; }
        void AddToken();
        void RemoveToken();
        void Reset();
    }

    public class PlayerViewModel : IPlayer, INotifyPropertyChanged
    {
        private int _tokens;
        private bool _isWinner;

        public PlayerViewModel(string name)
        {
            Name = name;
            Tokens = 3;
        }

        public bool IsWinnter
        {
            get => _isWinner;
            set
            {
                if (_isWinner != value)
                {
                    _isWinner = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Name { get; }

        public int Tokens
        {
            get => _tokens;
            private set
            {
                if (_tokens != value)
                {
                    _tokens = value;
                    OnPropertyChanged();
                }
            }
        }

        public void Reset()
        {
            Tokens = 3;
            IsWinnter = false;
        }

        public void AddToken()
        {
            Tokens++;
        }

        public void RemoveToken()
        {
            Tokens--;
        }

        public override string ToString()
        {
            return $"{Name} with {Tokens} Tokens";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
