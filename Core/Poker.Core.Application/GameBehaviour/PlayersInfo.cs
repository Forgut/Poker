using Poker.Core.Domain.Entity;
using System.Linq;

namespace Poker.Core.Application.GameBehaviour
{
    public interface IPlayersInfo
    {
        Players Players { get; }
        Player TargetPlayer { get; }

        bool SetTargetPlayer(string playerName);
    }

    public class PlayersInfo : IPlayersInfo
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
