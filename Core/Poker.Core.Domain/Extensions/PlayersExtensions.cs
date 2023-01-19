using Poker.Core.Domain.Entity;
using System.Collections.Generic;

namespace Poker.Core.Domain.Extensions
{
    public static class PlayersExtensions
    {
        public static Players ToPlayers(this IEnumerable<Player> collection)
        {
            var players = new Players();
            players.AddRange(collection);
            return players;
        }
    }
}
