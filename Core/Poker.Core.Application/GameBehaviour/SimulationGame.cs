using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Application.Events;
using Poker.Core.Domain.Entity;

namespace Poker.Core.Application.GameBehaviour
{
    public class SimulationGame : Game
    {
        private readonly Croupier _croupier;

        public SimulationGame(Croupier croupier,
                    CombinationComparer combinationComparer,
                    WinChanceEstimator winChanceEstimator,
                    Table table,
                    Players players,
                    IEventPublisher eventPublisher)
            : base(combinationComparer,
                 winChanceEstimator,
                 table,
                 players,
                 eventPublisher)
        {
            _croupier = croupier;
        }

        public void DrawPlayerCards()
        {
            _croupier.PreFlop(_playersInfo.Players);
            GameState = EGameState.PreFlop;
        }

        public void Flop()
        {
            _croupier.Flop(_table);
            GameState = EGameState.Flop;
        }

        public void Turn()
        {
            _croupier.Turn(_table);
            GameState = EGameState.Turn;
        }

        public void River()
        {
            _croupier.River(_table);
            GameState = EGameState.River;
        }
    }
}
