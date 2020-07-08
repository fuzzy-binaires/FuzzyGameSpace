using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpShower : MonoBehaviour
{

    //public Collider thisCollider;
    public TMP_Text popUpText;
    public GameObject popUpCanvasGroup;

    public bool playerIsDeciding;

    public string roomName;

    void Start()
    {
        //thisCollider = GetComponent<Collider>();

        popUpCanvasGroup = GameObject.Find("RoomConfirmPopUp");
        GameObject popUpTextGO = GameObject.Find("RoomConfirmPopUp/Canvas/popUpText").gameObject;
        popUpText = popUpTextGO.GetComponent<TMP_Text>();
        popUpText.text = "";


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
                Debug.Log("Open WebPage for Room" + roomName);
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
            popUpText.text = "<size=20>Press ENTER to leave to:\n<size=30>" + roomName;

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
