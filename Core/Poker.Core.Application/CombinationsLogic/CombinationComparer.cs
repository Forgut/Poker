﻿using System.Collections.Generic;
using System.Linq;

namespace Poker.Core.Application.CombinationsLogic
{
    public interface ICombinationComparer
    {
        IEnumerable<CombinationDTO> GetBestCombinations(IEnumerable<CombinationDTO> combinations);
    }

    public class CombinationComparer : ICombinationComparer
    {
        public IEnumerable<CombinationDTO> GetBestCombinations(IEnumerable<CombinationDTO> combinations)
        {
            var bestCombinations = combinations.GroupBy(x => x.Combination)
                .OrderByDescending(x => x.Key)
                .First();

            if (bestCombinations.Count() == 1)
                return bestCombinations;

            var orderedByBest = bestCombinations
                .OrderByDescending(x => x)
                .ToList();

            return orderedByBest
                .Where(x => x == orderedByBest.First());
        }
    }
}
