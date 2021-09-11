using System;
using System.Linq;
using System.Threading.Tasks;

namespace rocket_bot
{
    public partial class Bot
    {
        public Rocket GetNextMove(Rocket rocket)
        {
            var searchTaskResults = new Task<Tuple<Turn, double>>[threadsCount];
            var iterationsOneThread = iterationsCount / threadsCount;
            for (int i = 0; i < threadsCount; i++)
            {
                searchTaskResults[i] = Task.Factory.StartNew(()
                    => SearchBestMove(rocket, new Random(random.Next()), iterationsOneThread));
            }
            var newRocket = rocket.Move(searchTaskResults.Min(res => res.Result.Item1), level);
            return newRocket;
        }
    }
}