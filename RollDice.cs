using System;
using System.Collections.Generic;
using System.Text;

namespace LcrGame
{
    internal interface IDiceRull
    {
        resultEnum RollDice();
    }

    class DiceRoll : IDiceRull
    {
        private Random _randomizer = new Random();

        public resultEnum RollDice()
        {
           var roll = _randomizer.Next(1, 6);
            switch (roll)
            {
               case 1:
                    return resultEnum.Center;
               case 2:
                    return resultEnum.Right;
               case 3:
                    return resultEnum.Left;
               default:
                    return resultEnum.None;
            }
        }
    }

    public enum resultEnum
    {
        Right, Left, Center, None
    }
}
