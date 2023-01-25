using Poker.Core.Application.Betting;
using Poker.Core.Domain.Entity;
using System.Linq;
using Xunit;

namespace Poker.Tests.UnitTests
{
    public class PlayersRotationTests
    {
        private readonly PlayersRotation _rotation;
        public PlayersRotationTests()
        {
            var players = new Players()
            {
                new Player("Test1", 10),
                new Player("Test2", 10),
            };
            _rotation = new PlayersRotation(players);
        }

        [Fact]
        public void Should_return_CurrentPlayer_correctly_after_calling_MoveToNextPlayer()
        {
            Assert.Equal("Test1", _rotation.CurrentPlayer.Name);
            _rotation.MoveToNextNotFoldedPlayer();
            Assert.Equal("Test2", _rotation.CurrentPlayer.Name);
            _rotation.MoveToNextNotFoldedPlayer();
            Assert.Equal("Test1", _rotation.CurrentPlayer.Name);
        }

        [Fact]
        public void IsBettingOver_should_return_true_when_all_players_folded()
        {
            _rotation.MarkCurrentPlayerAsFolded();
            _rotation.MoveToNextNotFoldedPlayer();
            _rotation.MarkCurrentPlayerAsFolded();
            Assert.True(_rotation.IsBettingOver());
        }

        [Fact]
        public void IsBettingOver_should_return_true_when_all_players_finished()
        {
            _rotation.MarkCurrentPlayerAsFinished();
            _rotation.MoveToNextNotFoldedPlayer();
            _rotation.MarkCurrentPlayerAsFinished();
            Assert.True(_rotation.IsBettingOver());
        }

        [Fact]
        public void IsBettingOver_should_return_true_when_all_players_finished_or_folded()
        {
            _rotation.MarkCurrentPlayerAsFolded();
            _rotation.MoveToNextNotFoldedPlayer();
            _rotation.MarkCurrentPlayerAsFinished();
            Assert.True(_rotation.IsBettingOver());
        }

        [Fact]
        public void IsBettingOver_should_return_false_when_not_all_players_finished_nor_folded()
        {
            _rotation.MarkCurrentPlayerAsFolded();
            _rotation.MoveToNextNotFoldedPlayer();
            Assert.False(_rotation.IsBettingOver());
        }

        [Fact]
        public void MoveToNextNotFoldedPlayer_should_skip_players_that_folded()
        {
            Assert.Equal("Test1", _rotation.CurrentPlayer.Name);
            _rotation.MarkCurrentPlayerAsFolded();
            _rotation.MoveToNextNotFoldedPlayer();
            Assert.Equal("Test2", _rotation.CurrentPlayer.Name);
            _rotation.MoveToNextNotFoldedPlayer();
            Assert.Equal("Test2", _rotation.CurrentPlayer.Name);
        }

        [Fact]
        public void GetNotFoldedPlayers_should_return_only_not_folded_players()
        {
            var preFoldResult = _rotation.GetNotFoldedPlayers();
            Assert.Contains("Test1", preFoldResult.Select(x => x.Name));
            Assert.Contains("Test2", preFoldResult.Select(x => x.Name));
            _rotation.MarkCurrentPlayerAsFolded();
            var postFoldResult = _rotation.GetNotFoldedPlayers();
            Assert.DoesNotContain("Test1", postFoldResult.Select(x => x.Name));
            Assert.Contains("Test2", postFoldResult.Select(x => x.Name));
        }
    }
}
