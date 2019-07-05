using Amazon.DynamoDBv2.DataModel;

namespace AWSLambdaGetRandomMobs.Models
{
    [DynamoDBTable("mobs1")]
    public class Mob
    {
        [DynamoDBProperty("id")] public string Id { get; set; }
        [DynamoDBProperty("imgurl")] public string ImgLink { get; set; }
        [DynamoDBProperty("title")] public string Title { get; set; }
    }
}