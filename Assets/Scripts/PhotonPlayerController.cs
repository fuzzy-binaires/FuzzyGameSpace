using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonPlayerController : MonoBehaviour
{
    private PhotonView photonView;
    private float lastPressedTime = 0.0f;
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
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) {
            Vector3 pos = this.transform.position;
            pos += new Vector3(Time.deltaTime, 0f, 0f);
            transform.position = pos;
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
            Vector3 pos = this.transform.position;
            pos += new Vector3(0f, 0f, Time.deltaTime);
            transform.position = pos;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) {
            Vector3 pos = this.transform.position;
            pos += new Vector3(-Time.deltaTime, 0f, 0f);
            transform.position = pos;
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
            Vector3 pos = this.transform.position;
            pos += new Vector3(0f, 0f, -Time.deltaTime);
            transform.position = pos;
        }

        if (Input.GetKey(KeyCode.U)) {

          if (lastPressedTime + 0.2 < Time.time) {
              lastPressedTime = Time.time;
              transform.RotateAround(transform.position, transform.right, 90f);

          }


        }

    }

}
