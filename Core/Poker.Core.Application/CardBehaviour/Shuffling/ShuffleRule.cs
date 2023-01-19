using System;
using System.Collections.Generic;
using Poker.Core.Domain.Entity;

namespace Poker.Core.Application.CardBehaviour.Shuffling
{
    public class ShuffleRule : IShuffleRule
    {
        private readonly Random _random;

        public ShuffleRule(Random random)
        {
            _random = random;
        }

        public void ShuffleDeck(List<Card> cards)
        {
            Shuffle(cards);
        }
        /// <summary>
        /// https://stackoverflow.com/questions/108819/best-way-to-randomize-an-array-with-net
        /// </summary>
        private void Shuffle<T>(List<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                int k = _random.Next(n--);
                T temp = list[n];
                list[n] = list[k];
                list[k] = temp;
            }
        }
    }
}
