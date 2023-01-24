﻿using Poker.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.Betting
{
    public class PlayersRotation
    {
        private readonly List<PlayerInfo> _playerInfos;
        private RecurringIndex _currentPlayerIndex;
        private RecurringIndex _bigBlindIndex;
        private RecurringIndex _smallBlindIndex;

        public PlayersRotation(Players players)
        {
            _playerInfos = players
                .Select(x => new PlayerInfo(x))
                .ToList();
            _currentPlayerIndex = new RecurringIndex(_playerInfos.Count);
            _currentPlayerIndex.Value = 0;
            _smallBlindIndex = new RecurringIndex(_playerInfos.Count);
            _smallBlindIndex.Value = 0;
            _bigBlindIndex = new RecurringIndex(_playerInfos.Count);
            _bigBlindIndex.Value = 1;
        }

        public Player CurrentPlayer
            => _playerInfos[_currentPlayerIndex.Value].Player;

        public Player BigBlindPlayer
            => _playerInfos[_bigBlindIndex.Value].Player;
        public Player SmallBlindPlayer
            => _playerInfos[_smallBlindIndex.Value].Player;

        public void MoveToNextNotFoldedPlayer()
        {
            int maxIterations = _playerInfos.Count;
            do
            {
                if (maxIterations <= 0)
                    break;
                _currentPlayerIndex++;
                maxIterations--;
            }
            while (_playerInfos[_currentPlayerIndex.Value].HasFolded);
        }

        public void MarkNotFoldedPlayersAsNotFinished()
        {
            foreach (var player in _playerInfos)
            {
                if (player.HasFolded)
                    continue;

                player.HasFinished = false;
            }
        }

        public void MarkCurrentPlayerAsFinished()
        {
            _playerInfos[_currentPlayerIndex.Value].HasFinished = true;
        }

        public void MarkCurrentPlayerAsFolded()
        {
            _playerInfos[_currentPlayerIndex.Value].Fold();
        }

        public void MoveBlinds()
        {
            _smallBlindIndex++;
            _bigBlindIndex++;
        }


        public bool IsBettingOver()
        {
            return _playerInfos.All(x => x.HasFolded || x.HasFinished);
        }

        public void ResetPlayersTurns()
        {
            foreach (var player in _playerInfos)
                player.HasFinished = false;
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
            public bool HasFinished { get; set; } = false;
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
