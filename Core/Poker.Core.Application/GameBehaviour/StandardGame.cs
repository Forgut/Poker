using Poker.Core.Application.Betting;
using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Application.Events;
using Poker.Core.Application.GameBehaviour.WinCalculation;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Events;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.GameBehaviour
{
    public class StandardGame : Game
    {
        private readonly BetOverseer _betOverseer;
        public StandardGame(CombinationComparer combinationComparer,
                    WinChanceEstimator winChanceEstimator,
                    Table table,
                    Players players,
                    IEventPublisher eventPublisher,
                    BetOverseer betOverseer)
            : base(combinationComparer,
                  winChanceEstimator,
                  table,
                  players,
                  eventPublisher)
        {
            _betOverseer = betOverseer;
        }

        public void InsertTargetPlayerCards(string cards, char separator = ';')
        {
            var split = cards.Split(separator);
            _playersInfo.TargetPlayer.SetFirstCard(Card.FromString(split[0]));
            _playersInfo.TargetPlayer.SetSecondCard(Card.FromString(split[1]));
            GameState = EGameState.PreFlopBet;
        }

        public void Flop(string cards, char separator = ';')
        {
            var split = cards.Split(separator);
            _table.SetFirstCard(Card.FromString(split[0]));
            _table.SetSecondCard(Card.FromString(split[1]));
            _table.SetThirdCard(Card.FromString(split[2]));
            GameState = EGameState.FlopBet;
        }

        public void Turn(string card)
        {
            _table.SetFourthCard(Card.FromString(card));
            GameState = EGameState.TurnBet;
        }

        public void River(string card)
        {
            _table.SetFifthCard(Card.FromString(card));
            GameState = EGameState.RiverBet;
        }

        public void SetGameStateAsEnd()
        {
            GameState = EGameState.End;
        }

        public void FillPlayersCards(string playerName, string cards, char separator = ';')
        {
            var player = _playersInfo.Players.First(x => x.Name == playerName);
            var split = cards.Split(separator);
            player.SetFirstCard(Card.FromString(split[0]));
            player.SetSecondCard(Card.FromString(split[1]));
        }

        public (string CurrentlyBettingPlayer, int AmountToCheck, int AmountOnThePot, string BigBlindPlayer, string SmallBlindPlayer) GetCurrentBetInfo()
        {
            var currentPlayer = _betOverseer.GetCurrentlyBettingPlayer();
            var amountToCheck = _betOverseer.GetAmountToCheck();
            var totalPotAmount = _betOverseer.GetTotalAmountOnPot();
            var bigBlind = _betOverseer.GetBigBlindPlayer();
            var smallBlind = _betOverseer.GetSmallBlindPlayer();
            return (currentPlayer, amountToCheck, totalPotAmount, bigBlind, smallBlind);
        }

        public bool ShouldExectueBigAndSmallBlindBet()
        {
            return !_betOverseer.DidExecuteBigBlindAndSmallBlind;
        }

        public void BetBigAndSmallBlind()
        {
            _betOverseer.ExecuteBigAndSmallBlind(2, 1);
        }

        public (bool BettingSucceeded, bool IsBettingOver) Bet(string betDecision)
        {
            var isSuccess = _betOverseer.ExecuteForCurrentPlayer(betDecision);
            var isBettingOver = _betOverseer.IsBettingOver();
            if (isBettingOver)
            {
                GameState++;
                _betOverseer.ResetForNextRound();
            }
            return (isSuccess, isBettingOver);
        }

        public void MoveBlinds()
        {
            _betOverseer.MoveBlinds();
        }

        public void EndRound()
        {
            var winners = _winDecision.Winners.ToList();

            PublishEvent(winners);
            GameState = EGameState.End;

            void PublishEvent(IEnumerable<Winner> winners)
            {
                var eventParameters = winners
                    .Select(x => new WinnerInfo(x.Name, x.Combination));
                var @event = new RoundEndedEvent(eventParameters);
                _eventPublisher.RoundEnded(@event);
            }
        }

        public void ResetRound()
        {
            _table.ClearCards();
            _winDecision.ResetWinners();
            foreach (var player in _playersInfo.Players)
                player.ClearCards();
            GameState = EGameState.PreFlop;
        }
    }
}
