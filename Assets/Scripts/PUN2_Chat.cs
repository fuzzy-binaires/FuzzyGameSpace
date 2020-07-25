using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using System.Runtime.InteropServices;

public class PUN2_Chat : MonoBehaviourPun
{

   [DllImport("__Internal")]
    private static extern void OpenBrowserTabJS(string url);


    public TMP_InputField TMP_ChatInput;

    public TMP_InputField  InputFieldUrlLink;

    public TMP_Text TMP_ChatOutput;

    public ScrollRect ChatScrollView;

    private GameObject playlistCollider;

    private GameObject MusicPlayerCanvasGroup;


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
    private string playerName = "";

    //--------------------------------------------------------------------------
    void DisplayMeshTextBox(){

      // Clear Input Field
      //TMP_ChatInput.text = string.Empty;
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
      ChatScrollView.verticalNormalizedPosition = 0;

    }
    //--------------------------------------------------------------------------

    string FormatEmoji(string text){
      string formatedText = text;

        Debug.Log("FormatEmoji");

      var myDict = new Dictionary<string, string>
      {
          { ":D", "<sprite index=0>" },
          { ":)", "<sprite index=1>" },
          { ";)", "<sprite index=2>" },
          { ":|", "<sprite index=3>" },
          { ":o", "<sprite index=4>" },
          { ":O", "<sprite index=4>" },
          { ":(", "<sprite index=5>" },
      };

      foreach(var item in myDict)
      {
        if (formatedText.Contains(item.Key)){
            formatedText = formatedText.Replace(item.Key, item.Value );
        }
      }

      return formatedText;
    }
    //--------------------------------------------------------------------------
    void Start()
    {
      Debug.Log("Chat start");

      
      playerName = PlayerPrefs.GetString("userName");

      

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
        //InputFieldUrlLink = playlistCollider.GetComponent<CustomButtonList>().inputUrlTMP;
        //GameObject.Find("InputFieldUrlLink").GetComponent<TMP_InputField>();
        

        //Hide the chat UI initially
        playlistCollider = GameObject.Find("playlist_trigger");
        chatRoomCollider.GetComponent<ToggleChatGui>().ChatCanvasGroup.SetActive(false);
        playlistCollider.GetComponent<ToggleMusicGui>().MusicPlayerCanvasGroup.SetActive(false);

    }
    
    //--------------------------------------------------------------------------
    // Update is called once per frame
    void Update()
    {

        //Hide messages after timer is expired
        for (int i = 0; i < chatMessages.Count; i++)
        {
            if (chatMessages[i].timer > 0)
            {
                chatMessages[i].timer -= Time.deltaTime;
            }
        }
    }
    //--------------------------------------------------------------------------
  void OnGUI()
    {

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Return && 
        chatRoomCollider.GetComponent<ToggleChatGui>().ChatCanvasGroup.activeSelf)// && string.IsNullOrEmpty(TMP_ChatInput.text))
        {
          
          
          var message = string.Format ("{0}", FormatEmoji(TMP_ChatInput.text));

         var timeNow = System.DateTime.Now;
         string formated_message = "<size=30> <color=#A9A40C>" + timeNow.Hour.ToString("d2") +
        ":" + timeNow.Minute.ToString("d2") +
        ":" + timeNow.Second.ToString("d2") + "</color> " +
        " <#89A46A>" + playerName +  "</color> says: " +
        message;

          photonView.RPC("SendChat", RpcTarget.All, PhotonNetwork.LocalPlayer, formated_message);

          if (playlistCollider.GetComponent<ToggleMusicGui>().MusicPlayerCanvasGroup.activeSelf){
              photonView.RPC("PostLink", RpcTarget.All, PhotonNetwork.LocalPlayer, playlistCollider.GetComponent<CustomButtonList>().GetInputUrl());//InputFieldUrlLink.text);
              //InputFieldUrlLink.text = "";
          }

        }

        DisplayMeshTextBox();

    }
    //--------------------------------------------------------------------------
    [PunRPC]
    void PostLink(Player sender, string message)
    {
      
      if (message.StartsWith("http://") || message.StartsWith("https://")){
              playlistCollider.GetComponent<CustomButtonList>().CreateButton(message);
          }
          

    }
    //--------------------------------------------------------------------------
    [PunRPC]
    void SendChat(Player sender, string message)
    {
        ChatMessage m = new ChatMessage();
        m.sender = sender.NickName;
        Debug.Log(m.sender + "!!!!!!!");


        //Display player NickName otherwise just use the player id from PUN
        //string playerName = (PhotonNetwork.LocalPlayer.NickName == "") ?
        //  PhotonNetwork.AuthValues.UserId : PhotonNetwork.LocalPlayer.NickName;

        m.message = message;
        //
        m.timer = 50.0f;

        chatMessages.Add(m);


        if(chatMessages.Count > 8)
        {
            chatMessages.RemoveAt(0);
        }
    }
    //--------------------------------------------------------------------------
}
