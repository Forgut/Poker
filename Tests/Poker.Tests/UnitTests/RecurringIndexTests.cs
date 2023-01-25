using Xunit;
using static Poker.Core.Application.Betting.PlayersRotation;

namespace Poker.Tests.UnitTests
{
    public class RecurringIndexTests
    {
        [Fact]
        public void Should_overlap_after_meeting_max_value()
        {
            var recurringIndex = new RecurringIndex(3);
            Assert.Equal(0, recurringIndex.Value);
            recurringIndex.Value++;
            Assert.Equal(1, recurringIndex.Value);
            recurringIndex.Value++;
            Assert.Equal(2, recurringIndex.Value);
            recurringIndex.Value++;
            Assert.Equal(0, recurringIndex.Value);
        }

        [Fact]
        public void Should_overlap_proper_amount_of_times_if_exceeds_max_value()
        {
            var recurringIndex = new RecurringIndex(3);
            recurringIndex.Value += 2;
            Assert.Equal(2, recurringIndex.Value);
            recurringIndex.Value += 2;
            Assert.Equal(1, recurringIndex.Value);
            recurringIndex.Value += 2;
            Assert.Equal(0, recurringIndex.Value);
        }
    }
}
