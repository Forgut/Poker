namespace Poker.Core.Domain.Interfaces
{
    public interface IMoneyHolder
    {
        string Name { get; }
        int Money { get; }
        public int TakeMoney(int amount);
        public void AddMoney(int amount);
    }
}
