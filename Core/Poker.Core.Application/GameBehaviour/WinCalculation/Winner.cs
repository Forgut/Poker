using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Interfaces;

namespace Poker.Core.Application.GameBehaviour.WinCalculation
{
    public class Winner
    {
        public Winner(ICardsHolder player, string combination)
        {
            Player = player;
            Combination = combination;
        }

        public ICardsHolder Player { get; set; }
        public string Combination { get; }
    }
}
