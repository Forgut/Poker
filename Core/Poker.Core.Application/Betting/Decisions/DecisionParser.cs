using System;

namespace Poker.Core.Application.Betting.Decisions
{
    public static class DecisionParser
    {
        public static (Decision Decision, int? Amount) Parse(string input)
        {
            try
            {
                var split = input.Split(':');
                var decision = (Decision)Enum.Parse(typeof(Decision), split[0], ignoreCase: true);
                if (split.Length < 2 || decision != Decision.Raise)
                    return (decision, null);
                var amount = int.Parse(split[1]);
                return (decision, amount);
            }
            catch
            {
                return (Decision.Unkown, null);
            }
        }
    }
}
