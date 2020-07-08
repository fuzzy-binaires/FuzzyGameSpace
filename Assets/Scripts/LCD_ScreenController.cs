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

    private TMP_Text cloneText;
    private RectTransform textRectTransform;
    private string sourceText;
    private string tempText;

    [SerializeField] LcdData lcdDataEditor;

    static readonly string serverPath = "http://134.122.74.56/space/appdata/"; // use path to save data on virtual machine
    //static string credentialsLocalPath() => Application.dataPath + "/Resources/" + "lcdDatabase.json";
    static string credentialsServerPath() => serverPath + "lcdDatabase.json"; // to put hard coded server path


    [System.Serializable]
    public class LcdData
    {
        [System.Serializable]
        public class LcdDescription
        {
            public string text;
        }
        public LcdDescription pinDescriptions;


    }



    void Start()
    {
        LCDText = GameObject.Find("LCD_Text_TMP").GetComponent<TMP_Text>();
        originalText = LCDText.text;
        textRectTransform = LCDText.GetComponent<RectTransform>();


        //StartCoroutine("Scroll");
        InvokeRepeating("Scroll", 5.0f, 0.3f);
        InvokeRepeating("UpdateJsonFromServer", 1.0f, 10.0f);
        //Debug.Log("In this Done");

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



        LCDText.text = text.Substring(1, text.Length - 1) + text.Substring(0, 1); ;

    }

    void UpdateJsonFromServer()
    {
        readDataFromFile();
    }


    private LcdData readDataFromFile()
    {
        StreamReader reader = new StreamReader(credentialsServerPath());
        string json = reader.ReadToEnd();
        reader.Close();
        //Debug.Log("json: " + json);
        return JsonUtility.FromJson<LcdData>(json);

    }
}
