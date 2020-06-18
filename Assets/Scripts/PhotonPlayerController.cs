using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonPlayerController : MonoBehaviour
{
    private PhotonView photonView;
    public static PhotonPlayerController localPlayer;

    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine) {
            // this is the local player, can't be overwritten.
            localPlayer = this;
        }
        

    }

    // Update is called once per frame
    void Update()
    {

        if (!photonView.IsMine) {
            // this is not my player to move!
            return;
        }

        // moving the player according to Keyboard Input
        if (Input.GetKey(KeyCode.S)) {
            Vector3 pos = this.transform.position;
            pos += new Vector3(Time.deltaTime, 0f, 0f);
            transform.position = pos;
        }

        else if (Input.GetKey(KeyCode.D)) {
            Vector3 pos = this.transform.position;
            pos += new Vector3(0f, 0f, Time.deltaTime);
            transform.position = pos;
        }

        else if (Input.GetKey(KeyCode.W)) {
            Vector3 pos = this.transform.position;
            pos += new Vector3(-Time.deltaTime, 0f, 0f);
            transform.position = pos;
        }

         else if (Input.GetKey(KeyCode.A)) {
            Vector3 pos = this.transform.position;
            pos += new Vector3(0f, 0f, -Time.deltaTime);
            transform.position = pos;
        }
        
    }

}
