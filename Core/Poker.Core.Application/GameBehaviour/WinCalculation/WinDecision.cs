﻿using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Exceptions;
using Poker.Core.Domain.Interfaces;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Poker.Core.Application.GameBehaviour.WinCalculation
{
    public interface IWinDecision
    {
        ReadOnlyCollection<Winner> GetWinners(IEnumerable<ICardsHolder> notFoldedPlayers);
        void ResetWinners();
    }

    public class WinDecision : IWinDecision
    {
        private readonly ITable _table;
        private readonly ICombinationComparer _combinationComparer;
        private readonly ICombinationFinder _combinationFinder;

        public WinDecision(ITable table,
                           ICombinationComparer combinationComparer,
                           ICombinationFinder combinationFinder)
        {
            _table = table;
            _combinationComparer = combinationComparer;
            _combinationFinder = combinationFinder;
        }

        private ReadOnlyCollection<Winner>? _winners;
        public ReadOnlyCollection<Winner> GetWinners(IEnumerable<ICardsHolder> notFoldedPlayers) => _winners ??= CalculateWinners(notFoldedPlayers);

        public void ResetWinners()
        {
            _winners = null;
        }

        private ReadOnlyCollection<Winner> CalculateWinners(IEnumerable<ICardsHolder> notFoldedPlayers)
        {
            if (!_table.IsFullySet)
                throw new UnableToCalculateWinnerException();

            var playersCombinations = notFoldedPlayers
                .Where(x => x.HasCards)
                .Select(x => new
                {
                    Player = x,
                    Combination = _combinationFinder.GetBestCombination(x, _table)
                });

            var winners = _combinationComparer
               .GetBestCombinations(playersCombinations.Select(x => x.Combination));

            var winnersAndCombinations = playersCombinations.Where(x => winners.Contains(x.Combination))
                .Select(x => new { x.Player, x.Combination });

            return winnersAndCombinations
                .Select(x => new Winner(x.Player, x.Combination.ToString()))
                .ToList()
                .AsReadOnly();
        }
    }
}
