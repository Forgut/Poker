using NSubstitute.ReceivedExtensions;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Entity.Snapshot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Poker.Tests.UnitTests.Snapshot
{
    public class PlayerSnapshotTests
    {
        [Fact]
        public void ToSnapshot_should_create_snapshot_object_with_same_name()
        {
            var player = GetPlayer();
            var snapshot = player.ToSnapshot();
            Assert.Equal(player.Name, snapshot.Name);
        }

        [Fact]
        public void ToSnapshot_should_create_snapshot_object_with_same_money()
        {
            var player = GetPlayer();
            var snapshot = player.ToSnapshot();
            Assert.Equal(player.Money, snapshot.Money);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void ToSnapshot_should_create_snapshot_object_with_same_cards_count(int cardsCount)
        {
            var player = GetPlayer();
            if (cardsCount > 0)
                player.SetFirstCard(new Card(EValue.Jack, EColor.Spades));
            if (cardsCount > 1)
                player.SetSecondCard(new Card(EValue.Ace, EColor.Hearts));
            var snapshot = player.ToSnapshot();
            Assert.Equal(player.Cards.Count, snapshot.Cards.Count());
        }

        [Fact]
        public void FromSnapshot_should_create_player_with_same_name()
        {
            var snapshot = GetPlayerSnapshot(0);

            var player = Player.EmptyPlayer.FromSnapshot(snapshot);
            Assert.Equal(snapshot.Name, player.Name);
        }

        [Fact]
        public void FromSnapshot_should_create_player_with_same_money()
        {
            var snapshot = GetPlayerSnapshot(0);

            var player = Player.EmptyPlayer.FromSnapshot(snapshot);
            Assert.Equal(snapshot.Money, player.Money);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void FromSnapshot_should_create_player_with_same_Cards(int cardsCount)
        {
            var snapshot = GetPlayerSnapshot(cardsCount);

            var player = Player.EmptyPlayer.FromSnapshot(snapshot);
            Assert.Equal(snapshot.Cards.Count(), player.Cards.Count);
        }

        private Player GetPlayer()
        {
            return new Player("Test", 100);
        }

        private PlayerSnapshot GetPlayerSnapshot(int cardsCount)
        {
            var cards = new List<CardSnapshot?>();
            if (cardsCount > 0)
                cards.Add(new CardSnapshot() { Color = EColor.Diamonds, Value = EValue.Queen });
            if (cardsCount > 1)
                cards.Add(new CardSnapshot() { Color = EColor.Diamonds, Value = EValue.Ace });

            return new PlayerSnapshot()
            {
                Cards = cards,
                Money = 100,
                Name = "test"
            };
        }
    }
}
