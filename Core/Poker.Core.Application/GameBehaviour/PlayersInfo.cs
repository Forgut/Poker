using Poker.Core.Domain.Entity;
using System.Linq;

namespace Poker.Core.Application.GameBehaviour
{
    public class PlayersInfo
    {
        public Players Players { get; private set; }
        public PlayersInfo(Players players)
        {
            Players = players;
        }

        private Player? _targetPlayer;
        public Player TargetPlayer
        {
            get
            {
                if (_targetPlayer == null)
                    _targetPlayer = Players.First();
                return _targetPlayer;
            }
        }
        public bool SetTargetPlayer(string playerName)
        {
            var player = Players.FirstOrDefault(x => x.Name == playerName);
            if (player != null)
            {
                _targetPlayer = player;
                return true;
            }

            return false;
        }
    }
}
