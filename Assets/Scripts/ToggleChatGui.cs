using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleChatGui : MonoBehaviour
{
   public bool isChatGuiVisible = false;

   private bool insideCollider = false;
   public GameObject ChatCanvasGroup;

   private GUIStyle guiStyle = new GUIStyle(); 
    // Start is called before the first frame update
    void Awake()
    {
      ChatCanvasGroup = GameObject.Find("ChatCanvasGroup");
      guiStyle.fontSize = 30; 
      guiStyle.normal.textColor = Color.red;

    }

    // Update is called once per frame
    void Update()
    {
       if (!isChatGuiVisible && insideCollider && Input.GetKeyUp(KeyCode.T)){
               isChatGuiVisible = true;
               ChatCanvasGroup.SetActive(true);
             
       }

       if (isChatGuiVisible && Input.GetKeyDown(KeyCode.Escape)){
          isChatGuiVisible = false;
          ChatCanvasGroup.SetActive(false);
       }
    }

    void OnGUI(){
       if (insideCollider && !isChatGuiVisible){
           
           GUI.Label(new Rect(5, Screen.height - 30, 200, 30), "Press 't' to chat", guiStyle);
       }
    }

    private void OnTriggerEnter(Collider other)
    {
      if(other.CompareTag("Player"))
       {
         insideCollider = true;
       }

    }

    private void OnTriggerExit(Collider other)
    {
       
       if(other.CompareTag("Player"))
       {
          insideCollider = false;
          
       }

    }
}
