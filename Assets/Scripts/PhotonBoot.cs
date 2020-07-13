using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using NaughtyAttributes;
using UnityEngine.SceneManagement;


public class PhotonBoot : MonoBehaviourPunCallbacks
{
    public static PhotonBoot Instance;
    public static PUN2_Chat chat;

    public PinController pinController;

    [SerializeField] Photon.Realtime.TypedLobby lobby = Photon.Realtime.TypedLobby.Default;
    [SerializeField] string roomName = "FuzzyGameSpace";
    [SerializeField] bool isOffline = false;
    //[SerializeField] string connectToScene = "";

    bool connectedToRoom = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Connect();
    }

    public void Connect()
    {
        PhotonNetwork.OfflineMode = isOffline;

        PhotonNetwork.ConnectUsingSettings();
    }

    public void Reconnect(bool isOffline)
    {
        PhotonNetwork.Disconnect();

        this.isOffline = isOffline;
        connectedToRoom = false;

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Photon: ConnectedToMaster -> " + PhotonNetwork.CloudRegion + ", " + PhotonNetwork.CurrentCluster);

        // get username and add a unique random ID 
        //Debug.Log("input conncetion values" + PlayerPrefs.GetString(LogInController.userName_Pointer) + PlayerPrefs.GetString(LogInController.userColor_Pointer));
        PhotonNetwork.AuthValues = new AuthenticationValues(PlayerPrefs.GetString(LogInController.userName_Pointer) + Random.Range(0, 10000000).ToString("D8"));
        // make the username = nickname visible to all players
        PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString(LogInController.userName_Pointer) + PlayerPrefs.GetString(LogInController.userColor_Pointer);
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Photon: OnJoinedLobby -> " + PhotonNetwork.CurrentLobby);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = 0;
        roomOptions.PublishUserId = true; // UserIDs visible to all other users
        PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Photon: OnCreatedRoom");

        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Photon: OnJoinedRoom -> " + PhotonNetwork.CurrentRoom);

        if (connectedToRoom)
        {
            Debug.Log("already connected?");
            return;
        }

        // settings for networked objects such as player, pins 
        GameObject player;

        if (!isOffline)
        {
            player = PhotonNetwork.Instantiate("Player/Player_LED", new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
        }
        else
        {
            player = PhotonNetwork.Instantiate("Player/Player_LED", new Vector3(0f, 0f, 0f), Quaternion.identity, 0);
        }

        player.gameObject.tag = "Player";

        pinController?.initializePinState();

        connectedToRoom = true;

    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("User joined!");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log("User left!");
    }


    public static string GetPhotonLog()
    {
        string photonLog = "ONLINE STATUS" + "\n" + "\n";
        photonLog += "CurrentCluster: " + PhotonNetwork.CurrentCluster + "\n";
        photonLog += "CloudRegion: " + PhotonNetwork.CloudRegion + "\n";
        photonLog += "CountOfPlayers: " + PhotonNetwork.CountOfPlayers + "\n";
        photonLog += "CountOfPlayersInRooms: " + PhotonNetwork.CountOfPlayersInRooms + "\n";
        photonLog += "CountOfPlayersOnMaster: " + PhotonNetwork.CountOfPlayersOnMaster + "\n";
        photonLog += "CountOfRooms: " + PhotonNetwork.CountOfRooms + "\n";
        photonLog += "CurrentRoom: " + PhotonNetwork.CurrentRoom + "\n";

        return photonLog;
    }
}
