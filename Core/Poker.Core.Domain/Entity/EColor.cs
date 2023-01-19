using System;

namespace Poker.Core.Domain.Entity
{
    public enum EColor
    {
        Clubs,
        Spades,
        Diamonds,
        Hearts,
    }

    public static class ColorExtensions
    {
        public static string ToStr(this EColor color)
        {
            switch (color)
            {
                case EColor.Clubs:
                    return "♣";
                case EColor.Spades:
                    return "♠";
                case EColor.Diamonds:
                    return "♦";
                case EColor.Hearts:
                    return "♥";
                default:
                    throw new NotSupportedException();
            }
        }
    }
}
