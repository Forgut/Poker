using Poker.Core.Domain.Events;
using System.Threading.Tasks;

namespace Poker.Core.Application.Events
{
    public interface IEventPublisher
    {
        Task RoundEnded(RoundEndedEvent @event);
    }
}
