using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.Betting
{
    public class WinMoneyDistributor
    {
        public static void DistributeMoney(int totalPotValue, IEnumerable<IMoneyHolder> winners)
        {
            var winShare = totalPotValue / winners.Count();
            foreach(var player in winners)
                player.AddMoney(winShare);
        }
    }
}
