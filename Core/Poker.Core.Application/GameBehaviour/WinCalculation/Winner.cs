using Poker.Core.Domain.Entity;

namespace Poker.Core.Application.GameBehaviour.WinCalculation
{
    public class Winner
    {
        public Winner(Player player, string combination)
        {
            Player = player;
            Combination = combination;
        }

        public Player Player { get; set; }
        public string Combination { get; }
    }
}
