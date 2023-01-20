using System;
using System.Collections.Generic;
using System.Text;

namespace Poker.Core.Domain.Events
{
    public abstract class Event
    {
        protected Event()
        {
            Occured = DateTimeOffset.UtcNow;
        }
        public DateTimeOffset Occured { get; }
        public abstract EEventType Type { get; }
    }
}
