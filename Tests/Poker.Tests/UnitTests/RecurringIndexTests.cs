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
            recurringIndex++;
            Assert.Equal(1, recurringIndex.Value);
            recurringIndex++;
            Assert.Equal(2, recurringIndex.Value);
            recurringIndex++;
            Assert.Equal(0, recurringIndex.Value);
        }
    }
}
