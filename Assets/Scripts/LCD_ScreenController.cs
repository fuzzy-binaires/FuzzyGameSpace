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


public class LCD_ScreenController : MonoBehaviour
{

    public TMP_Text LCDText;
    public float scrollSpeed = 1.0f;
    private string originalText;

    public float serverReadPeriod = 60.0f; //How often we refresh the string from the server

    private TMP_Text cloneText;
    private RectTransform textRectTransform;
    
    private string filePath;

    [SerializeField] LcdData lcdDataEditor;

    

    [System.Serializable]
    public class LcdData
    {
            public string text;
    }


    static string serverPath() =>  "http://134.122.74.56/space/appdata/lcdDatabase.json"; 


    void Start()
    {
        LCDText = GameObject.Find("LCD_Text_TMP").GetComponent<TMP_Text>();
        originalText = LCDText.text;
        textRectTransform = LCDText.GetComponent<RectTransform>();
        
        InvokeRepeating("Scroll", 1.0f, 0.3f);
        

        #if UNITY_EDITOR
            filePath = Application.dataPath + "/Resources/" + "lcdDatabase.json";
            LcdData data = readDataFromFile();
            LCDText.text = data.text;
        #else
            
            filePath = serverPath();
            InvokeRepeating("RequestJsonFromServer",  1.0f, serverReadPeriod);
        #endif

        

        

    }
    void RequestJsonFromServer(){
            StartCoroutine(DownloadString(filePath, OnReceivedLcdData));
    }
    void OnReceivedLcdData (string json)
    {
            
            LcdData data = JsonUtility.FromJson<LcdData>(json);
            LCDText.text = data.text;
            
       
    }


    void Scroll()
    {

        float width = LCDText.preferredWidth;

        Vector3 startPosition = textRectTransform.localPosition;
        float scrollPosition = 0;
        string text = LCDText.text;


        if (text.Length > 1){
            LCDText.text = text.Substring(1, text.Length - 1) + text.Substring(0, 1); 
        }
        

    }

    private LcdData readDataFromFile()
    {
        Debug.Log(filePath);
        string json = System.IO.File.ReadAllText(filePath);
        
        Debug.Log("json: " + json);
        return JsonUtility.FromJson<LcdData>(json);

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
                LcdData data = JsonUtility.FromJson<LcdData>(webRequest.downloadHandler.text);
                
            }
        }
    }
}
