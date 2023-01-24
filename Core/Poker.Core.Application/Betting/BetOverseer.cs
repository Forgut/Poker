using Poker.Core.Domain.Entity;
using System.Text;
using System.Threading;

namespace Poker.Core.Application.Betting
{
    public class BetOverseer //todo name
    {
        private PlayersRotation _playersRotation = new PlayersRotation(new Players()); //todo
        private Pot _pot = new Pot(); //todo

        public string GetCurrentlyBettingPlayer()
        {
            return _playersRotation.CurrentPlayer.Name;
        }

        public int GetAmountToCheck()
        {
            return _pot.AmountToCheck(_playersRotation.CurrentPlayer.Name);
        }

        public bool ExecuteForCurrentPlayer(string input) //todo name
        {
            var (decision, amount) = DecisionParser.Parse(input);

            if (decision == Decision.Unkown)
                return false;
            if (amount <= 0)
                return false;

            switch (decision)
            {
                case Decision.Check:
                    Check();
                    return true;
                case Decision.Raise:
                    Raise(amount!.Value);
                    return true;
                case Decision.Fold:
                    Fold();
                    return true;
            }
            return false;
        }

        private void Check()
        {
            var amount = _pot.AmountToCheck(_playersRotation.CurrentPlayer.Name);
            _pot.AddToPot(_playersRotation.CurrentPlayer.Name, amount);
            _playersRotation.MoveToNextPlayer();
        }

        private void Raise(int amount)
        {
            var amountToCheck = _pot.AmountToCheck(_playersRotation.CurrentPlayer.Name);
            _pot.AddToPot(_playersRotation.CurrentPlayer.Name, amountToCheck + amount);
            _playersRotation.MoveToNextPlayer();
        }

        private void Fold()
        {
            _playersRotation.CurrentPlayerFolded();
            _playersRotation.MoveToNextPlayer();
        }

        public void MoveBlinds()
        {
            _playersRotation.MoveBlinds();
        }
    }

    class Pot
    {
        public int AmountToCheck(string playerName) => 5; //todo
        public void AddToPot(string playerName, int amount)
        {
            //todo
        }
    }
}
