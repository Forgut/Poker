using NSubstitute;
using Poker.Core.Application.Betting;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Interfaces;
using Xunit;

namespace Poker.Tests.UnitTests
{
    public class WinMoneyDistributorTests
    {
        [Fact]
        public void Should_distribute_money_equally_if_possible()
        {
            var player1 = Substitute.For<IMoneyHolder>();
            var player2 = Substitute.For<IMoneyHolder>();

            WinMoneyDistributor.DistributeMoney(100, new IMoneyHolder[] { player1, player2 });

            player1.Received(1).AddMoney(50);
            player2.Received(1).AddMoney(50);
        }

        [Fact]
        public void Should_distribute_money_equally_leaving_not_dividable_money_to_noone()
        {
            var player1 = Substitute.For<IMoneyHolder>();
            var player2 = Substitute.For<IMoneyHolder>();

            WinMoneyDistributor.DistributeMoney(101, new IMoneyHolder[] { player1, player2 });

            player1.Received(1).AddMoney(50);
            player2.Received(1).AddMoney(50);
        }
    }
}
