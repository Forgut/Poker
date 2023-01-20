using Poker.Core.Application.Events;
using Poker.Core.Domain.Events;
using System.Threading.Tasks;

namespace Poker.Infrastructure.Services.Events
{
    public class EmptyEventPublisher : IEventPublisher
    {
        public Task RoundEnded(RoundEndedEvent @event)
        {
            return Task.CompletedTask;
        }
    }
}
