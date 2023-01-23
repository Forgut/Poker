using Poker.Core.Application.Betting;
using Xunit;

namespace Poker.Tests.UnitTests
{
    public class DecisionParserTests
    {
        [Theory]
        [InlineData("casdad", Decision.Unkown, null)]
        [InlineData("asdasd:0", Decision.Unkown, null)]
        [InlineData("raise", Decision.Raise, null)]
        [InlineData("raise:10", Decision.Raise, 10)]
        [InlineData("check", Decision.Check, null)]
        [InlineData("check:10", Decision.Check, null)]
        [InlineData("fold:10", Decision.Fold, null)]
        [InlineData("fold", Decision.Fold, null)]
        public void Should_properly_parse_decision(string input, Decision expectedDecision, int? expectedAmount)
        {
            var (decision, amount) = DecisionParser.Parse(input);
            Assert.Equal(expectedDecision, decision);
            Assert.Equal(expectedAmount, amount);
        }
    }
}
