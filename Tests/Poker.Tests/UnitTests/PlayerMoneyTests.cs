using Poker.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Poker.Tests.UnitTests
{
    public class PlayerMoneyTests
    {
        [Fact]
        public void Should_Add_Money()
        {
            var player = new Player("test", 100);
            player.AddMoney(30);
            Assert.Equal(130, player.Money);
        }

        [Fact]
        public void Should_Remove_Money()
        {
            var player = new Player("test", 100);
            var moneyTaken = player.TakeMoney(30);
            Assert.Equal(70, player.Money);
            Assert.Equal(30, moneyTaken);
        }

        [Fact]
        public void Should_take_as_much_as_possible_if_amount_is_grater_than_money()
        {
            var player = new Player("test", 20);
            var moneyTaken = player.TakeMoney(30);
            Assert.Equal(20, moneyTaken);
            Assert.Equal(0, player.Money);
        }
    }
}
