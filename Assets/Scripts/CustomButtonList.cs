using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class CustomButtonList : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void OpenBrowserTabJS(string url);


    public GameObject prefabButton;
    public RectTransform ParentPanel;

    public int buttonCount = 0;
   
    // Use this for initialization
    void Start () {
       CreateButton("https://www.youtube.com/watch?v=mA808ppj8OQ");
        
       
    }

    public void CreateButton(string url ){

        GameObject goButton = (GameObject)Instantiate(prefabButton);
        goButton.transform.SetParent(ParentPanel, false);
        goButton.transform.localPosition = new Vector2(0,buttonCount*20);
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
