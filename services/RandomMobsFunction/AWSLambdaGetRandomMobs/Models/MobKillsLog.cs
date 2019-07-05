using System;
using System.Collections.Generic;
using System.Text;
using Amazon.DynamoDBv2.DataModel;

namespace AWSLambdaGetRandomMobs.Models
{
    [DynamoDBTable("MobKillsLog")]
    public class MobKillsLog
    {
        [DynamoDBProperty("id")] public string Id { get; set; }
        [DynamoDBProperty("mobid")] public string MobId { get; set; }
        [DynamoDBProperty("playerid")] public string PlayerId { get; set; }
        [DynamoDBProperty("timestamp")] public string TimeStamp { get; set; }
    }
}
