using System.Threading.Tasks;

namespace StarKnights
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private async static Task Main(string[] args)
        {
            using StarKnightsGame game = new StarKnightsGame();
            var mainLoop = Task.Factory.StartNew(() =>
            {
                game.Run();
            });
            while (!mainLoop.IsCompleted)
            {
                await Task.WhenAny(new Task[] { mainLoop, Task.Delay(50) });
            }
        }
    }
}

