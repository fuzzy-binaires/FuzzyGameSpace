using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class PhotonPlayerController : MonoBehaviour
{
    private PhotonView photonView;
    public static PhotonPlayerController localPlayer;

    public TextMeshPro userName;

    GameObject chatRoomCollider = null;

    Rigidbody playerRB;

    [SerializeField] MeshRenderer LED_Head_Capsule;
    [SerializeField] MeshRenderer LED_Neck;
    [SerializeField] MeshRenderer LED_LowerNeck;
    [SerializeField] MeshRenderer LED_Truck;

    Color playerColor = Color.red;

    private static int UIInFocusLastFrame;


    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView == null)
        {
            //Debug.LogError("PhotonPlayerController::Start photonView is null!!!");
        }

        if (photonView.IsMine)
        {
            // this is the local player, can't be overwritten.
            localPlayer = this;
        }



        playerRB = GetComponent<Rigidbody>();

        chatRoomCollider = GameObject.Find("chatArea");
        //Debug.Log("--------|| Found Pin Controller: " + pinController);


        // test code: Make it possible to test Main without login
        string nickname = "test_user";
        if (photonView.Owner == null || photonView.Owner.NickName == null)
        {
            //Debug.LogError("PhotonPlayerController::Start photonView.Owner is null!!!");
        }
        else
        {

            if (string.IsNullOrEmpty(photonView.Owner.NickName) || (photonView.Owner.NickName.Length - 3) <= 0)
            {
                //Debug.Log("Username " + photonView.Owner.NickName + " is too short");

            }
            else
            {
                nickname = photonView.Owner.NickName;
            }
        }
        // -- end test code snippet --


        string username = nickname.Substring(0, nickname.Length - 3);
        string usercolor = nickname.Substring(nickname.Length - 3, 3);

        //Debug.Log("User joined! " + "username: " + username + " usercolor: " + usercolor);
        userName.text = username;
        gameObject.name = "Player_" + username;

        playerColor = HSBColor.StringToHue(usercolor).ToColor();
        userName.color = playerColor;
        userName.UpdateFontAsset();

        setColors(playerColor);



        if (photonView.IsMine)
        {
            PinController.Instance.Setup(username);
        }
    }

    [PunRPC]
    public void LightOn()
    {
        LED_Head_Capsule.material.SetColor("_EmissionColor", Color.Lerp(Color.white, playerColor, 0.5f));
        AudioController.playPlayerLight();
    }

    [PunRPC] // make this function shared among networked participants
    public void LightOff()
    {
        LED_Head_Capsule.material.SetColor("_EmissionColor", Color.black);
    }


    void setColors(Color c)
    {
        LED_Head_Capsule.material.color = c;
        LED_Neck.material.color = c;
        LED_LowerNeck.material.color = c;
        LED_Truck.material.color = c;
    }
    void Update()
    {
        if (!photonView.IsMine)
        {
            // this is not my player to move!
            return;
        }

        //DISABLING DEFAULT MOTION AND LIGHTS UP WHEN INSIDE CHAT OR PIN CONNECTORS

        if (chatRoomCollider != null && chatRoomCollider.GetComponent<ToggleChatGui>().isChatGuiVisible)
        {
            //ignore player movement if chat window is active
            return;
        }



        // we prevent movement while a UI element is in focus – we can move only one frame after the UI usage
        if (Time.frameCount > UIInFocusLastFrame)
        {


            Vector2 moveInput = Vector2.zero;

            // moving the player according to Keyboard Input
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                moveInput += new Vector2(-1f, 0f);
            }

            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                moveInput += new Vector2(0f, 1f);
            }

            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                moveInput += new Vector2(1f, 0f);
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                moveInput += new Vector2(0f, -1f);
            }

            if (moveInput.magnitude > 0f)
            {
                Vector3 direction = Camera.main.transform.TransformDirection(new Vector3(moveInput.y, 0f, moveInput.x));
                direction = Vector3.ProjectOnPlane(direction, Vector3.up);

                Vector3 pos = transform.position;
                pos += direction.RemoveDiagonal().normalized * Time.deltaTime; // map the inputs onto the grid 

                transform.position = pos;

                constraintToBoard();

            }
            playerRB.velocity = Vector3.zero;
        }




        if (Input.GetKeyDown(KeyCode.Space))
        {
            // turn the light on and tell everyone on the network
            photonView.RPC("LightOn", RpcTarget.All);
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            // turn the light off and tell everyone on the network
            photonView.RPC("LightOff", RpcTarget.All);
        }
    }

    public static void SetUIInFocusLastFrame()
    {
        UIInFocusLastFrame = Time.frameCount; // set current frameCount as last frame when UI is in focus
    }


    [PunRPC] // share updates Pin info on the network at runtime
    public void UpdatedPinDescription(string pinName, string text)
    {
        //Debug.Log("Somebody changed a Pin: " + pinName + ", " + text);

        PinController.Instance.UpdatedPinDescription(pinName, text);
    }

    private void constraintToBoard()
    {
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -5, 5);
        pos.z = Mathf.Clamp(pos.z, -2.5f, 2.5f);

        transform.position = pos;

    }

}
