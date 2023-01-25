using Poker.Core.Application.Betting;
using Poker.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Sdk;

namespace Poker.Tests.UnitTests
{
    public class BetOverseerTests
    {
        private readonly BetOverseer _betOverseer;
        private readonly Player _smallBlind;
        private readonly Player _bigBlind;
        private readonly Player _firstAfterBigBlind;
        private const string _smallBlindName = "SmallBlind";
        private const string _bigBlindName = "BigBlind";
        private const string _firstAfterBigBlindName = "FirstAfterBigBlind";

        public BetOverseerTests()
        {
            _smallBlind = new Player(_smallBlindName, 100);
            _bigBlind = new Player(_bigBlindName, 100);
            _firstAfterBigBlind = new Player(_firstAfterBigBlindName, 100);
            var players = new Players()
            {
                _smallBlind,
                _bigBlind,
                _firstAfterBigBlind,
            };
            _betOverseer = new BetOverseer(players);
        }

        [Fact]
        public void DidExecuteBigBlindAndSmallBlind_Should_be_true_after_ExecuteBigAndSmallBlind_was_called()
        {
            Assert.False(_betOverseer.DidExecuteBigBlindAndSmallBlind);
            _betOverseer.ExecuteBigAndSmallBlind(2, 1);
            Assert.True(_betOverseer.DidExecuteBigBlindAndSmallBlind);
        }

        [Fact]
        public void Should_properly_execute_initial_big_and_small_blind()
        {

            _betOverseer.ExecuteBigAndSmallBlind(2, 1);
            Assert.Equal(99, _smallBlind.Money);
            Assert.Equal(98, _bigBlind.Money);
            Assert.Equal(100, _firstAfterBigBlind.Money);
        }

        [Fact]
        public void GetCurrentlyBettingPlayer_should_point_to_first_player_after_big_blind_after_ExecuteBigAndSmallBlind()
        {
            _betOverseer.ExecuteBigAndSmallBlind(2, 1);
            Assert.Equal(_firstAfterBigBlind.Name, _betOverseer.GetCurrentlyBettingPlayer());
        }

        [Fact]
        public void MoveBlinds_should_pass_blind_pointer_to_next_person()
        {
            Assert.Equal(_smallBlind.Name, _betOverseer.GetSmallBlindPlayer());
            Assert.Equal(_bigBlind.Name, _betOverseer.GetBigBlindPlayer());

            _betOverseer.MoveBlinds();

            Assert.Equal(_bigBlind.Name, _betOverseer.GetSmallBlindPlayer());
            Assert.Equal(_firstAfterBigBlind.Name, _betOverseer.GetBigBlindPlayer());
        }

        [Fact]
        public void ExecuteForCurrentPlayer_should_move_to_another_player_for_valid_decision()
        {
            Assert.Equal(_smallBlindName, _betOverseer.GetCurrentlyBettingPlayer());
            _betOverseer.ExecuteForCurrentPlayer("raise:10");
            Assert.Equal(_bigBlindName, _betOverseer.GetCurrentlyBettingPlayer());
        }

        [Fact]
        public void ExecuteForCurrentPlayer_should_not_move_to_another_player_for_invalid_decision()
        {
            Assert.Equal(_smallBlindName, _betOverseer.GetCurrentlyBettingPlayer());
            _betOverseer.ExecuteForCurrentPlayer("asdasdsa");
            Assert.Equal(_smallBlindName, _betOverseer.GetCurrentlyBettingPlayer());
        }

        [Fact]
        public void ExecuteForCurrentPlayer_raise_should_remove_money_from_player_and_add_it_to_pot()
        {
            _betOverseer.ExecuteForCurrentPlayer("raise:10");
            Assert.Equal(90, _smallBlind.Money);
            Assert.Equal(10, _betOverseer.GetTotalAmountOnPot());
        }

        [Fact]
        public void ExecuteForCurrentPlayer_check_should_remove_money_from_player_and_add_it_to_pot()
        {
            _betOverseer.ExecuteForCurrentPlayer("raise:10");
            _betOverseer.ExecuteForCurrentPlayer("check");
            Assert.Equal(90, _bigBlind.Money);
            Assert.Equal(20, _betOverseer.GetTotalAmountOnPot());
        }

        [Fact]
        public void ExecuteForCurrentPlayer_fold_should_not_change_pot_amount_nor_player_money()
        {
            var smallBlindMoney = _smallBlind.Money;
            var pot = _betOverseer.GetTotalAmountOnPot();
            _betOverseer.ExecuteForCurrentPlayer("fold");
            Assert.Equal(smallBlindMoney, _smallBlind.Money);
            Assert.Equal(pot, _betOverseer.GetTotalAmountOnPot());
        }

        [Fact]
        public void ExecuteForCurrentPlayer_should_return_that_betting_is_over_if_all_players_folded_or_finished()
        {
            bool isBettingOver = false;
            (_, isBettingOver) = _betOverseer.ExecuteForCurrentPlayer("check");
            Assert.False(isBettingOver);
            (_, isBettingOver) = _betOverseer.ExecuteForCurrentPlayer("fold");
            Assert.False(isBettingOver);
            (_, isBettingOver) = _betOverseer.ExecuteForCurrentPlayer("fold");
            Assert.True(isBettingOver);
        }
    }
}
