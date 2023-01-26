using NSubstitute;
using Poker.Core.Application.Betting;
using Poker.Core.Application.Betting.BetOrder;
using Poker.Core.Domain.Interfaces;
using Xunit;

namespace Poker.Tests.UnitTests
{
    public class BetOverseerTests
    {
        private readonly BetOverseer _betOverseer;

        private readonly IPlayersRotation _playersRotation;
        private readonly IPot _pot;

        public BetOverseerTests()
        {
            _playersRotation = Substitute.For<IPlayersRotation>();
            var currentPlayer = Substitute.For<IMoneyHolder>();
            currentPlayer.Money.Returns(100);
            _playersRotation.CurrentPlayer.Returns(currentPlayer);

            _pot = Substitute.For<IPot>();
            _betOverseer = new BetOverseer(_playersRotation, _pot);
        }

        [Fact]
        public void DidExecuteBigBlindAndSmallBlind_Should_be_true_after_ExecuteBigAndSmallBlind_was_called()
        {
            Assert.False(_betOverseer.DidExecuteBigBlindAndSmallBlind);
            _betOverseer.ExecuteBigAndSmallBlind(2, 1);
            Assert.True(_betOverseer.DidExecuteBigBlindAndSmallBlind);
        }

        [Fact]
        public void Should_properly_execute_initial_big_and_small_blind()
        {
            _betOverseer.ExecuteBigAndSmallBlind(2, 1);
            _playersRotation.Received(1).TakeBigBlindMoney(2);
            _playersRotation.Received(1).TakeSmallBlindMoney(1);
            _pot.Received(1).AddToPot(Arg.Any<string>(), 2);
            _pot.Received(1).AddToPot(Arg.Any<string>(), 1);
        }

        [Fact]
        public void ExecuteBigAndSmallBlind_should_result_in_moving_bet_round_to_first_player_after_big_blind()
        {
            _betOverseer.ExecuteBigAndSmallBlind(2, 1);
            _playersRotation.Received(1).MoveToPlayerAfterBigBlind();
        }

        [Fact]
        public void MoveBlinds_should_pass_blind_pointer_to_next_person()
        {
            _betOverseer.MoveBlinds();
            _playersRotation.Received(1).MoveBlinds();
        }

        [Fact]
        public void ExecuteForCurrentPlayer_should_move_to_another_player_for_valid_decision()
        {
            _betOverseer.ExecuteForCurrentPlayer("raise:10");
            _playersRotation.Received(1).MoveToNextNotFoldedPlayer();
        }

        [Fact]
        public void ExecuteForCurrentPlayer_should_not_move_to_another_player_for_invalid_decision()
        {
            _betOverseer.ExecuteForCurrentPlayer("asdasdsa");
            _playersRotation.DidNotReceive().MoveToNextNotFoldedPlayer();
        }

        [Fact]
        public void ExecuteForCurrentPlayer_raise_should_remove_money_from_player_and_add_it_to_pot()
        {
            _betOverseer.ExecuteForCurrentPlayer("raise:10");
            _playersRotation.Received(1).TakeCurrentPlayerMoney(10);
            _pot.Received(1).AddToPot(Arg.Any<string>(), 10);
        }

        [Fact]
        public void ExecuteForCurrentPlayer_raise_should_mark_other_not_folded_players_as_not_finished()
        {
            _betOverseer.ExecuteForCurrentPlayer("raise:10");
            _playersRotation.Received(1).MarkNotFoldedPlayersAsNotFinished();
        }

        [Fact]
        public void ExecuteForCurrentPlayer_raise_should_mark_current_player_finished()
        {
            _betOverseer.ExecuteForCurrentPlayer("raise:10");
            _playersRotation.Received(1).MarkCurrentPlayerAsFinished();
        }

        [Fact]
        public void ExecuteForCurrentPlayer_check_should_remove_money_from_player_and_add_it_to_pot()
        {
            _betOverseer.ExecuteForCurrentPlayer("check");
            _playersRotation.Received(1).TakeCurrentPlayerMoney(Arg.Any<int>());
            _pot.Received(1).AddToPot(Arg.Any<string>(), Arg.Any<int>());
        }

        [Fact]
        public void ExecuteForCurrentPlayer_check_should_not_mark_other_not_folded_players_as_not_finished()
        {
            _betOverseer.ExecuteForCurrentPlayer("check");
            _playersRotation.DidNotReceive().MarkNotFoldedPlayersAsNotFinished();
        }

        [Fact]
        public void ExecuteForCurrentPlayer_check_should_mark_current_player_finished()
        {
            _betOverseer.ExecuteForCurrentPlayer("check");
            _playersRotation.Received(1).MarkCurrentPlayerAsFinished();
        }

        [Fact]
        public void ExecuteForCurrentPlayer_fold_should_not_change_pot_amount_nor_player_money()
        {
            _betOverseer.ExecuteForCurrentPlayer("fold");
            _playersRotation.DidNotReceive().TakeCurrentPlayerMoney(Arg.Any<int>());
            _pot.DidNotReceive().AddToPot(Arg.Any<string>(), Arg.Any<int>());
        }

        [Fact]
        public void ExecuteForCurrentPlayer_should_return_that_betting_is_over_if_all_players_folded_or_finished()
        {
            _playersRotation.IsBettingOver()
                .Returns(true);
            var (_, isBettingOver) = _betOverseer.ExecuteForCurrentPlayer("check");
            Assert.True(isBettingOver);
           
        }
    }
}
