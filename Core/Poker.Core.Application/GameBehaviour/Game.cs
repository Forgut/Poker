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
    public abstract class Game
    {
        protected readonly ITable _table;
        protected readonly IWinChanceEstimator _winChanceEstimator;
        protected readonly IPlayersInfo _playersInfo;
        protected readonly IWinDecision _winDecision;
        protected readonly IEventPublisher _eventPublisher;

        protected Game(
            ICombinationComparer combinationComparer,
            IWinChanceEstimator winChanceEstimator,
            ITable table,
            Players players,
            IEventPublisher eventPublisher)
        {
            _table = table;
            _winChanceEstimator = winChanceEstimator;
            _playersInfo = new PlayersInfo(players);
            _winDecision = new WinDecision(table, combinationComparer);
            _eventPublisher = eventPublisher;
        }

        public EGameState GameState { get; internal set; } //todo wyniesc to do GameState,
        //tutaj jest to leaking abstraction

        public bool SetTargetPlayer(string playerName)
        {
            return _playersInfo.SetTargetPlayer(playerName);
        }

        public virtual string GameStateAsString()
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
                sb.AppendLine($"{player.Name} ({player.Money}): {player.Cards[0]} {player.Cards[1]}");

            return sb.ToString();
        }

        public string GetWinProbabilitiesAsString()
        {
            var sb = new StringBuilder();
            switch (GameState)
            {
                case EGameState.PreFlop:
                    break;
                case EGameState.Flop:
                    PrintFlop();
                    break;
                case EGameState.Turn:
                    PrintTurn();
                    break;
                case EGameState.River:
                case EGameState.ShowCards:
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
                var playersCombinations = _playersInfo.Players
                    .Where(x => x.HasCards)
                    .Select(x => new
                    {
                        Player = x,
                        Combination = new CombinationFinder(x, _table).GetBestCombination()
                    });

                foreach (var playerCombination in playersCombinations)
                    sb.AppendLine($"{playerCombination.Player.Name}: {playerCombination.Combination}");
            }
        }

        
    }
}
