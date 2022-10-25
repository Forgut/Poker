using System;

namespace Poker.Entity
{
    public class Card 
    {
        public Card(EValue value, EColor color)
        {
            Value = value;
            Color = color;
        }

        public EValue Value { get; }
        public EColor Color { get; }

        public override string ToString()
        {
            return $"{Value.ToStr()}{Color.ToStr()}";
        }
    }
}
