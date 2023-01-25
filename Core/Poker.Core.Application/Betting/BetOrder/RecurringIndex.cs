namespace Poker.Core.Application.Betting.BetOrder
{
    public struct RecurringIndex
    {
        private readonly int _maxValue;

        public RecurringIndex(int maxValue) : this()
        {
            _maxValue = maxValue;
        }

        private int _value;
        public int Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (value >= _maxValue)
                    _value = value % _maxValue;
                else
                    _value = value;
            }
        }
    }
}
