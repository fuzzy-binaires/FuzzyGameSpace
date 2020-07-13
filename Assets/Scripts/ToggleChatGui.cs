using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleChatGui : MonoBehaviour
{
   public bool isChatGuiVisible = false;

   private bool insideCollider = false;
   public GameObject ChatCanvasGroup;
    // Start is called before the first frame update
    void Awake()
    {
      ChatCanvasGroup = GameObject.Find("ChatCanvasGroup");

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

    void OnGui(){
       if (insideCollider && !isChatGuiVisible){
           GUI.Label(new Rect(5, Screen.height - 25, 200, 25), "Press 'T' to chat");
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
