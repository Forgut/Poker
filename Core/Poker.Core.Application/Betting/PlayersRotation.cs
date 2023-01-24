using Poker.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.Betting
{
    public class PlayersRotation
    {
        private readonly List<PlayerInfo> _playerInfos;
        private RecurringIndex _currentPlayerIndex;

        public PlayersRotation(Players players)
        {
            _playerInfos = players
                .Select(x => new PlayerInfo(x))
                .ToList();
            _currentPlayerIndex = new RecurringIndex(_playerInfos.Count);
        }

        public Player CurrentPlayer 
            => _playerInfos[_currentPlayerIndex.Value].Player;

        public void MoveToNextPlayer()
        {
            _currentPlayerIndex++;
        }

        public void CurrentPlayerFolded()
        {
            _playerInfos[_currentPlayerIndex.Value].Fold();
        }

        public void MoveBlinds()
        {
            throw new NotImplementedException();
        }

        class PlayerInfo
        {
            public PlayerInfo(Player player)
            {
                Player = player;
            }

            public Player Player { get; }
            public bool HasFolded { get; private set; }
            public void Fold() => HasFolded = true;
        }

        public struct RecurringIndex
        {
            private readonly int _maxValue;

            public RecurringIndex(int maxValue) : this()
            {
                _maxValue = maxValue;
            }

            public int Value { get; set; }
            public static RecurringIndex operator ++(RecurringIndex index)
            {
                index.Value++;
                if (index.Value >= index._maxValue)
                    index.Value = 0;
                return index;
            }
        }
    }
}
