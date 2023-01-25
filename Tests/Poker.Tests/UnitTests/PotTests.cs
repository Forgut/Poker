using Poker.Core.Application.Betting;
using Xunit;

namespace Poker.Tests.UnitTests
{
    public class PotTests
    {
        [Fact]
        public void Should_return_proper_amount_to_check()
        {
            var pot = new Pot();
            pot.AddToPot("Test1", 5);
            pot.AddToPot("Test2", 7);
            pot.AddToPot("Test1", 5);

            Assert.Equal(0, pot.AmountToCheck("Test1"));
            Assert.Equal(3, pot.AmountToCheck("Test2"));
            Assert.Equal(10, pot.AmountToCheck("Test3"));
        }

        [Fact]
        public void Should_properly_calculate_pot_total_value()
        {
            var pot = new Pot();
            Assert.Equal(0, pot.GetTotalAmount());
            pot.AddToPot("Test1", 3);
            Assert.Equal(3, pot.GetTotalAmount());
            pot.AddToPot("Test2", 6);
            Assert.Equal(9, pot.GetTotalAmount());
            pot.AddToPot("Test1", 3);
            Assert.Equal(12, pot.GetTotalAmount());
        }

        [Fact]
        public void Should_have_0_total_value_and_no_amount_to_check_after_reset()
        {
            var pot = new Pot();
            pot.AddToPot("Test1", 10);
            pot.Reset();
            Assert.Equal(0, pot.GetTotalAmount());
            Assert.Equal(0, pot.AmountToCheck("Test1"));
        }
    }
}
