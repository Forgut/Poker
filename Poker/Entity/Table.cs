using System.Collections.Generic;
using System.Text;

namespace Poker.Entity
{
    public class Table
    {
        public Table(IEnumerable<Player> players)
        {
            Cards = new Card[5];
            Players = players;
        }
        public Card[] Cards { get; }
        public IEnumerable<Player> Players { get; }

        public string GetTableState()
        {
            var sb = new StringBuilder();
            sb.Append("Table: ");
            foreach (var card in Cards)
            {
                if (card == null)
                    sb.Append("X ");
                else
                    sb.Append($"{card} ");
            }
            sb.AppendLine();

            sb.AppendLine("Players:");
            foreach(var player in Players)
                sb.AppendLine($"{player.Name}: ({player.Money}), {player.Cards[0]} {player.Cards[1]}");

            return sb.ToString();
        }
    }
}
