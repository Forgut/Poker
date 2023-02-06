using System;
using System.Collections.Generic;
using System.Text;

namespace Poker.Core.Domain.Entity.Snapshot
{
    public struct PlayerSnapshot
    {
        public string Name { get; init; }
        public int Money { get; init; }
        public IEnumerable<CardSnapshot?> Cards { get; init; }
    }
}
