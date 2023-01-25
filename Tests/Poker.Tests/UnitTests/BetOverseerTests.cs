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
        [Fact]
        public void DidExecuteBigBlindAndSmallBlind_Should_be_true_after_ExecuteBigAndSmallBlind_was_called()
        {
            var players = new Players()
            {
                new Player("Test1", 100),
                new Player("Test2", 100),
            };
            var betOverseer = new BetOverseer(players);
            Assert.False(betOverseer.DidExecuteBigBlindAndSmallBlind);
            betOverseer.ExecuteBigAndSmallBlind(2, 1);
            Assert.True(betOverseer.DidExecuteBigBlindAndSmallBlind);
        }

        [Fact]
        public void Should_properly_execute_initial_big_and_small_blind()
        {
            var smallBlind = new Player("Test1", 100);
            var bigBlind = new Player("Test2", 100);
            var firstAfterBigBlind = new Player("Test3", 100);
            var players = new Players()
            {
                smallBlind,
                bigBlind,
                firstAfterBigBlind,
            };
            var betOverseer = new BetOverseer(players);
            betOverseer.ExecuteBigAndSmallBlind(2, 1);
            Assert.Equal(99, smallBlind.Money);
            Assert.Equal(98, bigBlind.Money);
            Assert.Equal(100, firstAfterBigBlind.Money);
        }

        [Fact]
        public void GetCurrentlyBettingPlayer_should_point_to_first_player_after_big_blind_after_ExecuteBigAndSmallBlind()
        {
            var smallBlind = new Player("Test1", 100);
            var bigBlind = new Player("Test2", 100);
            var firstAfterBigBlind = new Player("Test3", 100);
            var players = new Players()
            {
                smallBlind,
                bigBlind,
                firstAfterBigBlind,
            };
            var betOverseer = new BetOverseer(players);
            betOverseer.ExecuteBigAndSmallBlind(2, 1);
            Assert.Equal(firstAfterBigBlind.Name, betOverseer.GetCurrentlyBettingPlayer());
        }

        [Fact]
        public void MoveBlinds_should_pass_blind_pointer_to_next_person()
        {
            var smallBlind = new Player("Test1", 100);
            var bigBlind = new Player("Test2", 100);
            var firstAfterBigBlind = new Player("Test3", 100);
            var players = new Players()
            {
                smallBlind,
                bigBlind,
                firstAfterBigBlind,
            };
            var betOverseer = new BetOverseer(players);

            Assert.Equal(smallBlind.Name, betOverseer.GetSmallBlindPlayer());
            Assert.Equal(bigBlind.Name, betOverseer.GetBigBlindPlayer());

            betOverseer.MoveBlinds();

            Assert.Equal(bigBlind.Name, betOverseer.GetSmallBlindPlayer());
            Assert.Equal(firstAfterBigBlind.Name, betOverseer.GetBigBlindPlayer());
        }
    }
}
