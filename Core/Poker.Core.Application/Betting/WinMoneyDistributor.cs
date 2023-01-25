using Poker.Core.Domain.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.Betting
{
    public class WinMoneyDistributor
    {
        public static void DistributeMoney(int totalPotValue, IEnumerable<Player> winners)
        {
            var winShare = totalPotValue / winners.Count();
            foreach(var player in winners)
                player.AddMoney(winShare);
        }
    }
}
