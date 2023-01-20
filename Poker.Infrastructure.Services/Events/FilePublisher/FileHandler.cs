using System.Threading.Tasks;

namespace Poker.Infrastructure.Services.Events.FilePublisher
{
    public class FileHandler
    {
        private readonly string _fileName;

        public FileHandler(string fileName)
        {
            _fileName = fileName;
        }

        public Task AddToFileAsync(string content)
        {
            return System.IO.File.AppendAllTextAsync(_fileName, content);
        }
    }
}
