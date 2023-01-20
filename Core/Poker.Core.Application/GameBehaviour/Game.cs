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
    public class Game
    {
        private readonly Table _table;
        private readonly WinChanceEstimator _winChanceEstimator;
        private readonly IEventPublisher _eventPublisher;
        private readonly PlayersInfo _playersInfo;
        private readonly WinDecision _winDecision;

        public EGameState GameState { get; private set; }

        public Game(CombinationComparer combinationComparer,
                    WinChanceEstimator winEstimator,
                    Table table,
                    Players players,
                    IEventPublisher eventPublisher)
        {
            _winChanceEstimator = winEstimator;
            _table = table;
            _winDecision = new WinDecision(table, players, combinationComparer);
            _playersInfo = new PlayersInfo(players);
            _eventPublisher = eventPublisher;
        }

        public bool SetTargetPlayer(string playerName)
        {
            return _playersInfo.SetTargetPlayer(playerName);
        }

        public void ResetRound()
        {
            _table.ClearCards();
            foreach (var player in _playersInfo.Players)
                player.ClearCards();
            GameState = EGameState.New;
        }

        public void InsertTargetPlayerCards(string cards, char separator = ';')
        {
            var split = cards.Split(separator);
            _playersInfo.TargetPlayer.SetFirstCard(Card.FromString(split[0]));
            _playersInfo.TargetPlayer.SetSecondCard(Card.FromString(split[1]));
            GameState = EGameState.PreFlop;
        }

        public void Flop(string cards, char separator = ';')
        {
            var split = cards.Split(separator);
            _table.SetFirstCard(Card.FromString(split[0]));
            _table.SetSecondCard(Card.FromString(split[1]));
            _table.SetThirdCard(Card.FromString(split[2]));
            GameState = EGameState.Flop;
        }

        public void Turn(string card)
        {
            _table.SetFourthCard(Card.FromString(card));
            GameState = EGameState.Turn;
        }

        public void River(string card)
        {
            _table.SetFifthCard(Card.FromString(card));
            GameState = EGameState.River;
        }

        public void ShowCards()
        {
            GameState = EGameState.ShowCards;
        }

        public void FillPlayersCards(string playerName, string cards, char separator = ';')
        {
            var player = _playersInfo.Players.First(x => x.Name == playerName);
            var split = cards.Split(separator);
            player.SetFirstCard(Card.FromString(split[0]));
            player.SetSecondCard(Card.FromString(split[1]));
        }

        public string GameStateAsString()
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

        public string GetWinnerAsString()
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
