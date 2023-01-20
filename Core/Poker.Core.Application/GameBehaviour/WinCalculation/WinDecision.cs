using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Exceptions;
using System.Collections.ObjectModel;
using System.Linq;

namespace Poker.Core.Application.GameBehaviour.WinCalculation
{
    public class WinDecision
    {
        private readonly Table _table;
        private readonly Players _players;
        private readonly CombinationComparer _combinationComparer;

        public WinDecision(Table table, Players players, CombinationComparer combinationComparer)
        {
            _table = table;
            _players = players;
            _combinationComparer = combinationComparer;
        }

        private ReadOnlyCollection<Winner>? _winners;
        public ReadOnlyCollection<Winner> Winners => _winners ??= CalculateWinners();

        public void ResetWinners()
        {
            _winners = null;
        }

        public ReadOnlyCollection<Winner> CalculateWinners()
        {
            if (!_table.IsFullySet)
                throw new UnableToCalculateWinnerException();

            var playersCombinations = _players
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
                .Select(x => new Winner(x.Player.Name, x.Combination.ToString()))
                .ToList()
                .AsReadOnly();
        }
    }
}
