using NSubstitute;
using Poker.Core.Application;
using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Application.Events;
using Poker.Core.Application.GameBehaviour;
using Poker.Core.Domain.Entity;
using Xunit;

namespace Poker.Tests.IntegrationTests
{
    public class StandardGameTests
    {
        private readonly StandardGame _game;
        private const string Player1Name = "Player1";
        private const string Player2Name = "Player2";
        private const string Player3Name = "Player3";
        private const string Player4Name = "Player4";

        public StandardGameTests()
        {
            var combinationComparer = new CombinationComparer();
            var winChanceEstimator = new WinChanceEstimator();
            var table = new Table();
            var eventPublisher = Substitute.For<IEventPublisher>();
            var players = new Players()
            {
                new Player(Player1Name, 100),
                new Player(Player2Name, 100),
                new Player(Player3Name, 100),
                new Player(Player4Name, 100),
            };
            _game = new StandardGame(combinationComparer,
                                     winChanceEstimator,
                                     table,
                                     players,
                                     eventPublisher);
        }

        [Theory]
        [InlineData("Ac;Ah", "9h;8h", "As;Js", "3h;4h", "Kh;Qh;Jh", "10h", "3h", Player1Name)]
        [InlineData("2h;Ah", "9h;8h", "As;Js", "3h;4h", "Kh;Qh;Jh", "10h", "3h", Player1Name)]
        [InlineData("2h;Ah", "9h;8h", "As;Js", "", "Kh;Qh;Jh", "10h", "3h", Player1Name)]
        public void Should_properly_play_the_game_and_select_proper_winners(string targetPlayerCards,
            string player2Cards,
            string player3Cards,
            string player4Cards,
            string flop,
            string turn,
            string river,
            string expectedWinnerName)
        {
            _game.ResetRound();
            _game.InsertTargetPlayerCards(targetPlayerCards);
            _game.Flop(flop);
            _game.Turn(turn);
            _game.River(river);
            if (!string.IsNullOrEmpty(player2Cards))
                _game.FillPlayersCards(Player2Name, player2Cards);
            if (!string.IsNullOrEmpty(player3Cards))
                _game.FillPlayersCards(Player3Name, player3Cards);
            if (!string.IsNullOrEmpty(player4Cards))
                _game.FillPlayersCards(Player4Name, player4Cards);
            _game.EndRound();

            Assert.Contains(expectedWinnerName, _game.GetWinnersAsString());
        }
    }
}
