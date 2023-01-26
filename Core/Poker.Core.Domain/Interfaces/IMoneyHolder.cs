namespace Poker.Core.Domain.Interfaces
{
    public interface IMoneyHolder : IPlayer
    {
        int Money { get; }
        public int TakeMoney(int amount);
        public void AddMoney(int amount);
    }
}
