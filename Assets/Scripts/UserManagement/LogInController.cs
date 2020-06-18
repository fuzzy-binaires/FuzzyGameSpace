using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System;
using System.IO;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

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

    const string serverPath = "";
    static string credentialsLocalPath() => Application.dataPath + "/Resources/" + "credentials.json";
    static string credentialsServerPath() => serverPath + "credentials.json"; // to put hard coded server path


    #if UNITY_EDITOR

    void Awake()
    {
        emailContent.text = PlayerPrefs.GetString("email_cookie");
    }

    [Button]
    void SaveCredentialsToFile()
    {
        string json = JsonUtility.ToJson(credentialsEditor);
        StreamWriter writer = new StreamWriter(credentialsLocalPath());
        writer.WriteLine(json);
        writer.Close();
        AssetDatabase.ImportAsset(credentialsLocalPath());
    }

    [Button]
    LogInData ReadLocalCredentials()
    {
        StreamReader reader = new StreamReader(credentialsLocalPath());
        string json = reader.ReadToEnd();
        reader.Close();
        Debug.Log("json: " + json);
        return JsonUtility.FromJson<LogInData>(json);
    }

    #endif

    public void AttemptLogIn()
    {
        string email = emailContent.text;
        PlayerPrefs.SetString("email_cookie", email);

        string password = passwordContent.text;

        LogInData data = ReadLocalCredentials();

        //Debug.Log("Attempting Login for " + email + " and " + password);

        if (DoesLogInExist(data, email, password) == true) {
            SceneManager.LoadScene(sceneFuzzyHouse);
        } else {
            SceneManager.LoadScene(sceneLogInFailed);
        }
    }    

    LogInData ReadServerCredentials()
    {
        StreamReader reader = new StreamReader(credentialsLocalPath());
        string json = reader.ReadToEnd();
        reader.Close();
        //Debug.Log("json: " + json);
        return JsonUtility.FromJson<LogInData>(json);
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
