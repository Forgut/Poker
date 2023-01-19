using Poker.Entity;
using Poker.Logic.Estimator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Poker.Tests
{
    public class DupaTest
    {
        [Fact]
        public void Should_have_chance_2_missing()
        {
            var table = new Table(null);
            table.Cards[0] = new Card(EValue.Ace, EColor.Diamonds);
            table.Cards[1] = new Card(EValue.King, EColor.Hearts);
            table.Cards[2] = new Card(EValue.Queen, EColor.Clubs);
            var player = new Player("Xd", 100);
            player.Cards[0] = new Card(EValue.Ace, EColor.Spades);
            player.Cards[1] = new Card(EValue.Queen, EColor.Spades);
            var result = new WinEstimator()
                .ProbableCombinationsForPlayer2Missing(table, player);
        }

        [Fact]
        public void Should_have_chance_1_missing()
        {
            var table = new Table(null);
            table.Cards[0] = new Card(EValue.Ace, EColor.Diamonds);
            table.Cards[1] = new Card(EValue.King, EColor.Hearts);
            table.Cards[2] = new Card(EValue.Queen, EColor.Clubs);
            table.Cards[3] = new Card(EValue.Jack, EColor.Clubs);
            var player = new Player("Xd", 100);
            player.Cards[0] = new Card(EValue.Ace, EColor.Spades);
            player.Cards[1] = new Card(EValue.Queen, EColor.Spades);
            var result = new WinEstimator()
                .ProbableCombinationsForPlayer1Missing(table, player);
        }

        [Fact]
        public void Should_have_chance_2_missing_enemy()
        {
            var table = new Table(null);
            table.Cards[0] = new Card(EValue.Ace, EColor.Diamonds);
            table.Cards[1] = new Card(EValue.King, EColor.Hearts);
            table.Cards[2] = new Card(EValue.Queen, EColor.Clubs);
            var player = new Player("Xd", 100);
            player.Cards[0] = new Card(EValue.Ace, EColor.Spades);
            player.Cards[1] = new Card(EValue.Queen, EColor.Spades);
            var result = new WinEstimator()
                .ProbableCombinationsForEnemy2Missing(table, player);
        }

        [Fact]
        public void Should_have_chance_1_missing_enemy()
        {
            var table = new Table(null);
            table.Cards[0] = new Card(EValue.Ace, EColor.Diamonds);
            table.Cards[1] = new Card(EValue.King, EColor.Hearts);
            table.Cards[2] = new Card(EValue.Queen, EColor.Clubs);
            table.Cards[3] = new Card(EValue.Queen, EColor.Diamonds);
            var player = new Player("Xd", 100);
            player.Cards[0] = new Card(EValue.Ace, EColor.Spades);
            player.Cards[1] = new Card(EValue.Queen, EColor.Spades);
            var result = new WinEstimator()
                .ProbableCombinationsForEnemy1Missing(table, player);
        }
    }
}
