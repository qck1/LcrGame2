using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace LcrGame.ViewModels
{
    public interface IPlayer
    {
        string Name { get; }
        int Tokens { get; }
        void AddToken();
        void RemoveToken();
        void Reset();
    }

    public class PlayerViewModel : IPlayer, INotifyPropertyChanged
    {
        private int tokens;

        public PlayerViewModel(string name)
        {
            Name = name;
            Tokens = 3;
        }

        public string Name { get; }

        public int Tokens
        {
            get => tokens;
            private set
            {
                if (tokens != value)
                {
                    tokens = value;
                    OnPropertyChanged();
                }
            }
        }

        public void Reset()
        {
            Tokens = 3;
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
