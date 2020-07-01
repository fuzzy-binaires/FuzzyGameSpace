using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

#if UNITY_EDITOR 
using UnityEditor;
#endif

/* -- manages login --*/
public class LogInController : MonoBehaviour
{
    [SerializeField] LogInData credentialsEditor;
    [SerializeField] TMP_InputField emailContent;
    [SerializeField] TMP_InputField passwordContent;

    [SerializeField] string sceneFuzzyHouse;
    [SerializeField] string sceneLogInFailed;

    static readonly string serverPath = "http://134.122.74.56/space/appdata/"; // use path to save data on virtual machine
    static string credentialsLocalPath() => Application.dataPath + "/Resources/" + "credentials.json";
    static string credentialsServerPath() => serverPath + "credentials.json"; // to put hard coded server path


  //  #if UNITY_EDITOR

    void Awake()
    {
        emailContent.text = PlayerPrefs.GetString("email_cookie");
    }

     #if UNITY_EDITOR
    [Button]
    void SaveCredentialsToFile()
    {
        string json = JsonUtility.ToJson(credentialsEditor);
        StreamWriter writer = new StreamWriter(credentialsLocalPath());
        writer.WriteLine(json);
        writer.Close();
        AssetDatabase.ImportAsset(credentialsLocalPath());
    }
    #endif

    [Button]
    LogInData ReadLocalCredentials()
    {
        StreamReader reader = new StreamReader(credentialsLocalPath());
        string json = reader.ReadToEnd();
        reader.Close();
        Debug.Log("json: " + json);
        return JsonUtility.FromJson<LogInData>(json);
    }

   // #endif

    public void AttemptLogIn()
    {
        StartCoroutine(DownloadString(credentialsServerPath(), OnReceivedLogInData));
    }    

    void OnReceivedLogInData (string json)
    {
        // once json is received, we parse it directly to the class LogInData
        LogInData data = JsonUtility.FromJson<LogInData>(json);

        // log-in data fields
        string email = emailContent.text;
        PlayerPrefs.SetString("email_cookie", email);
        string password = passwordContent.text;

        // we check if login is correct
        if (DoesLogInExist(data, email, password) == true) {
            SceneManager.LoadScene(sceneFuzzyHouse);
        } else {
            SceneManager.LoadScene(sceneLogInFailed);
        }
    }

    IEnumerator DownloadString(string uri, Action<string> callback)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // request and wait for page
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                callback(webRequest.downloadHandler.text);
                //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }

    static bool DoesLogInExist (LogInData logInData, string email, string password) {
        if (logInData == null)
        {
            Debug.LogError("LogInData is null!");
            return false;
        }

        for (int i = 0; i < logInData.credentials.Length; i++) {
            if (logInData.credentials == null || logInData.credentials[i] == null)
            {
                Debug.LogError("Credentials List is null!");
                return false;
            }

            email = email.Trim();
            string data_email = logInData.credentials[i].email.Trim();
            password = password.Trim();
            string data_password = logInData.credentials[i].password.Trim();

            if (string.Equals(data_email, email) && string.Equals(data_password, password)) {
                Debug.Log("Success.");
                return true;
            } 
        } 
        
        Debug.Log("Failed.");
        return false;
    }
}
