using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;


public class PUN2_Chat : MonoBehaviourPun
{

    public TMP_InputField TMP_ChatInput;

    public TMP_Text TMP_ChatOutput;

    public ScrollRect ChatScrollView;

    bool isChatting = false;
    string chatInput = "";
    GameObject chatRoomCollider = null;

    [System.Serializable]
    public class ChatMessage
    {
        public string sender = "";
        public string message = "";
        public float timer = 0;
    }

    List<ChatMessage> chatMessages = new List<ChatMessage>();


    void DisplayMeshTextBox(string newText){

      // Clear Input Field
      TMP_ChatInput.text = string.Empty;
      TMP_ChatOutput.text = string.Empty;

      //Show messages
      for(int i = 0; i < chatMessages.Count; i++)
      {
          if(chatMessages[i].timer > 0 )
          {
            TMP_ChatOutput.text +=  chatMessages[i].message + "\n";

          }
      }

      var timeNow = System.DateTime.Now;
      TMP_ChatInput.ActivateInputField();


    }



    void Start()
    {
      Debug.Log("Chat start");
        //Initialize Photon View
        if(gameObject.GetComponent<PhotonView>() == null)
        {
            PhotonView photonView = gameObject.AddComponent<PhotonView>();
            photonView.ViewID = 1;
        }
        else
        {
            photonView.ViewID = 1;
        }

        //Let's chace a reference to the chatRoom objects
        chatRoomCollider = GameObject.Find("chatArea");

        TMP_ChatInput  = GameObject.Find("chatInput").GetComponent<TMP_InputField>();
        TMP_ChatOutput = GameObject.Find("chatOutput").GetComponent<TMP_Text>();
        ChatScrollView = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
        TMP_ChatInput.onSubmit.AddListener(DisplayMeshTextBox);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.T) && !isChatting)
        {
            isChatting = true;
            chatInput = "";
        }

        //Hide messages after timer is expired
        for (int i = 0; i < chatMessages.Count; i++)
        {
            if (chatMessages[i].timer > 0)
            {
                chatMessages[i].timer -= Time.deltaTime;
            }
        }
    }

    void OnGUI()
    {

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return)
        {
          var message = string.Format ("{0}", TMP_ChatInput.text);

          photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, message);
        }

    }

    [PunRPC]
    void SendChat(Player sender, string message)
    {
        ChatMessage m = new ChatMessage();
        m.sender = sender.NickName;
        var timeNow = System.DateTime.Now;

        Debug.Log(m.message);

        //Display player NickName otherwise just use the player id from PUN
        string playerName = (PhotonNetwork.LocalPlayer.NickName == "") ?
          PhotonNetwork.AuthValues.UserId : PhotonNetwork.LocalPlayer.NickName;

        m.message = "[<#FFFF80>" + timeNow.Hour.ToString("d2") +
        ":" + timeNow.Minute.ToString("d2") +
        ":" + timeNow.Second.ToString("d2") + "</color>] " +
        "[<#FFFF80>" + playerName +  "</color>] " +
        message + "\n";
        //
        m.timer = 15.0f;

        chatMessages.Insert(0, m);


        if(chatMessages.Count > 8)
        {
            chatMessages.RemoveAt(chatMessages.Count - 1);
        }
    }
}
