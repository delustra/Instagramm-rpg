using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Amazon.Internal;
using Amazon.Lambda.Core;
using Amazon.Runtime;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AWSLambdaLoginFunction
{
    public class Function
    {
        private const string _clientId = "";
        private const string _accessKey = "";
        private const string _secretKey = "";

        private static BasicAWSCredentials GetBasicAwsCredentials => new BasicAWSCredentials(_accessKey, _secretKey);

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<string> FunctionHandlerAsync(User user)
        {

            var cognito = new AmazonCognitoIdentityProviderClient(GetBasicAwsCredentials, RegionEndpoint.EUCentral1);

            var request = new SignUpRequest
            {
                ClientId = _clientId,
                Password = user.Password,
               Username = user.Username
            };

            var emailAttribute = new AttributeType
            {
                Name = "email",
                Value = user.Email
            };
            request.UserAttributes.Add(emailAttribute);

            var response = await cognito.SignUpAsync(request);

            return "OK";
        }
    }
}
