using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;
using TMPro;

public class CustomButtonList : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OpenBrowserTabJS(string url);


    public GameObject prefabButton;
    public GameObject prefabInputField;
    public RectTransform ParentPanel;

    public TMP_InputField inputUrlTMP;


    public int buttonCount = 0;
   
    // Use this for initialization
    void Awake () {

        GameObject goInputField = (GameObject)Instantiate(prefabInputField);
        goInputField.transform.SetParent(ParentPanel, false);
        goInputField.transform.localPosition = new Vector2(0,buttonCount*20);
        goInputField.transform.localScale = new Vector3(1, 1, 1);

        inputUrlTMP = goInputField.GetComponent<TMP_InputField>();
        buttonCount++;

       CreateButton("https://www.youtube.com/watch?v=mA808ppj8OQ");
        
       
    }

    public string GetInputUrl(){
        return inputUrlTMP.text;
    }

    public void CreateButton(string url ){

        GameObject goButton = (GameObject)Instantiate(prefabButton);
        goButton.transform.SetParent(ParentPanel, false);
        goButton.transform.localPosition = new Vector2(0,buttonCount*-20);
        goButton.transform.localScale = new Vector3(1, 1, 1);
        
        
        goButton.GetComponent<Button>().GetComponentInChildren<Text>().text = url;
        
        
        goButton.GetComponent<Button>().onClick.AddListener(() => ButtonClicked(url));
        buttonCount++;
    }
   
    void ButtonClicked(string url)
    {
        

        Debug.LogError("ButtonClicked: " + url);
      // https://docs.unity3d.com/Manual/webgl-interactingwithbrowserscripting.html

        #if UNITY_EDITOR
            Application.OpenURL(url);
        #else
            OpenBrowserTabJS(url);
        #endif
    }
}
