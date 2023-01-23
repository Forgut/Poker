using Poker.Core.Application.CombinationsLogic;
using Poker.Core.Application.Events;
using Poker.Core.Application.GameBehaviour.WinCalculation;
using Poker.Core.Domain.Entity;
using Poker.Core.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poker.Core.Application.GameBehaviour
{
    public class StandardGame : Game
    {
        public StandardGame(CombinationComparer combinationComparer,
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

        }

        public void InsertTargetPlayerCards(string cards, char separator = ';')
        {
            var split = cards.Split(separator);
            _playersInfo.TargetPlayer.SetFirstCard(Card.FromString(split[0]));
            _playersInfo.TargetPlayer.SetSecondCard(Card.FromString(split[1]));
            GameState = EGameState.PreFlopBet;
        }

        public void Flop(string cards, char separator = ';')
        {
            var split = cards.Split(separator);
            _table.SetFirstCard(Card.FromString(split[0]));
            _table.SetSecondCard(Card.FromString(split[1]));
            _table.SetThirdCard(Card.FromString(split[2]));
            GameState = EGameState.FlopBet;
        }

        public void Turn(string card)
        {
            _table.SetFourthCard(Card.FromString(card));
            GameState = EGameState.TurnBet;
        }

        public void River(string card)
        {
            _table.SetFifthCard(Card.FromString(card));
            GameState = EGameState.RiverBet;
        }

        public void SetGameStateAsEnd()
        {
            GameState = EGameState.End;
        }

        public void FillPlayersCards(string playerName, string cards, char separator = ';')
        {
            var player = _playersInfo.Players.First(x => x.Name == playerName);
            var split = cards.Split(separator);
            player.SetFirstCard(Card.FromString(split[0]));
            player.SetSecondCard(Card.FromString(split[1]));
        }

        public void Bet()
        {
            //todo logic of making bets, using new module.
            //at this moment only proceed directly to next game state.
            GameState++;
        }
    }
}
