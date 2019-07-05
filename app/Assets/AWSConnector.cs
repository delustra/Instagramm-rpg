using UnityEngine;
using UnityEngine.UI;
using System;
using Amazon.Lambda;
using Amazon.Runtime;
using Amazon.CognitoIdentity;
using Amazon;
using MoreMobs.MobQ;
using MoreMobs.Model;
using System.Text;
using System.Collections.Generic;
using Amazon.Lambda.Model;
using Newtonsoft.Json;




namespace MoreMobs.AWS
{
    public class AWSConnector : MonoBehaviour
    {
        public string IdentityPoolId = "";
        public string CognitoIdentityRegion;

        //AWS Lambda Function names
        public string KillMobFunctionName = "";
        public string DeliverMobFunctionName = "MobDistributionv1";



        private RegionEndpoint _CognitoIdentityRegion
        {
            get { return RegionEndpoint.GetBySystemName(CognitoIdentityRegion); }
        }
        public string LambdaRegion;
        private RegionEndpoint _LambdaRegion
        {
            get { return RegionEndpoint.GetBySystemName(LambdaRegion); }
        }


        public Button InvokeButton = null;
        
        public Text FunctionNameText = null;
        public Text EventText = null;
        public Text ResultText = null;
        public List<Mob> Mobs = null;
        public MobQueue MobsQueue;

        void Start()
        {
            UnityInitializer.AttachToGameObject(this.gameObject);
            Amazon.AWSConfigs.HttpClient = Amazon.AWSConfigs.HttpClientOption.UnityWebRequest;
            InvokeButton.onClick.AddListener(() => { Invoke(); });
//          ListFunctionsButton.onClick.AddListener(() => { ListFunctions(); });
        }

        #region private members

        private IAmazonLambda _lambdaClient;
        private AWSCredentials _credentials;
        private AWSCredentials Credentials
        {
            get
            {
                if (_credentials == null)
                    _credentials = new CognitoAWSCredentials(IdentityPoolId, _CognitoIdentityRegion);
                return _credentials;
            }
        }

        private IAmazonLambda Client
        {
            get
            {
                if (_lambdaClient == null)
                {
                    _lambdaClient = new AmazonLambdaClient(Credentials, _LambdaRegion);
               
                }
                return _lambdaClient;
            }
        }

        #endregion

        #region Invoke
        /// <summary>
        /// Example method to demonstrate Invoke. Invokes the Lambda function with the specified
        /// function name (e.g. helloWorld) with the parameters specified in the Event JSON.
        /// Because no InvokationType is specified, the default 'RequestResponse' is used, meaning
        /// that we expect the AWS Lambda function to return a value.
        /// </summary>
        public void Invoke()
        {
            ResultText.text = "Invoking '" + FunctionNameText.text + " function in Lambda... \n";
            Client.InvokeAsync(new Amazon.Lambda.Model.InvokeRequest()
            {
                FunctionName = DeliverMobFunctionName,
                Payload = EventText.text
            },
            (responseObject) =>
            {
                ResultText.text += "\n";
                if (responseObject.Exception == null)
                {
                    ResultText.text = Encoding.ASCII.GetString(responseObject.Response.Payload.ToArray()) + "\n";
                    Mobs = JsonConvert.DeserializeObject<List<Mob>>(ResultText.text);
                    MobsQueue.DeliverNewMobs(Mobs);           }
                else
                {
                    ResultText.text += responseObject.Exception + "\n";
                }
            }
            );
        }
        public void KillMob(Mob mob)
        {
            DeadMob deadMob = new DeadMob
            {
                id = Time.time.ToString(),
                mobid = mob.id,
                playerid = "player1",
                timestamp = DateTime.Now.ToString()
            };

            string DeadMobSerialized = JsonConvert.SerializeObject(deadMob);
            ResultText.text = "Invoking '" + KillMobFunctionName + " function in Lambda... \n";

            Client.InvokeAsync(new Amazon.Lambda.Model.InvokeRequest()
            {
                FunctionName = KillMobFunctionName,
                Payload = DeadMobSerialized
            },
             (responseObject) =>
              {
                  ResultText.text += "\n";
                  if (responseObject.Exception == null)
                  {
                      ResultText.text = Encoding.ASCII.GetString(responseObject.Response.Payload.ToArray()) + "\n";

                  }
                  else
                  {
                      ResultText.text += responseObject.Exception + "\n";
                  }
              }
           );

            // { "id": "log1", "mobid": "testmob1", "playerid": "player1", "timestamp": "1550074014" }


        }

        #endregion

        #region List Functions
        /// <summary>
        /// Example method to demonstrate ListFunctions
        /// </summary>
        public void ListFunctions()
        {
            ResultText.text = "Listing all of your Lambda functions... \n";
            Client.ListFunctionsAsync(new Amazon.Lambda.Model.ListFunctionsRequest(),
            (responseObject) =>
            {
                ResultText.text += "\n";
                if (responseObject.Exception == null)
                {
                    ResultText.text += "Functions: \n";
                    foreach (FunctionConfiguration function in responseObject.Response.Functions)
                    {
                        ResultText.text += "    " + function.FunctionName + "\n";
                    }
                }
                else
                {
                    ResultText.text += responseObject.Exception + "\n";
                }
            }
            );
        }

        #endregion
    }
}
