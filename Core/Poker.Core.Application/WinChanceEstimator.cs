using Poker.Core.Application.CardBehaviour;
using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Common;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Poker.Core.Application
{
    public interface IWinChanceEstimator
    {
        CombinationEstimationResult ProbableCombinationsForEnemy1Missing(ITable table, ICardsHolder player);
        CombinationEstimationResult ProbableCombinationsForEnemy2Missing(ITable table, ICardsHolder player);
        CombinationEstimationResult ProbableCombinationsForPlayer1Missing(ITable table, ICardsHolder player);
        CombinationEstimationResult ProbableCombinationsForPlayer2Missing(ITable table, ICardsHolder player);
    }

    public class WinChanceEstimator : IWinChanceEstimator
    {
        private readonly ICombinationFinder _combinationFinder;

        public WinChanceEstimator(ICombinationFinder combinationFinder)
        {
            _combinationFinder = combinationFinder;
        }

        public CombinationEstimationResult ProbableCombinationsForPlayer2Missing(ITable table, ICardsHolder player)
        {
            var notAvailableCards = GetNotAvailableCards(table, player);

            var availableCards = GetAvailableCards(notAvailableCards).ToList();

            var possibleCombinations = new List<IEnumerable<Card>>();

            for (int i = 0; i < availableCards.Count; i++)
            {
                for (int j = i + 1; j < availableCards.Count; j++)
                {
                    var result = notAvailableCards.ToList();
                    result.Add(availableCards[i]);
                    result.Add(availableCards[j]);
                    possibleCombinations.Add(result);
                }
            }

            var combinationsDictionary = possibleCombinations
                .Select(x => new { Combination = x, Comb = _combinationFinder.GetBestCombination(x).Combination })
                .GroupBy(x => x.Comb)
                .Select(x => new { x.Key, Chance = x.Count() / (double)possibleCombinations.Count })
                .ToDictionary(keySelector: x => x.Key, elementSelector: chance => chance.Chance);

            return new CombinationEstimationResult(combinationsDictionary);
        }

        public CombinationEstimationResult ProbableCombinationsForPlayer1Missing(ITable table, ICardsHolder player)
        {
            var notAvailableCards = GetNotAvailableCards(table, player);

            var availableCards = GetAvailableCards(notAvailableCards);

            var possibleCombinations = new List<IEnumerable<Card>>();
            foreach (var card1 in availableCards)
            {
                var result = notAvailableCards.ToList();
                result.Add(card1);
                possibleCombinations.Add(result);
            }

            var combinationsDictionary = possibleCombinations
                .Select(x => new { Combination = x, Comb = _combinationFinder.GetBestCombination(x).Combination })
                .GroupBy(x => x.Comb)
                .Select(x => new { x.Key, Chance = x.Count() / (double)possibleCombinations.Count })
                .ToDictionary(keySelector: x => x.Key, elementSelector: chance => chance.Chance);

            return new CombinationEstimationResult(combinationsDictionary);
        }

        public CombinationEstimationResult ProbableCombinationsForEnemy2Missing(ITable table, ICardsHolder player)
        {
            var notAvailableCards = GetNotAvailableCards(table, player);

            var availableCards = GetAvailableCards(notAvailableCards).ToList();

            var possibleCombinations = new List<IEnumerable<Card>>();

            for (int i1 = 0; i1 < availableCards.Count(); i1++)
            {
                for (int i2 = i1 + 1; i2 < availableCards.Count(); i2++)
                {
                    for (int i3 = i2 + 1; i3 < availableCards.Count(); i3++)
                    {
                        for (int i4 = i3 + 1; i4 < availableCards.Count(); i4++)
                        {
                            var result = table.Cards.Where(x => x != null).ToList();
                            result.Add(availableCards[i1]);
                            result.Add(availableCards[i2]);
                            result.Add(availableCards[i3]);
                            result.Add(availableCards[i4]);
                            possibleCombinations.Add(result);
                        }
                    }
                }
            }

            var combinationsDictionary = possibleCombinations
                .Select(x => new { Combination = x, Comb = _combinationFinder.GetBestCombination(x).Combination })
                .GroupBy(x => x.Comb)
                .Select(x => new { x.Key, Chance = x.Count() / (double)possibleCombinations.Count })
                .ToDictionary(keySelector: x => x.Key, elementSelector: chance => chance.Chance);

            return new CombinationEstimationResult(combinationsDictionary);
        }

        public CombinationEstimationResult ProbableCombinationsForEnemy1Missing(ITable table, ICardsHolder player)
        {
            var notAvailableCards = GetNotAvailableCards(table, player);

            var availableCards = GetAvailableCards(notAvailableCards).ToList();
            var availableCards2 = GetAvailableCards(notAvailableCards).ToList();

            var possibleCombinations = new List<IEnumerable<Card>>();

            for (int i1 = 0; i1 < availableCards.Count(); i1++)
            {
                for (int i2 = i1 + 1; i2 < availableCards.Count(); i2++)
                {
                    for (int i3 = i2 + 1; i3 < availableCards.Count(); i3++)
                    {
                        var result = table.Cards.Where(x => x != null).ToList();
                        result.Add(availableCards[i1]);
                        result.Add(availableCards[i2]);
                        result.Add(availableCards[i3]);
                        possibleCombinations.Add(result);
                    }
                }
            }

            var combinationsDictionary = possibleCombinations
                .Select(x => new { Combination = x, Comb = _combinationFinder.GetBestCombination(x).Combination })
                .GroupBy(x => x.Comb)
                .Select(x => new { x.Key, Chance = x.Count() / (double)possibleCombinations.Count })
                .ToDictionary(keySelector: x => x.Key, elementSelector: chance => chance.Chance);

            return new CombinationEstimationResult(combinationsDictionary);
        }


        private IEnumerable<Card> GetAvailableCards(IEnumerable<Card> unavailableCards)
        {
            return Deck.NewDeck
                .NotShuffled()
                .Cards
                .Except(unavailableCards)
                .ToList();
        }



        private IEnumerable<Card> GetNotAvailableCards(ITable table, ICardsHolder player)
        {
            var cards = player.Cards.ToList();
            cards.AddRange(table.Cards);

            var cardsNotAvailable = cards
                .ExceptNull()
                .ToList();

            return cardsNotAvailable;
        }
    }

    public class CombinationEstimationResult
    {
        public CombinationEstimationResult(IDictionary<ECombination, double> chances)
        {
            CombinationChances = chances;
        }
        public IDictionary<ECombination, double> CombinationChances { get; }

        public override string ToString()
        {
            return string.Join("\n", CombinationChances.Select(x => $"{x.Key}: {ToPercent(x.Value)}"));

            string ToPercent(double value)
            {
                if (value < 0.01)
                    return "< 0%";
                var result = value.ToString("0.##%");
                return result;
            }
        }
    }

    class CardsEqualityComparer : IEqualityComparer<IEnumerable<Card>>
    {
        public bool Equals(IEnumerable<Card> first, IEnumerable<Card> second)
        {
            return first.SequenceEqual(second);
        }

        public int GetHashCode([DisallowNull] IEnumerable<Card> obj)
        {
            return obj.Sum(x => x.GetHashCode());
        }
    }
}
