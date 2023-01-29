using Poker.Core.Application.Betting;
using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Application.Events;
using Poker.Core.Application.GameBehaviour.WinCalculation;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Events;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace Poker.Core.Application.GameBehaviour
{
    public class StandardGame : Game
    {
        private readonly IBetOverseer _betOverseer;
        public StandardGame(ICombinationComparer combinationComparer,
                    IWinChanceEstimator winChanceEstimator,
                    ITable table,
                    Players players,
                    IEventPublisher eventPublisher,
                    IBetOverseer betOverseer,
                    ICombinationFinder combinationFinder)
            : base(combinationComparer,
                  winChanceEstimator,
                  table,
                  players,
                  eventPublisher,
                  combinationFinder)
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
            var (betSuccess, isBetOver) = _betOverseer.ExecuteForCurrentPlayer(betDecision);
            if (isBetOver)
                GameState++;
            return (betSuccess, isBetOver);
        }

        public override string GameStateAsString()
        {
            var stateWithoutPotInfo = base.GameStateAsString();
            var sb = new StringBuilder(stateWithoutPotInfo);
            sb.AppendLine($"Pot value: {_betOverseer.GetTotalAmountOnPot()}");
            return sb.ToString();
        }

        public void EndRound()
        {
            var notFoldedPlayers = _betOverseer.GetNotFoldedPlayers().Cast<Player>(); //hack
            var winners = _winDecision.GetWinners(notFoldedPlayers)
                .Where(winner => notFoldedPlayers.Select(x => x.Name).Contains(winner.Player.Name)).ToList();

            PublishEvent(winners);
            WinMoneyDistributor.DistributeMoney(_betOverseer.GetTotalAmountOnPot(), winners.Select(x => x.Player));
            _betOverseer.MoveBlinds();
            GameState = EGameState.End;

            void PublishEvent(IEnumerable<Winner> winners)
            {
                var eventParameters = winners
                    .Select(x => new WinnerInfo(x.Player.Name, x.Combination));
                var @event = new RoundEndedEvent(eventParameters);
                _eventPublisher.RoundEnded(@event);
            }
        }

        public void ResetRound()
        {
            _table.ClearCards();
            _winDecision.ResetWinners();
            _betOverseer.ResetForNewGame();
            foreach (var player in _playersInfo.Players)
                player.ClearCards();
            GameState = EGameState.PreFlop;
        }

        public string GetWinnersAsString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Winners:");
            foreach (var winner in _winDecision.GetWinners(_betOverseer.GetNotFoldedPlayers().Cast<Player>())) //hack
                sb.Append($"{winner.Player.Name};");
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
