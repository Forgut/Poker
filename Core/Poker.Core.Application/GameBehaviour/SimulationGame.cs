using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Application.Events;
using Poker.Core.Application.GameBehaviour.WinCalculation;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Events;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poker.Core.Application.GameBehaviour
{
    public class SimulationGame : Game
    {
        private readonly Croupier _croupier;

        public SimulationGame(Croupier croupier,
                    CombinationComparer combinationComparer,
                    WinChanceEstimator winChanceEstimator,
                    Table table,
                    Players players,
                    IEventPublisher eventPublisher)
            : base(combinationComparer,
                 winChanceEstimator,
                 table,
                 players,
                 eventPublisher)
        {
            _croupier = croupier;
        }

        public void DrawPlayerCards()
        {
            _croupier.PreFlop(_playersInfo.Players);
            GameState = EGameState.Flop;
        }

        public void Flop()
        {
            _croupier.Flop(_table);
            GameState = EGameState.Turn;
        }

        public void Turn()
        {
            _croupier.Turn(_table);
            GameState = EGameState.River;
        }

        public void River()
        {
            _croupier.River(_table);
            GameState = EGameState.End;
        }

        public void EndRound()
        {
            var winners = _winDecision.GetWinners(_playersInfo.Players).ToList();

            PublishEvent(winners);
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
            foreach (var player in _playersInfo.Players)
                player.ClearCards();
            GameState = EGameState.PreFlop;
        }

        public string GetWinnersAsString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Winners:");
            foreach (var winner in _winDecision.GetWinners(_playersInfo.Players))
                sb.Append($"{winner.Player.Name};");
            sb.AppendLine();
            return sb.ToString();
        }
    }
}
