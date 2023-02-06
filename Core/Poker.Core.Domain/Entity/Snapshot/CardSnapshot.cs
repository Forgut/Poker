using System;
using System.Collections.Generic;
using System.Text;

namespace Poker.Core.Domain.Entity.Snapshot
{
    public struct CardSnapshot
    {
        public EValue Value { get; init; }
        public EColor Color { get; init; }
    }
}
