using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMusicGui : MonoBehaviour
{
    public bool isMusicGuiVisible = false;
    public GameObject MusicPlayerCanvasGroup;
    // Start is called before the first frame update
    void Start()
    {
    //  MusicPlayerCanvasGroup = GameObject.Find("MusicPlayerCanvasGroup");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
       if(other.CompareTag("Player"))
       {
          isMusicGuiVisible = true;
          MusicPlayerCanvasGroup.SetActive(true);
       }

    }

    private void OnTriggerExit(Collider other)
    {
       if(other.CompareTag("Player"))
       {
          isMusicGuiVisible = false;
          MusicPlayerCanvasGroup.SetActive(false);
       }

    }
}
