using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Exceptions;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Poker.Core.Application.GameBehaviour.WinCalculation
{
    public class WinDecision
    {
        private readonly ITable _table;
        private readonly CombinationComparer _combinationComparer;

        public WinDecision(ITable table, CombinationComparer combinationComparer)
        {
            _table = table;
            _combinationComparer = combinationComparer;
        }

        private ReadOnlyCollection<Winner>? _winners;
        public ReadOnlyCollection<Winner> GetWinners(IEnumerable<Player> notFoldedPlayers) => _winners ??= CalculateWinners(notFoldedPlayers);

        public void ResetWinners()
        {
            _winners = null;
        }

        public ReadOnlyCollection<Winner> CalculateWinners(IEnumerable<Player> notFoldedPlayers)
        {
            if (!_table.IsFullySet)
                throw new UnableToCalculateWinnerException();

            var playersCombinations = notFoldedPlayers
                .Where(x => x.HasCards)
                .Select(x => new
                {
                    Player = x,
                    Combination = new CombinationFinder(x, _table).GetBestCombination()
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
