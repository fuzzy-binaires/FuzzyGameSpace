using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

public class PopUpShower : MonoBehaviour
{

    [DllImport("__Internal")]
    private static extern void OpenBrowserTabJS(string url);

    public TMP_Text popUpText;
    public GameObject popUpCanvasGroup;

    public bool playerIsDeciding;

    public string roomName;
    public string webURLName;

    void Start()
    {

        popUpCanvasGroup = GameObject.Find("RoomConfirmPopUp");
        GameObject popUpTextGO = GameObject.Find("RoomConfirmPopUp/Canvas/popUpText").gameObject;
        popUpText = popUpTextGO.GetComponent<TMP_Text>();
        popUpText.text = "";

        webURLName = "http://134.122.74.56/" + webURLName;
        Debug.Log(webURLName);

        playerIsDeciding = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            if (playerIsDeciding)
            {
                // OPEN ROOM'S WEBPAGE
                //Debug.Log("Open WebPage for Room" + roomName);

                OpenBrowserTabJS(webURLName);
                playerIsDeciding = false;
                popUpText.text = "";


            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log(other.gameObject.name + " <=> " + roomName);

            playerIsDeciding = true;
            //popUpCanvasGroup.SetActive(true);
            popUpText.text = "<size=20>Press ENTER to go to:\n<size=30>| " + roomName + " | ";

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log(other.gameObject.name + " <=> " + roomName);
            playerIsDeciding = false;
            popUpText.text = "";

            //popUpCanvasGroup.SetActive(false);
        }

    }
}
