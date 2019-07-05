using Amazon.Lambda.Core;
using AWSLambdaGetRandomMobs.Models;
using AWSLambdaGetRandomMobs.Service;
using Newtonsoft.Json;

[assembly: LambdaSerializer(typeof(JsonSerializer))]

namespace AWSLambdaGetRandomMobs
{
    public class Function
    {
        /// <summary>
        ///     A simple function that takes a string and does a ToUpper
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        [LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]
        public string FunctionHandler(MobKillsLog input, ILambdaContext context)
        {
            //var randomMob =  new RandomizeMobs().GetRandomMobs();
            return input != null ? new KilledMob().AddKilledMob(input).HttpStatusCode.ToString() : "input is null";
        }
    }
}