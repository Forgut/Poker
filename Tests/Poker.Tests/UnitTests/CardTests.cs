using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Exceptions;
using Xunit;

namespace Poker.Tests.UnitTests
{
    public class CardTests
    {
        [Theory]
        [InlineData("10c", EColor.Clubs, EValue.Ten)]
        [InlineData("6h", EColor.Hearts, EValue.Six)]
        [InlineData("3d", EColor.Diamonds, EValue.Three)]
        [InlineData("2d", EColor.Diamonds, EValue.Two)]
        [InlineData("Ad", EColor.Diamonds, EValue.Ace)]
        [InlineData("Jc", EColor.Clubs, EValue.Jack)]
        public void Should_be_able_to_create_card_with_these_parameters(string input,
            EColor expectedColor,
            EValue expectedValue)
        {
            var card = Card.FromString(input);
            Assert.Equal(expectedColor, card.Color);
            Assert.Equal(expectedValue, card.Value);
        }

        [Theory]
        [InlineData("11c")]
        [InlineData("1c")]
        [InlineData("6b")]
        [InlineData("4a")]
        [InlineData("cA")]
        [InlineData("hJ")]
        public void Should_not_be_able_to_create_card_with_these_parameters(string input)
        {
            Assert.Throws<WrongCardValueException>(() => Card.FromString(input));
        }
    }
}
