using Poker.Core.Application.Betting;
using Poker.Core.Domain.Entity;
using Xunit;

namespace Poker.Tests.UnitTests
{
    public class PlayersRotationTests
    {
        [Fact]
        public void Should_return_CurrentPlayer_correctly_after_calling_MoveToNextPlayer()
        {
            var players = new Players()
            {
                new Player("Test1", 10),
                new Player("Test2", 10),
            };
            var playersRotation = new PlayersRotation(players);
            Assert.Equal("Test1", playersRotation.CurrentPlayer.Name);
            playersRotation.MoveToNextPlayer();
            Assert.Equal("Test2", playersRotation.CurrentPlayer.Name);
            playersRotation.MoveToNextPlayer();
            Assert.Equal("Test1", playersRotation.CurrentPlayer.Name);
        }
    }
}
