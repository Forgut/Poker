using Poker.Core.Application.Events;
using Poker.Core.Domain.Events;
using System.Linq;
using System.Threading.Tasks;

namespace Poker.Infrastructure.Services.Events.FilePublisher
{
    public class SaveEventsToFilePublisher : IEventPublisher
    {
        private readonly FileHandler _fileHandler;
        public SaveEventsToFilePublisher()
        {
            _fileHandler = new FileHandler("results.txt");
        }
        public async Task RoundEnded(RoundEndedEvent @event)
        {
            var winnerAndCombinationMessage = string.Join("\n", @event.Winners
                .Select(x => $"{x.Name} with {x.Combination}"));

            var message = $"Round ended at {@event.Occured}: winner(s):\n" +
                $"{winnerAndCombinationMessage}\n";

            await _fileHandler.AddToFileAsync(message);
        }
    }
}
