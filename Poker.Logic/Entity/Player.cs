using Poker.Logic.Cards.Entity;

namespace Poker.Entity
{
    public class Player
    {
        public Player(string name, int money)
        {
            Name = name;
            Money = InitialMoney = money;
            Cards = new Card[2];
        }
        public string Name { get; }
        public int Money { get; }
        public int InitialMoney { get; }
        public Card[] Cards { get; }
    }
}
