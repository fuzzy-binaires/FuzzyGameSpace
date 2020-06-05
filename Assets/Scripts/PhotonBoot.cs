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

        PhotonNetwork.AuthValues = new AuthenticationValues(PlayerPrefs.GetString("name") + Random.Range(0, 10000000).ToString("D8"));
        Debug.Log("Player Name: " + PhotonNetwork.AuthValues.UserId);
        PhotonNetwork.LocalPlayer.NickName = PlayerPrefs.GetString("name");

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

        if (!isOffline)
        {
            PhotonNetwork.Instantiate("Player/PhotonPlayer", Vector3.zero, Quaternion.identity, 0);
        }
        else
        {
            PhotonNetwork.Instantiate("Player/PhotonAvatarOffline", Vector3.zero, Quaternion.identity, 0);
        }

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
