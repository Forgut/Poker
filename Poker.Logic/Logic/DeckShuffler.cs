﻿using Poker.Entity;
using Poker.Logic.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker.Logic
{
    public class DeckShuffler : IShuffleRule
    {
        private readonly Random _random;

        public DeckShuffler(Random random)
        {
            _random = random;
        }

        public void ShuffleDeck(IEnumerable<Card> cards)
        {
             ShuffleIterator(cards, _random);
        }

        /// <summary>
        /// https://stackoverflow.com/questions/5807128/an-extension-method-on-ienumerable-needed-for-shuffling
        /// </summary>
        private static IEnumerable<T> ShuffleIterator<T>(IEnumerable<T> source, Random rng)
        {
            var buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }
}