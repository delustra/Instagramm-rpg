using System.Text;
using Amazon;
using Amazon.Lambda;
using Assets;
using MoreMobs;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Register : MonoBehaviour
{
    private readonly string FunctionName = "RegisterFunction";
    private IAmazonLambda _lambdaClient;
    private DynamoContext _dinamoClient;
    public GameObject confPassword;
    private string ConfPassword;
    public GameObject email;
    private string Email;
    public GameObject password;
    private string Password;
    public string Region = RegionEndpoint.EUCentral1.SystemName;
    public GameObject username;
    private string Username;
    private AWSConnectionStringService connectionData = new AWSConnectionStringService();


    private DynamoContext DynamoContext
    {
        get
        {
            if (_dinamoClient == null) _dinamoClient = new DynamoContext();
            return _dinamoClient;
        }
    }

    private IAmazonLambda Client
    {
        get
        {
            if (_lambdaClient == null) _lambdaClient = new AmazonLambdaClient(connectionData.AccessKey, connectionData.SecretKey, RegionEndpoint.EUCentral1);
            return _lambdaClient;
        }
    }

    public void RegisterButton()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        if (password.GetComponent<InputField>().text != "" && email.GetComponent<InputField>().text != "" &&
            username.GetComponent<InputField>().text != "" && confPassword.GetComponent<InputField>().text != "")
        {
            if (password.GetComponent<InputField>().text != confPassword.GetComponent<InputField>().text)
            {
                Debug.Log("Pass not matched");
                return;
            }

            var user = new User
            {
                Username = username.GetComponent<InputField>().text,
                Email = email.GetComponent<InputField>().text,
                Password = password.GetComponent<InputField>().text,
                NickName = username.GetComponent<InputField>().text
            };
            SentToLambda(user);
        }
    }

    public  void SentToLambda(User user)
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
                    Debug.Log(Encoding.ASCII.GetString(responseObject.Response.Payload.ToArray()) );
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

    private void Update()  
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (username.GetComponent<InputField>().isFocused) email.GetComponent<InputField>().Select();

            if (email.GetComponent<InputField>().isFocused) password.GetComponent<InputField>().Select();

            if (password.GetComponent<InputField>().isFocused) confPassword.GetComponent<InputField>().Select();
        }
    }
}