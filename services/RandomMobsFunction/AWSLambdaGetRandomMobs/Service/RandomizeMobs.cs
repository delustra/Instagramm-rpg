using System.Collections.Generic;
using System.Threading.Tasks;
using AWSLambdaGetRandomMobs.Data;
using AWSLambdaGetRandomMobs.Models;

namespace AWSLambdaGetRandomMobs.Service
{
    public class RandomizeMobs
    {
        public Task<List<Mob>> GetRandomMobs()
        {
            return new MobRepository().MobsList();
        }
    }
}