using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.Betting.BetOrder
{
    public interface IPlayersRotation
    {
        IMoneyHolder BigBlindPlayer { get; }
        IMoneyHolder CurrentPlayer { get; }
        IMoneyHolder SmallBlindPlayer { get; }

        IEnumerable<IPlayer> GetNotFoldedPlayers();
        bool IsBettingOver();
        void MarkCurrentPlayerAsFinished();
        void MarkCurrentPlayerAsFolded();
        void MarkNotFoldedPlayersAsNotFinished();
        void MoveBlinds();
        void MoveToNextNotFoldedPlayer();
        void MoveToPlayerAfterBigBlind();
        void MoveToSmallBlind();
        void ResetPlayersTurns();
        void TakeBigBlindMoney(int amount);
        void TakeSmallBlindMoney(int amount);
        void TakeCurrentPlayerMoney(int amount);
    }

    public class PlayersRotation : IPlayersRotation
    {
        private readonly List<PlayerInfo> _playerInfos;
        private RecurringIndex _currentPlayerIndex;
        private RecurringIndex _bigBlindIndex;
        private RecurringIndex _smallBlindIndex;

        public PlayersRotation(List<IMoneyHolder> players)
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

        public IMoneyHolder CurrentPlayer
            => _playerInfos[_currentPlayerIndex.Value].Player;

        public IMoneyHolder BigBlindPlayer
            => _playerInfos[_bigBlindIndex.Value].Player;

        public IMoneyHolder SmallBlindPlayer
            => _playerInfos[_smallBlindIndex.Value].Player;

        public void MoveToNextNotFoldedPlayer()
        {
            int maxIterations = _playerInfos.Count;
            do
            {
                if (maxIterations <= 0)
                    break;
                _currentPlayerIndex.Value++;
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
            _smallBlindIndex.Value++;
            _bigBlindIndex.Value++;
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

        public void MoveToPlayerAfterBigBlind()
        {
            _currentPlayerIndex.Value = _bigBlindIndex.Value + 1;
        }

        public void MoveToSmallBlind()
        {
            _currentPlayerIndex.Value = _smallBlindIndex.Value;
        }

        public IEnumerable<IPlayer> GetNotFoldedPlayers()
        {
            return _playerInfos.Where(x => !x.HasFolded)
                .Select(x => x.Player);
        }

        public void TakeBigBlindMoney(int amount)
        {
            BigBlindPlayer.TakeMoney(amount);
        }

        public void TakeSmallBlindMoney(int amount)
        {
            SmallBlindPlayer.TakeMoney(amount);
        }

        public void TakeCurrentPlayerMoney(int amount)
        {
            CurrentPlayer.TakeMoney(amount);
        }
    }
}
