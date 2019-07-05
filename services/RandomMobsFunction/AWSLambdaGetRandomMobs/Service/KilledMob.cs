using Amazon.DynamoDBv2.Model;
using AWSLambdaGetRandomMobs.Data;
using AWSLambdaGetRandomMobs.Models;

namespace AWSLambdaGetRandomMobs.Service
{
    public class KilledMob
    {
        public PutItemResponse AddKilledMob(MobKillsLog mobKill)
        {
            return new MobRepository().AddKills(mobKill);
        }
    }
}