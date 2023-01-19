using System;

namespace Poker.Core.Domain.Entity
{
    public enum EValue
    {
        Ace = 14,
        King = 13,
        Queen = 12,
        Jack = 11,
        Ten = 10,
        Nine = 9,
        Eight = 8,
        Seven = 7,
        Six = 6,
        Five = 5,
        Four = 4,
        Three = 3,
        Two = 2,
    }

    public static class ValueExtensions
    {
        public static string ToStr(this EValue value)
        {
            switch (value)
            {
                case EValue.Ace:
                    return "A";
                case EValue.King:
                    return "K";
                case EValue.Queen:
                    return "Q";
                case EValue.Jack:
                    return "J";
                case EValue.Ten:
                case EValue.Nine:
                case EValue.Eight:
                case EValue.Seven:
                case EValue.Six:
                case EValue.Five:
                case EValue.Four:
                case EValue.Three:
                case EValue.Two:
                    return ((int)value).ToString();
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
