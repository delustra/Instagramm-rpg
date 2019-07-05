using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using AWSLambdaGetRandomMobs.Models;

namespace AWSLambdaGetRandomMobs.Data
{
    public class MobRepository
    {
        private readonly DynamoDBContext _context;
        private readonly AmazonDynamoDBClient _client;

        public MobRepository()
        {
            _client = AmazonDynamoDBClientSingleton.Instance;
            _context = new DynamoDBContext(_client);
        }

        public async Task<List<Mob>> MobsList()
        {
            var mobIds = await GetMobIds();

            var mobsList = new List<Mob>();
            var random = new Random();
            const int mobsMaxLimit = 3;
            for (var i = 0; i < mobsMaxLimit; i++)
            {
                var index = random.Next(mobIds.Count);
                mobsList.Add(await _context.LoadAsync<Mob>(mobIds[index],
                    new DynamoDBContextConfig {ConsistentRead = true, SkipVersionCheck = true}));
            }

            return mobsList;
        }

        private async Task<List<string>> GetMobIds()
        {
            Dictionary<string, AttributeValue> startKeys = null;

            const string tableName = "mobs1";
            const string keyName = "id";
            const int limit = 10;
            var request = new ScanRequest
            {
                TableName = tableName,
                Limit = limit
            };
            var keyList = new List<string>();
            do
            {
                request.ExclusiveStartKey = startKeys;
                var response = await _client.ScanAsync(request);

                response.Items.ForEach(
                    item => keyList.AddRange(item.Where(x => x.Key == keyName)
                        .Select(y => y.Value.S)
                        .ToList()));

                startKeys = response.LastEvaluatedKey;
            } while (startKeys != null && startKeys.Count != 0);

            return keyList;
        }

        public PutItemResponse AddKills(MobKillsLog killsLog)
        {
            var tableName = "MobKillsLog";

            var request = new PutItemRequest
            {
                TableName = tableName,
                Item = new Dictionary<string, AttributeValue>
                {
                    {"id", new AttributeValue {S = killsLog.Id}},
                    {"mobid", new AttributeValue {S = killsLog.MobId}},
                    {"playerid", new AttributeValue {S = killsLog.PlayerId}},
                    {"timestamp", new AttributeValue {S = killsLog.TimeStamp}}
                }
            };
            return _client.PutItemAsync(request).Result;
        }
    }
}