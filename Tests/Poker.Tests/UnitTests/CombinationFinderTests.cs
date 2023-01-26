using NSubstitute;
using Poker.Core.Application;
using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Domain.Entity;
using Poker.Tests.Common;
using System.Linq;
using Xunit;

namespace Poker.Tests.UnitTests
{
    public class CombinationFinderTests
    {
        private readonly Player _player;
        private readonly ITable _table;

        public CombinationFinderTests()
        {
            _player = new Player("test", 100);
            _table = Substitute.For<ITable>();
        }

        [Fact]
        public void Should_find_high_card()
        {
            _player.SetFirstCard(new Card(EValue.Eight, EColor.Diamonds));
            _player.SetSecondCard(new Card(EValue.Ten, EColor.Diamonds));

            _table.Cards.Returns(ReadOnlyCreator.GetReadonlyCollection(
                new Card(EValue.Two, EColor.Clubs),
                new Card(EValue.Four, EColor.Clubs),
                new Card(EValue.Six, EColor.Hearts),
                new Card(EValue.King, EColor.Clubs),
                new Card(EValue.Ace, EColor.Hearts)));

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.HighCard, combination.Combination);
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Ace));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.King));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Ten));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Eight));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Six));
        }

        [Fact]
        public void Should_find_pair()
        {
            _player.SetFirstCard(new Card(EValue.Eight, EColor.Hearts));
            _player.SetSecondCard(new Card(EValue.Ten, EColor.Clubs));
            _table.Cards.Returns(ReadOnlyCreator.GetReadonlyCollection(
                new Card(EValue.Two, EColor.Hearts),
                new Card(EValue.Four, EColor.Clubs),
                new Card(EValue.Ten, EColor.Diamonds),
                new Card(EValue.King, EColor.Clubs),
                new Card(EValue.Ace, EColor.Hearts)));

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.OnePair, combination.Combination);
            Assert.True(combination.Cards.Count(x => x.Value == EValue.Ten) == 2);
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Ace));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.King));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Eight));
        }

        [Fact]
        public void Should_find_two_pairs()
        {
            _player.SetFirstCard(new Card(EValue.Eight, EColor.Hearts));
            _player.SetSecondCard(new Card(EValue.Ten, EColor.Clubs));

            _table.Cards.Returns(ReadOnlyCreator.GetReadonlyCollection(
                new Card(EValue.Eight, EColor.Diamonds),
                new Card(EValue.Ten, EColor.Diamonds),
                new Card(EValue.Two, EColor.Clubs),
                new Card(EValue.King, EColor.Clubs),
                new Card(EValue.Four, EColor.Clubs)));

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.TwoPair, combination.Combination);
            Assert.True(combination.Cards.Count(x => x.Value == EValue.Ten) == 2);
            Assert.True(combination.Cards.Count(x => x.Value == EValue.Eight) == 2);
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.King));
        }

        [Fact]
        public void Should_find_two_pairs_when_there_are_3_pairs()
        {
            _player.SetFirstCard(new Card(EValue.Eight, EColor.Hearts));
            _player.SetSecondCard(new Card(EValue.Ten, EColor.Clubs));

            _table.Cards.Returns(ReadOnlyCreator.GetReadonlyCollection(
                new Card(EValue.Eight, EColor.Diamonds),
                new Card(EValue.Ten, EColor.Diamonds),
                new Card(EValue.Two, EColor.Clubs),
                new Card(EValue.Four, EColor.Hearts),
                new Card(EValue.Four, EColor.Clubs)));

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.TwoPair, combination.Combination);
            Assert.True(combination.Cards.Count(x => x.Value == EValue.Ten) == 2);
            Assert.True(combination.Cards.Count(x => x.Value == EValue.Eight) == 2);
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Four));
        }

        [Fact]
        public void Should_find_three_of_a_kind()
        {
            _player.SetFirstCard(new Card(EValue.Eight, EColor.Clubs));
            _player.SetSecondCard(new Card(EValue.Eight, EColor.Spades));

            _table.Cards.Returns(ReadOnlyCreator.GetReadonlyCollection(
                new Card(EValue.Eight, EColor.Diamonds),
                new Card(EValue.Ten, EColor.Diamonds),
                new Card(EValue.Four, EColor.Clubs),
                new Card(EValue.Four, EColor.Clubs),
                new Card(EValue.Four, EColor.Clubs)));

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.ThreeOfAKind, combination.Combination);
            Assert.True(combination.Cards.Count(x => x.Value == EValue.Eight) == 3);
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Ten));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Four));
        }

        [Fact]
        public void Should_find_flush()
        {
            _player.SetFirstCard(new Card(EValue.Eight, EColor.Clubs));
            _player.SetSecondCard(new Card(EValue.Three, EColor.Clubs));

            _table.Cards.Returns(ReadOnlyCreator.GetReadonlyCollection(
                new Card(EValue.Eight, EColor.Diamonds),
                new Card(EValue.Ten, EColor.Diamonds),
                new Card(EValue.Two, EColor.Clubs),
                new Card(EValue.King, EColor.Clubs),
                new Card(EValue.Four, EColor.Clubs)));

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.Flush, combination.Combination);
            Assert.True(combination.Cards.All(x => x.Color == EColor.Clubs));
        }

        [Fact]
        public void Should_find_straight()
        {
            _player.SetFirstCard(new Card(EValue.Five, EColor.Clubs));
            _player.SetSecondCard(new Card(EValue.Three, EColor.Clubs));

            _table.Cards.Returns(ReadOnlyCreator.GetReadonlyCollection(
                new Card(EValue.Four, EColor.Diamonds),
                new Card(EValue.Six, EColor.Diamonds),
                new Card(EValue.Six, EColor.Clubs),
                new Card(EValue.Seven, EColor.Clubs),
                new Card(EValue.King, EColor.Hearts)));

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.Straight, combination.Combination);
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Three));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Four));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Five));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Six));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Seven));
        }

        [Fact]
        public void Should_find_full_house()
        {
            _player.SetFirstCard(new Card(EValue.Five, EColor.Clubs));
            _player.SetSecondCard(new Card(EValue.Five, EColor.Diamonds));
            _table.Cards.Returns(ReadOnlyCreator.GetReadonlyCollection(
                new Card(EValue.Four, EColor.Diamonds),
                new Card(EValue.Six, EColor.Diamonds),
                new Card(EValue.Seven, EColor.Hearts),
                new Card(EValue.Five, EColor.Hearts),
                new Card(EValue.Seven, EColor.Clubs)));

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.FullHouse, combination.Combination);
            Assert.True(combination.Cards.Count(x => x.Value == EValue.Five) == 3);
            Assert.True(combination.Cards.Count(x => x.Value == EValue.Seven) == 2);
        }

        [Fact]
        public void Should_find_four_of_a_kind()
        {
            _player.SetFirstCard(new Card(EValue.Five, EColor.Clubs));
            _player.SetSecondCard(new Card(EValue.Five, EColor.Hearts));
            _table.Cards.Returns(ReadOnlyCreator.GetReadonlyCollection(
                new Card(EValue.Five, EColor.Diamonds),
                new Card(EValue.Five, EColor.Spades),
                new Card(EValue.Seven, EColor.Hearts),
                new Card(EValue.Eight, EColor.Hearts),
                new Card(EValue.Seven, EColor.Clubs)));

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.FourOfAKind, combination.Combination);
            Assert.True(combination.Cards.Count(x => x.Value == EValue.Five) == 4);
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Eight));

        }

        [Fact]
        public void Should_find_straight_flush()
        {
            _player.SetFirstCard(new Card(EValue.Five, EColor.Clubs));
            _player.SetSecondCard(new Card(EValue.Three, EColor.Clubs));

            _table.Cards.Returns(ReadOnlyCreator.GetReadonlyCollection(
                new Card(EValue.Four, EColor.Clubs),
                new Card(EValue.Six, EColor.Clubs),
                new Card(EValue.Seven, EColor.Clubs),
                new Card(EValue.Seven, EColor.Hearts),
                new Card(EValue.Ace, EColor.Clubs)));

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.StraightFlush, combination.Combination);
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Three));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Four));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Five));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Six));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Seven));
        }


        [Fact]
        public void Should_find_royal_flush()
        {
            _player.SetFirstCard(new Card(EValue.Ace, EColor.Hearts));
            _player.SetSecondCard(new Card(EValue.Queen, EColor.Hearts));

            _table.Cards.Returns(ReadOnlyCreator.GetReadonlyCollection(
                new Card(EValue.Jack, EColor.Hearts),
                new Card(EValue.King, EColor.Hearts),
                new Card(EValue.Ten, EColor.Clubs),
                new Card(EValue.Ten, EColor.Hearts),
                new Card(EValue.Three, EColor.Clubs)));

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.RoyalFlush, combination.Combination);
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Ace));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.King));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Queen));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Jack));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Ten));
        }
    }
}
