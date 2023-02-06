using System;
using System.Collections.Generic;
using System.Text;

namespace Poker.Core.Domain.Entity.Snapshot
{
    public struct CardSnapshot
    {
        public EValue Value { get; internal set; }
        public EColor Color { get; internal set; }
    }
}
