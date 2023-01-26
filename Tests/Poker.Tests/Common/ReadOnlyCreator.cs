using System.Collections.ObjectModel;
using System.Linq;

namespace Poker.Tests.Common
{
    static class ReadOnlyCreator
    {
        public static ReadOnlyCollection<T> GetReadonlyCollection<T>(params T[] values)
        {
            return values.ToList().AsReadOnly();
        }
    }
}
