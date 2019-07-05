using System;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Runtime;

namespace AWSLambdaGetRandomMobs.Data
{
    public class AmazonDynamoDBClientSingleton
    {
        private static readonly string AccessKey = Environment.GetEnvironmentVariable("AccessKey");
        private static readonly string SecretKey = Environment.GetEnvironmentVariable("SecretKey");
        private static readonly string ServiceUrl = Environment.GetEnvironmentVariable("ServiceURL");

        private static readonly Lazy<AmazonDynamoDBClient> LazyClient = new Lazy<AmazonDynamoDBClient>(
            new AmazonDynamoDBClient(GetBasicAwsCredentials, GetAmazonDynamoDbConfig));

        public static AmazonDynamoDBClient Instance => LazyClient.Value;

        private AmazonDynamoDBClientSingleton()
        {
        }

        private static BasicAWSCredentials GetBasicAwsCredentials => new BasicAWSCredentials(AccessKey, SecretKey);

        private static AmazonDynamoDBConfig GetAmazonDynamoDbConfig =>
            new AmazonDynamoDBConfig
            {
                ServiceURL = ServiceUrl,
                RegionEndpoint = RegionEndpoint.EUCentral1
            };
    }
}