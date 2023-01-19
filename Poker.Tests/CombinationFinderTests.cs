using Poker.Entity;
using Poker.Logic;
using Poker.Logic.Cards.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Poker.Tests
{
    public class CombinationFinderTests
    {
        private readonly Player _player;
        private readonly Table _table;

        public CombinationFinderTests()
        {
            _player = new Player("test", 100);
            _table = new Table(new List<Player>() { _player });
        }

        [Fact]
        public void Should_find_high_card()
        {
            _player.Cards[0] = new Card(EValue.Eight, EColor.Diamonds);
            _player.Cards[1] = new Card(EValue.Ten, EColor.Diamonds);
            _table.Cards[0] = new Card(EValue.Two, EColor.Clubs);
            _table.Cards[1] = new Card(EValue.Four, EColor.Clubs);
            _table.Cards[2] = new Card(EValue.Six, EColor.Hearts);
            _table.Cards[3] = new Card(EValue.King, EColor.Clubs);
            _table.Cards[4] = new Card(EValue.Ace, EColor.Hearts);

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.HighCard, combination.Combination);
            Assert.Single(combination.Cards.Where(x=>x.Value == EValue.Ace));
            Assert.Single(combination.Cards.Where(x=>x.Value == EValue.King));
            Assert.Single(combination.Cards.Where(x=>x.Value == EValue.Ten));
            Assert.Single(combination.Cards.Where(x=>x.Value == EValue.Eight));
            Assert.Single(combination.Cards.Where(x=>x.Value == EValue.Six));
        }

        [Fact]
        public void Should_find_pair()
        {
            _player.Cards[0] = new Card(EValue.Eight, EColor.Hearts);
            _player.Cards[1] = new Card(EValue.Ten, EColor.Clubs);
            _table.Cards[0] = new Card(EValue.Two, EColor.Hearts);
            _table.Cards[1] = new Card(EValue.Four, EColor.Clubs);
            _table.Cards[2] = new Card(EValue.Ten, EColor.Diamonds);
            _table.Cards[3] = new Card(EValue.King, EColor.Clubs);
            _table.Cards[4] = new Card(EValue.Ace, EColor.Hearts);

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
            _player.Cards[0] = new Card(EValue.Eight, EColor.Hearts);
            _player.Cards[1] = new Card(EValue.Ten, EColor.Clubs);
            _table.Cards[0] = new Card(EValue.Eight, EColor.Diamonds);
            _table.Cards[1] = new Card(EValue.Ten, EColor.Diamonds);
            _table.Cards[2] = new Card(EValue.Two, EColor.Clubs);
            _table.Cards[3] = new Card(EValue.King, EColor.Clubs);
            _table.Cards[4] = new Card(EValue.Four, EColor.Clubs);

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.TwoPair, combination.Combination);
            Assert.True(combination.Cards.Count(x => x.Value == EValue.Ten) == 2);
            Assert.True(combination.Cards.Count(x => x.Value == EValue.Eight) == 2);
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.King));
        }

        [Fact]
        public void Should_find_two_pairs_when_there_are_3_pairs()
        {
            _player.Cards[0] = new Card(EValue.Eight, EColor.Hearts);
            _player.Cards[1] = new Card(EValue.Ten, EColor.Clubs);
            _table.Cards[0] = new Card(EValue.Eight, EColor.Diamonds);
            _table.Cards[1] = new Card(EValue.Ten, EColor.Diamonds);
            _table.Cards[2] = new Card(EValue.Two, EColor.Clubs);
            _table.Cards[3] = new Card(EValue.Four, EColor.Hearts);
            _table.Cards[4] = new Card(EValue.Four, EColor.Clubs);

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.TwoPair, combination.Combination);
            Assert.True(combination.Cards.Count(x => x.Value == EValue.Ten) == 2);
            Assert.True(combination.Cards.Count(x => x.Value == EValue.Eight) == 2);
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Four));
        }

        [Fact]
        public void Should_find_three_of_a_kind()
        {
            _player.Cards[0] = new Card(EValue.Eight, EColor.Clubs);
            _player.Cards[1] = new Card(EValue.Eight, EColor.Spades);
            _table.Cards[0] = new Card(EValue.Eight, EColor.Diamonds);
            _table.Cards[1] = new Card(EValue.Ten, EColor.Diamonds);
            _table.Cards[2] = new Card(EValue.Four, EColor.Clubs);
            _table.Cards[3] = new Card(EValue.Four, EColor.Clubs);
            _table.Cards[4] = new Card(EValue.Four, EColor.Clubs);

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.ThreeOfAKind, combination.Combination);
            Assert.True(combination.Cards.Count(x => x.Value == EValue.Eight) == 3);
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Ten));
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Four));
        }

        [Fact]
        public void Should_find_flush()
        {
            _player.Cards[0] = new Card(EValue.Eight, EColor.Clubs);
            _player.Cards[1] = new Card(EValue.Three, EColor.Clubs);
            _table.Cards[0] = new Card(EValue.Eight, EColor.Diamonds);
            _table.Cards[1] = new Card(EValue.Ten, EColor.Diamonds);
            _table.Cards[2] = new Card(EValue.Two, EColor.Clubs);
            _table.Cards[3] = new Card(EValue.King, EColor.Clubs);
            _table.Cards[4] = new Card(EValue.Four, EColor.Clubs);

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.Flush, combination.Combination);
            Assert.True(combination.Cards.All(x => x.Color == EColor.Clubs));
        }

        [Fact]
        public void Should_find_straight()
        {
            _player.Cards[0] = new Card(EValue.Five, EColor.Clubs);
            _player.Cards[1] = new Card(EValue.Three, EColor.Clubs);
            _table.Cards[0] = new Card(EValue.Four, EColor.Diamonds);
            _table.Cards[1] = new Card(EValue.Six, EColor.Diamonds);
            _table.Cards[2] = new Card(EValue.Six, EColor.Clubs);
            _table.Cards[3] = new Card(EValue.Seven, EColor.Clubs);
            _table.Cards[4] = new Card(EValue.King, EColor.Hearts);

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
            _player.Cards[0] = new Card(EValue.Five, EColor.Clubs);
            _player.Cards[1] = new Card(EValue.Five, EColor.Diamonds);
            _table.Cards[0] = new Card(EValue.Four, EColor.Diamonds);
            _table.Cards[1] = new Card(EValue.Six, EColor.Diamonds);
            _table.Cards[2] = new Card(EValue.Seven, EColor.Hearts);
            _table.Cards[3] = new Card(EValue.Five, EColor.Hearts);
            _table.Cards[4] = new Card(EValue.Seven, EColor.Clubs);

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.FullHouse, combination.Combination);
            Assert.True(combination.Cards.Count(x => x.Value == EValue.Five) == 3);
            Assert.True(combination.Cards.Count(x => x.Value == EValue.Seven) == 2);
        }

        [Fact]
        public void Should_find_four_of_a_kind()
        {
            _player.Cards[0] = new Card(EValue.Five, EColor.Clubs);
            _player.Cards[1] = new Card(EValue.Five, EColor.Hearts);
            _table.Cards[0] = new Card(EValue.Five, EColor.Diamonds);
            _table.Cards[1] = new Card(EValue.Five, EColor.Spades);
            _table.Cards[2] = new Card(EValue.Seven, EColor.Hearts);
            _table.Cards[3] = new Card(EValue.Eight, EColor.Hearts);
            _table.Cards[4] = new Card(EValue.Seven, EColor.Clubs);

            var combination = new CombinationFinder(_player, _table).GetBestCombination();
            Assert.Equal(ECombination.FourOfAKind, combination.Combination);
            Assert.True(combination.Cards.Count(x => x.Value == EValue.Five) == 4);
            Assert.Single(combination.Cards.Where(x => x.Value == EValue.Eight));

        }

        [Fact]
        public void Should_find_straight_flush()
        {
            _player.Cards[0] = new Card(EValue.Five, EColor.Clubs);
            _player.Cards[1] = new Card(EValue.Three, EColor.Clubs);
            _table.Cards[0] = new Card(EValue.Four, EColor.Clubs);
            _table.Cards[1] = new Card(EValue.Six, EColor.Clubs);
            _table.Cards[2] = new Card(EValue.Seven, EColor.Clubs);
            _table.Cards[3] = new Card(EValue.Seven, EColor.Hearts);
            _table.Cards[4] = new Card(EValue.Ace, EColor.Clubs);

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
            _player.Cards[0] = new Card(EValue.Ace, EColor.Hearts);
            _player.Cards[1] = new Card(EValue.Queen, EColor.Hearts);
            _table.Cards[0] = new Card(EValue.Jack, EColor.Hearts);
            _table.Cards[1] = new Card(EValue.King, EColor.Hearts);
            _table.Cards[2] = new Card(EValue.Ten, EColor.Clubs);
            _table.Cards[3] = new Card(EValue.Ten, EColor.Hearts);
            _table.Cards[4] = new Card(EValue.Three, EColor.Clubs);

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
