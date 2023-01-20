namespace Poker.Core.Application.GameBehaviour.WinCalculation
{
    public class Winner
    {
        public Winner(string name, string combination)
        {
            Name = name;
            Combination = combination;
        }

        public string Name { get; }
        public string Combination { get; }
    }
}
