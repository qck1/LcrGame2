using System;
using System.Collections.Generic;
using System.Text;

namespace LcrGame
{
    public class GameStats
    {
        public GameStats(string winningPlayer, int numberOfTurns)
        {
            WinningPlayer = winningPlayer;
            NumberOfTurns = numberOfTurns;
        }

        public string WinningPlayer { get; }
        public int NumberOfTurns { get; }

        public override string ToString()
        {
            return $"Player \"{WinningPlayer}\" won after {NumberOfTurns} turns";
        }
    }
}
