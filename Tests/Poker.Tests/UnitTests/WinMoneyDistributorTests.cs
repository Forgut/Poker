using Poker.Core.Application.Betting;
using Poker.Core.Domain.Entity;
using Xunit;

namespace Poker.Tests.UnitTests
{
    public class WinMoneyDistributorTests
    {
        [Fact]
        public void Should_distribute_money_equally_if_possible()
        {
            var player1 = new Player("Test1", 0); 
            var player2 = new Player("Test2", 0); 

            WinMoneyDistributor.DistributeMoney(100, new Player[] {player1, player2 });

            Assert.Equal(50, player1.Money);
            Assert.Equal(50, player2.Money);
        }

        [Fact]
        public void Should_distribute_money_equally_leaving_not_dividable_money_to_noone()
        {
            var player1 = new Player("Test1", 0);
            var player2 = new Player("Test2", 0);

            WinMoneyDistributor.DistributeMoney(101, new Player[] { player1, player2 });

            Assert.Equal(50, player1.Money);
            Assert.Equal(50, player2.Money);
        }
    }
}
