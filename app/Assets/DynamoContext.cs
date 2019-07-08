
using Amazon.CognitoIdentity;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2.Model;
using Amazon.Runtime;
using Amazon;
using System;
using System.Collections.Generic;
using UnityEngine;
using MoreMobs;

namespace Assets
{
    class DynamoContext 
    {
        private IAmazonDynamoDB _client;
        private DynamoDBContext _context;
        private AWSConnectionStringService connectionData = new AWSConnectionStringService();
        public static string  tableName = "Player";
        public  string IdentityPoolId = "eu-central-1_2a1rrnynq";
        public  string CognitoPoolRegion = RegionEndpoint.EUCentral1.SystemName;
        public  string DynamoRegion = RegionEndpoint.EUCentral1.SystemName;

        private RegionEndpoint _CognitoPoolRegion
        {
            get { return RegionEndpoint.GetBySystemName(CognitoPoolRegion); }
        }

        private RegionEndpoint _DynamoRegion
        {
            get { return RegionEndpoint.GetBySystemName(DynamoRegion); }
        }

        private static IAmazonDynamoDB _ddbClient;

        private AWSCredentials _credentials;

        private AWSCredentials Credentials
        {
            get
            {
                if (_credentials == null)
                    _credentials = new BasicAWSCredentials(connectionData.AccessKey, connectionData.SecretKey);
                return _credentials;
            }
        }

        protected IAmazonDynamoDB Client
        {
            get
            {
                if (_ddbClient == null)
                {
                    _ddbClient = new AmazonDynamoDBClient(Credentials, _DynamoRegion);
                }

                return _ddbClient;
            }
        }

        private DynamoDBContext Context
        {
            get
            {
                if (_context == null)
                    _context = new DynamoDBContext(_client);

                return _context;
            }
        }

        public void CreatePlayer(string cognitoId)
        {
            _client = Client;
            var request = new PutItemRequest
            {
                TableName = "Player",
                Item = new Dictionary<string, AttributeValue>
                {
                    {"PlayerId", new AttributeValue {S = Guid.NewGuid().ToString()}},
                    {"CognitoUserId", new AttributeValue {S = cognitoId}},
                    {"DateCreated", new AttributeValue {S = DateTime.Now.ToString()}},
                    {"TrueGold", new AttributeValue {S = "0"}},
                    {"BonusGold", new AttributeValue {S = "0"}},
                    {"BonusCreds", new AttributeValue {S = "0"}},
                    {"TrueCreds", new AttributeValue {S = "0"}},
                    {"MobsKilled", new AttributeValue {S = "0"}},
                    {"ExperienceEarned", new AttributeValue {S = "0"}},
                    {"Level", new AttributeValue {S = "0"}},
                    {"Avatar", new AttributeValue {S = "Empty"}},
                }
            };
            _client.PutItemAsync(request, (result) =>
         {
             if (result.Exception == null)
                 Debug.Log("Player Created");
         });
        }
            

        private void UpdatePlayer(Player player)
        {
            _client = Client;
            var request = new UpdateItemRequest
            {
                TableName = "Player",
                AttributeUpdates = new Dictionary<string, AttributeValueUpdate>
                {
                    
                    {"TrueGold", new AttributeValueUpdate {Action = "0"}},
                    {"BonusGold", new AttributeValueUpdate {Action = "0"}},
                    {"TrueCreds", new AttributeValueUpdate {Action = "0"}},
                    {"MobsKilled", new AttributeValueUpdate {Action = "0"}},
                    {"ExperienceEarned", new AttributeValueUpdate {Action = "0"}},
                    {"Level", new AttributeValueUpdate {Action = "0"}},
                    {"Avatar", new AttributeValueUpdate {Action = "Empty"}},
                }
            };
            _client.UpdateItemAsync(request, (result) =>
            {
                if (result.Exception == null)
                    Debug.Log("Player Updated");
            });
        }

        public List<Dictionary<string, AttributeValue>> GetPlayer(string cognitoId)
        {
            _client = Client;
            Dictionary<string, AttributeValue> startKeys = null;
            List<Dictionary<string, AttributeValue>> player = null;

            var request = new QueryRequest
            {
                TableName = "Player",
                ReturnConsumedCapacity = "TOTAL",
                KeyConditions = new Dictionary<string, Condition>()
                {
                    {
                        "CognitoUserId",
                        new Condition
                        {
                            ComparisonOperator = "EQ",
                            AttributeValueList = new List<AttributeValue>()
                            {
                                new AttributeValue { S = cognitoId }
                            }
                        }
                    }
                },
            ExclusiveStartKey = startKeys,
            Limit = 2
            };

            _client.QueryAsync(request, (result) =>
            {
                foreach (Dictionary<string, AttributeValue> item
                         in result.Response.Items)
                {
                    player.Add(item);
                }
            });
            return player;
        }
    }
}

