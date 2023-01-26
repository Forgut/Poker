using NSubstitute;
using Poker.Core.Application.Betting.BetOrder;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace Poker.Tests.UnitTests
{
    public class PlayersRotationTests
    {
        private readonly PlayersRotation _rotation;
        private readonly IMoneyHolder _bigBlind;
        private readonly IMoneyHolder _smallBlind;

        private readonly string _bigBlindName = "BigBlind";
        private readonly string _smallBlindName = "SmallBlind";
        public PlayersRotationTests()
        {
            _bigBlind = Substitute.For<IMoneyHolder>();
            _bigBlind.Name.Returns(_bigBlindName);
            _smallBlind = Substitute.For<IMoneyHolder>();
            _smallBlind.Name.Returns(_smallBlindName);

            var moneyHolders = new List<IMoneyHolder>()
            {
                _smallBlind,
                _bigBlind,
            };

            _rotation = new PlayersRotation(moneyHolders);
        }

        [Fact]
        public void Should_return_CurrentPlayer_correctly_after_calling_MoveToNextPlayer()
        {
            Assert.Equal(_smallBlindName, _rotation.CurrentPlayer.Name);
            _rotation.MoveToNextNotFoldedPlayer();
            Assert.Equal(_bigBlindName, _rotation.CurrentPlayer.Name);
            _rotation.MoveToNextNotFoldedPlayer();
            Assert.Equal(_smallBlindName, _rotation.CurrentPlayer.Name);
        }

        [Fact]
        public void IsBettingOver_should_return_true_when_all_players_folded()
        {
            _rotation.MarkCurrentPlayerAsFolded();
            _rotation.MoveToNextNotFoldedPlayer();
            _rotation.MarkCurrentPlayerAsFolded();
            Assert.True(_rotation.IsBettingOver());
        }

        [Fact]
        public void IsBettingOver_should_return_true_when_all_players_finished()
        {
            _rotation.MarkCurrentPlayerAsFinished();
            _rotation.MoveToNextNotFoldedPlayer();
            _rotation.MarkCurrentPlayerAsFinished();
            Assert.True(_rotation.IsBettingOver());
        }

        [Fact]
        public void IsBettingOver_should_return_true_when_all_players_finished_or_folded()
        {
            _rotation.MarkCurrentPlayerAsFolded();
            _rotation.MoveToNextNotFoldedPlayer();
            _rotation.MarkCurrentPlayerAsFinished();
            Assert.True(_rotation.IsBettingOver());
        }

        [Fact]
        public void IsBettingOver_should_return_false_when_not_all_players_finished_nor_folded()
        {
            _rotation.MarkCurrentPlayerAsFolded();
            _rotation.MoveToNextNotFoldedPlayer();
            Assert.False(_rotation.IsBettingOver());
        }

        [Fact]
        public void MoveToNextNotFoldedPlayer_should_skip_players_that_folded()
        {
            Assert.Equal(_smallBlindName, _rotation.CurrentPlayer.Name);
            _rotation.MarkCurrentPlayerAsFolded();
            _rotation.MoveToNextNotFoldedPlayer();
            Assert.Equal(_bigBlindName, _rotation.CurrentPlayer.Name);
            _rotation.MoveToNextNotFoldedPlayer();
            Assert.Equal(_bigBlindName, _rotation.CurrentPlayer.Name);
        }

        [Fact]
        public void GetNotFoldedPlayers_should_return_only_not_folded_players()
        {
            var preFoldResult = _rotation.GetNotFoldedPlayers();
            Assert.Contains(_smallBlindName, preFoldResult.Select(x => x.Name));
            Assert.Contains(_bigBlindName, preFoldResult.Select(x => x.Name));
            _rotation.MarkCurrentPlayerAsFolded();
            var postFoldResult = _rotation.GetNotFoldedPlayers();
            Assert.DoesNotContain(_smallBlindName, postFoldResult.Select(x => x.Name));
            Assert.Contains(_bigBlindName, postFoldResult.Select(x => x.Name));
        }

        [Fact]
        public void TakeBigBlindMoney_should_take_money_from_big_blind_player()
        {
            _rotation.TakeBigBlindMoney(5);
            _bigBlind.Received(1).TakeMoney(5);
        }

        [Fact]
        public void TakeSmallBlindMoney_should_take_money_from_small_blind_player()
        {
            _rotation.TakeSmallBlindMoney(5);
            _smallBlind.Received(1).TakeMoney(5);
        }
    }
}
