using System;

namespace Poker.Logic.Entity
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

        public override bool Equals(object obj)
        {
            var card = (Card)obj;
            return card.Color == Color && card.Value == Value;
        }

        public override int GetHashCode()
        {
            return Color.GetHashCode() + Value.GetHashCode();
        }

        public static bool operator ==(Card obj1, Card obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;
            if (ReferenceEquals(obj1, null))
                return false;
            if (ReferenceEquals(obj2, null))
                return false;
            return obj1.Equals(obj2);
        }
        public static bool operator !=(Card obj1, Card obj2)
        => !(obj1 == obj2);
    }
}
