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
    public class CardSnapshotTests
    {
        [Fact]
        public void ToSnapshot_should_create_snapshot_object_with_same_value()
        {
            var card = new Card(EValue.Jack, EColor.Spades);
            var snapshot = card.ToSnapshot();
            Assert.Equal(card.Value, snapshot.Value);
        }

        [Fact]
        public void ToSnapshot_should_create_snapshot_object_with_same_color()
        {
            var card = new Card(EValue.Jack, EColor.Spades);
            var snapshot = card.ToSnapshot();
            Assert.Equal(card.Color, snapshot.Color);
        }

        [Fact]
        public void FromSnapshot_should_recreate_card_from_snapshot_with_proper_value()
        {
            var snapshot = new CardSnapshot()
            {
                Color = EColor.Clubs,
                Value = EValue.Ten,
            };

            var card = Card.EmptyCard.FromSnapshot(snapshot);
            Assert.Equal(snapshot.Value, card.Value);
        }

        [Fact]
        public void FromSnapshot_should_recreate_card_from_snapshot_with_proper_color()
        {
            var snapshot = new CardSnapshot()
            {
                Color = EColor.Clubs,
                Value = EValue.Ten,
            };

            var card = Card.EmptyCard.FromSnapshot(snapshot);
            Assert.Equal(snapshot.Color, card.Color);
        }
    }
}
