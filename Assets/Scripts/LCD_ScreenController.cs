using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;


public class LCD_ScreenController : MonoBehaviour
{

    public TMP_Text LCDText;
    public float scrollSpeed = 1.0f;
    private string originalText;

    public float serverReadPeriod = 20.0f; //How often we refresh the string from the server

    private TMP_Text cloneText;
    private RectTransform textRectTransform;
    
    private string credentialsPath;

    [SerializeField] LcdData lcdDataEditor;

    

    [System.Serializable]
    public class LcdData
    {
            public string text;
    }



    void Start()
    {
        LCDText = GameObject.Find("LCD_Text_TMP").GetComponent<TMP_Text>();
        originalText = LCDText.text;
        textRectTransform = LCDText.GetComponent<RectTransform>();


        //StartCoroutine("Scroll");
        InvokeRepeating("Scroll", 1.0f, 0.3f);
        InvokeRepeating("UpdateJsonFromServer", 1.0f, serverReadPeriod);
        //Debug.Log("In this Done");

        // use path to save data on virtual machine
        #if UNITY_EDITOR
            credentialsPath = Application.dataPath + "/Resources/" + "lcdDatabase.json";
            
        #else
            string serverPath = "http://134.122.74.56/space/appdata/";
            credentialsPath = serverPath + "lcdDatabase.json"; // to put hard coded server path
        #endif

    }

    private void Update()
    {
        //LCDText.text = TExtDelJSON; // TODO
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

    void UpdateJsonFromServer()
    {
        var data = readDataFromFile();

        LCDText.text = data.text;
    }


    private LcdData readDataFromFile()
    {
        Debug.Log(credentialsPath);
        StreamReader reader = new StreamReader(credentialsPath);
        string json = reader.ReadToEnd();
        reader.Close();
        Debug.Log("json: " + json);
        return JsonUtility.FromJson<LcdData>(json);

    }
}
