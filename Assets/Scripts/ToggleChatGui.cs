using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleChatGui : MonoBehaviour
{
   public bool isChatGuiVisible = false;
   public GameObject ChatCanvasGroup;
    // Start is called before the first frame update
    void Start()
    {
      ChatCanvasGroup = GameObject.Find("ChatCanvasGroup");

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Player"))
       {
          isChatGuiVisible = true;
          ChatCanvasGroup.SetActive(true);
       }

    }

    private void OnTriggerExit(Collider other)
    {
       if(other.CompareTag("Player"))
       {
          isChatGuiVisible = false;
          ChatCanvasGroup.SetActive(false);
       }

    }
}
