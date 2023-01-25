using Poker.Core.Domain.Entity;

namespace Poker.Core.Application.Betting.BetOrder
{
    class PlayerInfo
    {
        public PlayerInfo(Player player)
        {
            Player = player;
        }

        public Player Player { get; }
        public bool HasFolded { get; private set; }
        public void Fold() => HasFolded = true;
        public bool HasFinished { get; set; } = false;
    }
}
