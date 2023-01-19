using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Xunit;

namespace Poker.Tests.UnitTests
{
    public class CombinationComparerTests
    {
        [Theory]
        [InlineData(ECombination.HighCard, ECombination.HighCard, ECombination.HighCard, ECombination.OnePair, ECombination.OnePair)]
        [InlineData(ECombination.RoyalFlush, ECombination.TwoPair, ECombination.FourOfAKind, ECombination.OnePair, ECombination.RoyalFlush)]
        [InlineData(ECombination.OnePair, ECombination.TwoPair, ECombination.HighCard, ECombination.OnePair, ECombination.TwoPair)]
        [InlineData(ECombination.FourOfAKind, ECombination.HighCard, ECombination.ThreeOfAKind, ECombination.ThreeOfAKind, ECombination.FourOfAKind)]
        public void Should_return_best_combination_if_there_is_no_equal_level_combinations(ECombination combination1,
            ECombination combination2,
            ECombination combination3,
            ECombination combination4,
            ECombination expectedWinner)
        {
            var winner = new CombinationComparer()
                .GetBestCombinations(new List<CombinationDTO>()
                {
                    new CombinationDTO(combination1, null),
                    new CombinationDTO(combination2, null),
                    new CombinationDTO(combination3, null),
                    new CombinationDTO(combination4, null),
                });

            Assert.Equal(expectedWinner, winner.Single().Combination);
        }

        [Fact]
        public void Should_return_best_combination_based_on_highest_card_for_equal_level_combinations()
        {
            var combination1 = new CombinationDTO(ECombination.Flush,
                new List<Card>()
                {
                    new Card(EValue.King, EColor.Diamonds),
                    new Card(EValue.Ten, EColor.Diamonds),
                    new Card(EValue.Seven, EColor.Diamonds),
                    new Card(EValue.Four, EColor.Diamonds),
                    new Card(EValue.Two, EColor.Diamonds),
                });

            var combination2 = new CombinationDTO(ECombination.Flush,
                new List<Card>()
                {
                    new Card(EValue.Queen, EColor.Diamonds),
                    new Card(EValue.Jack, EColor.Diamonds),
                    new Card(EValue.Ten, EColor.Diamonds),
                    new Card(EValue.Four, EColor.Diamonds),
                    new Card(EValue.Two, EColor.Diamonds),
                });

            var combination3 = new CombinationDTO(ECombination.HighCard,
                new List<Card>()
                {
                    new Card(EValue.Queen, EColor.Hearts),
                    new Card(EValue.Jack, EColor.Diamonds),
                    new Card(EValue.Ten, EColor.Clubs),
                    new Card(EValue.Four, EColor.Diamonds),
                    new Card(EValue.Two, EColor.Diamonds),
                });

            var winner = new CombinationComparer()
                .GetBestCombinations(new List<CombinationDTO>()
                {
                    combination1,
                    combination2,
                    combination3
                });

            Assert.Equal(combination1, winner.Single());
        }

        [Fact]
        public void Should_return_combination_based_on_not_first_highest_card_for_equal_level_combinations()
        {
            var combination1 = new CombinationDTO(ECombination.Flush,
                new List<Card>()
                {
                    new Card(EValue.King, EColor.Diamonds),
                    new Card(EValue.Ten, EColor.Diamonds),
                    new Card(EValue.Seven, EColor.Diamonds),
                    new Card(EValue.Four, EColor.Diamonds),
                    new Card(EValue.Two, EColor.Diamonds),
                });

            var combination2 = new CombinationDTO(ECombination.Flush,
                new List<Card>()
                {
                    new Card(EValue.King, EColor.Diamonds),
                    new Card(EValue.Jack, EColor.Diamonds),
                    new Card(EValue.Ten, EColor.Diamonds),
                    new Card(EValue.Four, EColor.Diamonds),
                    new Card(EValue.Two, EColor.Diamonds),
                });

            var combination3 = new CombinationDTO(ECombination.HighCard,
                new List<Card>()
                {
                    new Card(EValue.Queen, EColor.Hearts),
                    new Card(EValue.Jack, EColor.Diamonds),
                    new Card(EValue.Ten, EColor.Clubs),
                    new Card(EValue.Four, EColor.Diamonds),
                    new Card(EValue.Two, EColor.Diamonds),
                });

            var winner = new CombinationComparer()
                .GetBestCombinations(new List<CombinationDTO>()
                {
                    combination1,
                    combination2,
                    combination3
                });

            Assert.Equal(combination2, winner.Single());
        }

        [Fact]
        public void Should_return_combination_based_on_not_first_highest_card_for_equal_level_combinations_2()
        {
            var combination1 = new CombinationDTO(ECombination.OnePair,
                new List<Card>()
                {
                    new Card(EValue.King, EColor.Diamonds),
                    new Card(EValue.King, EColor.Hearts),
                    new Card(EValue.Seven, EColor.Diamonds),
                    new Card(EValue.Four, EColor.Diamonds),
                    new Card(EValue.Two, EColor.Diamonds),
                });

            var combination2 = new CombinationDTO(ECombination.OnePair,
                new List<Card>()
                {
                    new Card(EValue.King, EColor.Clubs),
                    new Card(EValue.King, EColor.Spades),
                    new Card(EValue.Seven, EColor.Diamonds),
                    new Card(EValue.Five, EColor.Diamonds),
                    new Card(EValue.Two, EColor.Diamonds),
                });

            var combination3 = new CombinationDTO(ECombination.HighCard,
                new List<Card>()
                {
                    new Card(EValue.Queen, EColor.Hearts),
                    new Card(EValue.Jack, EColor.Diamonds),
                    new Card(EValue.Ten, EColor.Clubs),
                    new Card(EValue.Four, EColor.Diamonds),
                    new Card(EValue.Two, EColor.Diamonds),
                });
            var winner = new CombinationComparer()
                .GetBestCombinations(new List<CombinationDTO>()
                {
                    combination1,
                    combination2,
                    combination3
                });

            Assert.Equal(combination2, winner.Single());
        }

        [Fact]
        public void Should_return_draw_combination_for_equal_level_combinations()
        {
            var combination1 = new CombinationDTO(ECombination.OnePair,
                new List<Card>()
                {
                    new Card(EValue.King, EColor.Diamonds),
                    new Card(EValue.King, EColor.Hearts),
                    new Card(EValue.Seven, EColor.Diamonds),
                    new Card(EValue.Four, EColor.Diamonds),
                    new Card(EValue.Two, EColor.Diamonds),
                });

            var combination2 = new CombinationDTO(ECombination.OnePair,
                new List<Card>()
                {
                    new Card(EValue.King, EColor.Clubs),
                    new Card(EValue.King, EColor.Spades),
                    new Card(EValue.Seven, EColor.Diamonds),
                    new Card(EValue.Four, EColor.Diamonds),
                    new Card(EValue.Two, EColor.Diamonds),
                });

            var combination3 = new CombinationDTO(ECombination.HighCard,
                new List<Card>()
                {
                    new Card(EValue.Queen, EColor.Hearts),
                    new Card(EValue.Jack, EColor.Diamonds),
                    new Card(EValue.Ten, EColor.Clubs),
                    new Card(EValue.Four, EColor.Diamonds),
                    new Card(EValue.Two, EColor.Diamonds),
                });

            var winner = new CombinationComparer()
                .GetBestCombinations(new List<CombinationDTO>()
                {
                    combination1,
                    combination2,
                    combination3
                });

            Assert.Contains(combination1, winner);
            Assert.Contains(combination2, winner);
        }


        //♣ ♠ ♦ ♥
        [Theory]
        [InlineData("2♣ 3♣ 4♣ 5♣ 6♦ 7♣ A♥", "2♣ 3♣ 4♦ 5♣ 6♦ 8♦ A♥", "2♣ 3♣ 4♣ 5♣ 6♦ 5♠ A♥", ExpectedWinner.Hand1, ECombination.Flush)]
        [InlineData("9♠ 4♣ 7♣ 4♥ 2♣ A♥ K♥", "9♠ 4♣ 7♣ 4♥ 2♣ A♦ K♦", "9♠ 4♣ 7♣ 4♥ 2♣ A♠ K♠", ExpectedWinner.Hand1 | ExpectedWinner.Hand2 | ExpectedWinner.Hand3, ECombination.OnePair)]
        [InlineData("10♦ 2♠ 7♣ 5♣ A♥ 2♦ 8♦", "10♦ 2♠ 7♣ 5♣ A♥ 10♣ Q♥", "10♦ 2♠ 7♣ 5♣ A♥ 7♦ 3♠", ExpectedWinner.Hand2, ECombination.OnePair)]
        [InlineData("2♠ 10♣ K♦ 6♥ 9♠ 6♣ 6♦", "2♠ 10♣ K♦ 6♥ 9♠ 3♦ J♣", "2♠ 10♣ K♦ 6♥ 9♠ K♠ 5♠", ExpectedWinner.Hand1, ECombination.ThreeOfAKind)]
        [InlineData("A♦ K♦ Q♦ J♦ 6♦ 2♣ 7♦", "A♦ K♦ Q♦ J♦ 6♦ 10♥ A♣", "A♦ K♦ Q♦ J♦ 6♦ 3♣ 10♦", ExpectedWinner.Hand3, ECombination.RoyalFlush)]
        [InlineData("A♦ K♦ Q♦ J♦ 10♦ A♠ Q♠", "A♦ K♦ Q♦ J♦ 10♦ Q♣ A♣", "A♦ K♦ Q♦ J♦ 10♦ Q♥ A♥", ExpectedWinner.Hand1 | ExpectedWinner.Hand2 | ExpectedWinner.Hand3, ECombination.RoyalFlush)]
        public void Should_return_best_combination(string hand1,
            string hand2,
            string hand3,
            ExpectedWinner expectedWinner,
            ECombination expectedCombination)
        {
            var combination1 = new CombinationFinder(GetCards(hand1)).GetBestCombination();
            var combination2 = new CombinationFinder(GetCards(hand2)).GetBestCombination();
            var combination3 = new CombinationFinder(GetCards(hand3)).GetBestCombination();

            var winner = new CombinationComparer()
                .GetBestCombinations(new List<CombinationDTO>()
                {
                    combination1,
                    combination2,
                    combination3,
                });

            Assert.Equal(expectedCombination, winner.First().Combination);
            if (expectedWinner.HasFlag(ExpectedWinner.Hand1))
                Assert.Contains(combination1, winner);
            if (expectedWinner.HasFlag(ExpectedWinner.Hand2))
                Assert.Contains(combination2, winner);
            if (expectedWinner.HasFlag(ExpectedWinner.Hand3))
                Assert.Contains(combination3, winner);
        }

        [Theory]
        [InlineData("3♣ 3♥ A♣ A♦ A♠ 7♣ 8♥", "2♣ 2♥ A♣ A♦ A♠ 7♣ 8♥", ExpectedWinner.Hand1, ECombination.FullHouse)]
        [InlineData("A♣ A♥ 3♣ 3♦ 3♠ 7♣ 8♥", "Q♣ Q♥ 3♣ 3♦ 3♠ 7♣ 8♥", ExpectedWinner.Hand1, ECombination.FullHouse)]
        public void Should_recognize_better_full_house_properly(string hand1,
            string hand2,
            ExpectedWinner expectedWinner,
            ECombination expectedCombination)
        {
            var combination1 = new CombinationFinder(GetCards(hand1)).GetBestCombination();
            var combination2 = new CombinationFinder(GetCards(hand2)).GetBestCombination();

            var winner = new CombinationComparer()
                .GetBestCombinations(new List<CombinationDTO>()
                {
                    combination1,
                    combination2,
                });

            Assert.Equal(expectedCombination, winner.First().Combination);
            if (expectedWinner.HasFlag(ExpectedWinner.Hand1))
                Assert.Contains(combination1, winner);
            if (expectedWinner.HasFlag(ExpectedWinner.Hand2))
                Assert.Contains(combination2, winner);
        }

        [Flags]
        public enum ExpectedWinner
        {
            Hand1 = 1,
            Hand2 = 2,
            Hand3 = 4,
        }

        private IEnumerable<Card> GetCards(string input)
        {
            return input.Split(" ")
                .Select(x => GetCard(x));
        }

        private Card GetCard(string input)
        {
            var rx = new Regex(@"[0-9AKQJ]+");
            var value = GetValue(rx.Match(input).Value);
            var color = GetColor(input.Last());
            return new Card(value, color);
        }

        EValue GetValue(string input)
        {
            switch (input)
            {
                case "A":
                    return EValue.Ace;
                case "K":
                    return EValue.King;
                case "Q":
                    return EValue.Queen;
                case "J":
                    return EValue.Jack;
            }
            var number = int.Parse(input);
            return (EValue)number;
        }

        EColor GetColor(char input)
        {
            switch (input)
            {
                case '♣':
                    return EColor.Clubs;
                case '♠':
                    return EColor.Spades;
                case '♦':
                    return EColor.Diamonds;
                case '♥':
                    return EColor.Hearts;
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
