using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Application.Events;
using Poker.Core.Application.GameBehaviour.WinCalculation;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poker.Core.Application.GameBehaviour
{
    public class SimulationGame
    {
        private readonly Table _table;
        private readonly Croupier _croupier;
        private readonly WinChanceEstimator _winChanceEstimator;
        private readonly PlayersInfo _playersInfo;
        private readonly WinDecision _winDecision;
        private readonly IEventPublisher _eventPublisher;

        public EGameState GameState { get; private set; }

        public SimulationGame(Croupier croupier,
                    CombinationComparer combinationComparer,
                    WinChanceEstimator winChanceEstimator,
                    Table table,
                    Players players,
                    IEventPublisher eventPublisher)
        {
            _croupier = croupier;
            _winChanceEstimator = winChanceEstimator;
            _table = table;
            _playersInfo = new PlayersInfo(players);
            _winDecision = new WinDecision(table, players, combinationComparer);
            _eventPublisher = eventPublisher;
        }

        public bool SetTargetPlayer(string playerName)
        {
            return _playersInfo.SetTargetPlayer(playerName);
        }

        public void ResetRound()
        {
            _table.ClearCards();
            _winDecision.ResetWinners();
            foreach (var player in _playersInfo.Players)
                player.ClearCards();
            GameState = EGameState.New;
        }

        public void DrawPlayerCards()
        {
            _croupier.PreFlop(_playersInfo.Players);
            GameState = EGameState.PreFlop;
        }

        public void Flop()
        {
            _croupier.Flop(_table);
            GameState = EGameState.Flop;
        }

        public void Turn()
        {
            _croupier.Turn(_table);
            GameState = EGameState.Turn;
        }

        public void River()
        {
            _croupier.River(_table);
            GameState = EGameState.River;
        }

        public string PrintGameState()
        {
            var sb = new StringBuilder();
            sb.AppendLine();
            sb.AppendLine($"Current stage: {GameState}");
            sb.Append("Table: ");
            foreach (var card in _table.Cards)
            {
                if (card == null)
                    sb.Append("X ");
                else
                    sb.Append($"{card} ");
            }
            sb.AppendLine();

            sb.AppendLine("Players:");
            foreach (var player in _playersInfo.Players)
                sb.AppendLine($"{player.Name}: {player.Cards[0]} {player.Cards[1]}");

            return sb.ToString();
        }

        public string GetWinProbabilitiesAsString()
        {
            var sb = new StringBuilder();
            switch (GameState)
            {
                case EGameState.New:
                    break;
                case EGameState.PreFlop:
                    break;
                case EGameState.Flop:
                    PrintFlop();
                    break;
                case EGameState.Turn:
                    PrintTurn();
                    break;
                case EGameState.River:
                case EGameState.End:
                    PrintRiver();
                    break;
            }

            return sb.ToString();

            void PrintFlop()
            {
                if (_playersInfo.TargetPlayer == null)
                {
                    sb.AppendLine("Target player not specified");
                    return;
                }

                var flopProb = _winChanceEstimator
                        .ProbableCombinationsForPlayer2Missing(_table, _playersInfo.TargetPlayer);

                var flopEnemyProb = _winChanceEstimator
                    .ProbableCombinationsForEnemy2Missing(_table, _playersInfo.TargetPlayer);

                sb.AppendLine($"Flop probability for {_playersInfo.TargetPlayer.Name}");
                sb.AppendLine(flopProb.ToString());
                sb.AppendLine();

                sb.AppendLine($"Flop probability for {_playersInfo.TargetPlayer.Name} enemies");
                sb.AppendLine(flopEnemyProb.ToString());
                sb.AppendLine();
            }

            void PrintTurn()
            {
                if (_playersInfo.TargetPlayer == null)
                {
                    sb.AppendLine("Target player not specified");
                    return;
                }

                var turnProb = _winChanceEstimator
                .ProbableCombinationsForPlayer1Missing(_table, _playersInfo.TargetPlayer);

                var turnEnemyProb = _winChanceEstimator
                    .ProbableCombinationsForEnemy1Missing(_table, _playersInfo.TargetPlayer);

                sb.AppendLine($"Turn probability for {_playersInfo.TargetPlayer.Name}");
                sb.AppendLine(turnProb.ToString());
                sb.AppendLine();

                sb.AppendLine($"Turn probability for {_playersInfo.TargetPlayer.Name} enemies");
                sb.AppendLine(turnEnemyProb.ToString());
                sb.AppendLine();
            }

            void PrintRiver()
            {
                var playersCombinations =
                _playersInfo.Players.Select(x => new
                {
                    Player = x,
                    Combination = new CombinationFinder(x, _table).GetBestCombination()
                });

                foreach (var playerCombination in playersCombinations)
                    sb.AppendLine($"{playerCombination.Player.Name}: {playerCombination.Combination}");
            }
        }

        public string GetWinnersAsString()
        {
            var sb = new StringBuilder();
            sb.AppendLine("Winners:");
            foreach (var winner in _winDecision.Winners)
                sb.Append($"{winner.Name};");
            sb.AppendLine();
            return sb.ToString();
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
    }
}
