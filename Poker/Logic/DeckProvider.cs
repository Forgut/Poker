using Poker.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Logic
{
    internal class DeckProvider
    {
        public IEnumerable<Card> GetInitialDeck()
        {
            foreach (EColor color in Enum.GetValues(typeof(EColor)))
                foreach (EValue value in Enum.GetValues(typeof(EValue)))
                    yield return new Card(value, color);
        }
    }
}
