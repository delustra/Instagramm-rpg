using Amazon.DynamoDBv2.DataModel;

namespace Assets
{
    [DynamoDBTable("Player")]
    public class Player
    {
        [DynamoDBHashKey("PlayerId")] public string PlayerId { get; set; }
        [DynamoDBProperty("CognitoUserId")] public string CognitoUserId { get; set; }
        [DynamoDBProperty("DateCreated")] public string DateCreated { get; set; }
        [DynamoDBProperty("TrueGold")] public string TrueGold { get; set; }
        [DynamoDBProperty("BonusGold")] public string BonusGold { get; set; }
        [DynamoDBProperty("TrueCreds")] public string TrueCreds { get; set; }
        [DynamoDBProperty("BonusCreds")] public string BonusCreds { get; set; }
        [DynamoDBProperty("MobsKilled")] public string MobsKilled { get; set; }
        [DynamoDBProperty("ExperienceEarned")] public string ExperienceEarned { get; set; }
        [DynamoDBProperty("Level")] public string Level { get; set; }
        [DynamoDBProperty("Avatar")] public string Avatar { get; set; }
    }
}
