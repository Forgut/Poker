using Poker.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Logic.Entity
{
    public class Players : List<Player>
    {
    }

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
