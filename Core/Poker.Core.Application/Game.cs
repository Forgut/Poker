using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Application.Events;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poker.Core.Application
{
    public class Game
    {
        private readonly Table _table;
        private readonly CombinationComparer _combinationComparer;
        private readonly WinEstimator _winEstimator;
        private readonly Players _players;
        private readonly IEventPublisher _eventPublisher;

        private Player _targetPlayer;

        public List<Player> Winners { get; private set; }
        public EGameState GameState { get; private set; }

        public Game(CombinationComparer combinationComparer,
                    WinEstimator winEstimator,
                    Table table,
                    Players players,
                    IEventPublisher eventPublisher)
        {
            _combinationComparer = combinationComparer;
            _winEstimator = winEstimator;
            _table = table;
            Winners = new List<Player>();
            _players = players;
            _targetPlayer = players.First();
            _eventPublisher = eventPublisher;
        }

        public bool SetTargetPlayer(string playerName)
        {
            var player = _players.FirstOrDefault(x => x.Name == playerName);
            if (player != null)
            {
                _targetPlayer = player;
                return true;
            }

            _targetPlayer = _players.First();
            return false;
        }

        public void ResetRound()
        {
            _table.ClearCards();
            foreach (var player in _players)
                player.ClearCards();
            Winners.Clear();
            GameState = EGameState.New;
        }

        public void InsertTargetPlayerCards(string cards, char separator = ';')
        {
            var split = cards.Split(separator);
            _targetPlayer.SetFirstCard(Card.FromString(split[0]));
            _targetPlayer.SetSecondCard(Card.FromString(split[1]));
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
            var player = _players.First(x => x.Name == playerName);
            var split = cards.Split(separator);
            player.SetFirstCard(Card.FromString(split[0]));
            player.SetSecondCard(Card.FromString(split[1]));
        }

        public void PrintGameState()
        {
            Console.WriteLine();
            var sb = new StringBuilder();
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
            foreach (var player in _players)
                sb.AppendLine($"{player.Name}: {player.Cards[0]} {player.Cards[1]}");

            Console.WriteLine(sb.ToString());
        }

        public void PrintWinProbabilities()
        {
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
                    PrintRiver();
                    break;
                case EGameState.End:
                    break;
            }

            void PrintFlop()
            {
                if (_targetPlayer == null)
                {
                    Console.WriteLine("Target player not specified");
                    return;
                }

                var flopProb = _winEstimator
                        .ProbableCombinationsForPlayer2Missing(_table, _targetPlayer);

                var flopEnemyProb = _winEstimator
                    .ProbableCombinationsForEnemy2Missing(_table, _targetPlayer);

                Console.WriteLine($"Flop probability for {_targetPlayer.Name}");
                Console.WriteLine(flopProb);
                Console.WriteLine();

                Console.WriteLine($"Flop probability for {_targetPlayer.Name} enemies");
                Console.WriteLine(flopEnemyProb);
                Console.WriteLine();
            }

            void PrintTurn()
            {
                if (_targetPlayer == null)
                {
                    Console.WriteLine("Target player not specified");
                    return;
                }

                var turnProb = _winEstimator
                .ProbableCombinationsForPlayer1Missing(_table, _targetPlayer);

                var turnEnemyProb = _winEstimator
                    .ProbableCombinationsForEnemy1Missing(_table, _targetPlayer);

                Console.WriteLine($"Turn probability for {_targetPlayer.Name}");
                Console.WriteLine(turnProb);
                Console.WriteLine();

                Console.WriteLine($"Turn probability for {_targetPlayer.Name} enemies");
                Console.WriteLine(turnEnemyProb);
                Console.WriteLine();
            }

            void PrintRiver()
            {
                var playersCombinations = _players
                .Where(x => x.HasCards)
                .Select(x => new
                {
                    Player = x,
                    Combination = new CombinationFinder(x, _table).GetBestCombination()
                });

                foreach (var playerCombination in playersCombinations)
                    Console.WriteLine($"{playerCombination.Player.Name}: {playerCombination.Combination}");
            }
        }

        public void PrintWinner()
        {
            Console.WriteLine("Winners:");
            foreach (var winner in Winners)
                Console.Write($"{winner.Name};");
            Console.WriteLine();
        }

        public void EndRound()
        {
            var playersCombinations = _players
                .Where(x => x.HasCards)
                .Select(x => new
                {
                    Player = x,
                    Combination = new CombinationFinder(x, _table).GetBestCombination()
                });

            var winners = _combinationComparer
               .GetBestCombinations(playersCombinations.Select(x => x.Combination));

            var winnersAndCombinations = playersCombinations.Where(x => winners.Contains(x.Combination))
                .Select(x => new { x.Player, x.Combination });

            PublishEvent();

            Winners = winnersAndCombinations.Select(x => x.Player)
                .ToList();

            GameState = EGameState.End;

            void PublishEvent()
            {
                var eventParameters = winnersAndCombinations
                    .Select(x => new WinnerInfo(x.Player.Name, x.Combination.ToString()));
                var @event = new RoundEndedEvent(eventParameters);
                _eventPublisher.RoundEnded(@event);
            }

        }


    }
}
