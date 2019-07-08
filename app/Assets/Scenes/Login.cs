using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Amazon;
using Amazon.DynamoDBv2;
using Amazon.Lambda;
using Assets;
using MoreMobs;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Login : MonoBehaviour
{
    private readonly string FunctionName = "LoginFunction";
    private AWSConnectionStringService connectionData = new AWSConnectionStringService();
    private IAmazonLambda _lambdaClient;
    private DynamoContext _dinamoClient;
    public GameObject password;
    public GameObject email;

    private IAmazonLambda Client
    {
        get
        {
            if (_lambdaClient == null) _lambdaClient = new AmazonLambdaClient(connectionData.AccessKey, connectionData.SecretKey, RegionEndpoint.EUCentral1);
            return _lambdaClient;
        }
    }

    private DynamoContext DynamoContext
    {
        get
        {
            if (_dinamoClient == null) _dinamoClient = new DynamoContext();
            return _dinamoClient;
        }
    }

    public void LoginButton()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        if (email.GetComponent<InputField>().text != "" &&
            password.GetComponent<InputField>().text != "")
        {

            var user = new User
            {
                Username = email.GetComponent<InputField>().text,
                Password = password.GetComponent<InputField>().text,
            };
            SentToLambda(user);
        }
    }

    private void SentToLambda(User user)
    {
        Client.InvokeAsync(new Amazon.Lambda.Model.InvokeRequest()
        {
            FunctionName = FunctionName,
            Payload = JsonConvert.SerializeObject(user)
        },
            (responseObject) =>
            {

                if (responseObject.Exception == null)
                {
                    Debug.Log(Encoding.ASCII.GetString(responseObject.Response.Payload.ToArray()));
                }
                else
                {
                    DynamoContext.CreatePlayer(responseObject.Response.ToString());
                    int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                    if (SceneManager.sceneCountInBuildSettings > nextSceneIndex)
                    {
                        SceneManager.LoadScene(nextSceneIndex);
                    }
                }
            }
        );
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (email.GetComponent<InputField>().isFocused) password.GetComponent<InputField>().Select();
        }
    }
    private class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
