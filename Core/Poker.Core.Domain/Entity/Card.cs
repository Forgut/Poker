using System.Linq;
using System.Text.RegularExpressions;
using System;
using Poker.Core.Domain.Exceptions;
using Poker.Core.Domain.Entity.Snapshot;

namespace Poker.Core.Domain.Entity
{
    public class Card
    {
        public Card(EValue value, EColor color)
        {
            Value = value;
            Color = color;
        }

        private Card()
        {

        }

        public EValue Value { get; private set; }
        public EColor Color { get; private set; }

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

        public static bool operator ==(Card? obj1, Card? obj2)
        {
            if (ReferenceEquals(obj1, obj2))
                return true;
            if (obj1 is null)
                return false;
            if (obj2 is null)
                return false;
            return obj1.Equals(obj2);
        }
        public static bool operator !=(Card? obj1, Card? obj2)
        => !(obj1 == obj2);

        public static Card FromString(string input)
        {
            var rx = new Regex(@"[0-9AKQJ]+");
            var value = GetValue(rx.Match(input).Value);
            var color = GetColor(input.Last());
            return new Card(value, color);

            EValue GetValue(string value)
            {
                switch (value)
                {
                    case "A":
                        return EValue.Ace;
                    case "K":
                        return EValue.King;
                    case "Q":
                        return EValue.Queen;
                    case "J":
                        return EValue.Jack;
                }
                var number = int.Parse(value);
                if (number < 2 || number > 10)
                    throw new WrongCardValueException(input);
                return (EValue)number;
            }

            EColor GetColor(char color)
            {
                return color switch
                {
                    '♣' => EColor.Clubs,
                    '♠' => EColor.Spades,
                    '♦' => EColor.Diamonds,
                    '♥' => EColor.Hearts,

                    'c' => EColor.Clubs,
                    's' => EColor.Spades,
                    'd' => EColor.Diamonds,
                    'h' => EColor.Hearts,

                    _ => throw new WrongCardValueException(input),
                };
            }
        }

        public CardSnapshot ToSnapshot()
        {
            return new CardSnapshot()
            {
                Color = this.Color,
                Value = this.Value
            };
        }

        public static Card EmptyCard
            => new Card();

        public Card FromSnapshot(CardSnapshot snapshot)
        {
            this.Color = snapshot.Color;
            this.Value = snapshot.Value;
            return this;
        }
    }
}
