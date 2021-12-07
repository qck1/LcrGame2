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
    }

    public class Player : IPlayer, INotifyPropertyChanged
    {
        private int tokens;

        public Player(string name)
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

        public void AddToken()
        {
            Tokens++;
        }

        public void RemoveToken()
        {
            Tokens--;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
