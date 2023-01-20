using System.Collections.Generic;

namespace Poker.Core.Domain.Events
{
    public class RoundEndedEvent : Event
    {
        public RoundEndedEvent(IEnumerable<WinnerInfo> winners)
        {
            Winners = winners;
        }

        public override EEventType Type => EEventType.RoundEnded;
        public IEnumerable<WinnerInfo> Winners { get; }
    }

    public class WinnerInfo
    {
        public WinnerInfo(string name, string combination)
        {
            Name = name;
            Combination = combination;
        }

        public string Name { get; }
        public string Combination { get; }
    }
}
