using System;
using System.Linq;

namespace LcrGame
{
    public interface IPlayTurn
    {
        ICurrentTurnPlayers Players { get; }
        void Execute();

        bool IsWinner();
    }

    public class PlayTurn : IPlayTurn
    {
        private readonly DiceRoll _dice = new DiceRoll();

        public PlayTurn(ICurrentTurnPlayers players)
        {
            Players = players ?? throw new ArgumentNullException(nameof(players));
        }

        public void Execute()
        {
            var dieRolls = Math.Min(Players.CurrnetPlayer.Tokens, 3);
            Players.ClearDieRolls();

            for (int roll = 1; roll <= dieRolls; roll++)
            {
                var dieRollResult = _dice.RollDice();
                if (dieRollResult != resultEnum.None)
                {
                    Players.CurrnetPlayer.RemoveToken();
                    switch (dieRollResult)
                    {
                        case resultEnum.Right:
                            Players.RightPlayer.AddToken();
                            break;
                        case resultEnum.Left:
                            Players.LeftPlayer.AddToken();
                            break;
                        case resultEnum.Center:
                            Players.AddCenterTokens();
                            break;
                        default:
                            break;
                    }
                }
                Players.AddDieRoll(dieRollResult);
            }
            Players.NextTurn();
        }

        public bool IsWinner()
        {
            return Players.Players.Where(i => i.Tokens > 0).Count() == 1;
        }

        public ICurrentTurnPlayers Players { get; }
    }
}
