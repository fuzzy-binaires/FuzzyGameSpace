using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
public class PhotonPlayerController : MonoBehaviour
{
    private PhotonView photonView;
    private float lastPressedTime = 0.0f;
    public static PhotonPlayerController localPlayer;

    public TextMeshPro userName;

    GameObject chatRoomCollider = null;

    Rigidbody playerRB;

    private GUIStyle playerNameGuiStyle = new GUIStyle(); 

    [SerializeField] MeshRenderer LED_Head;
    [SerializeField] MeshRenderer LED_Neck;
    [SerializeField] MeshRenderer LED_LowerNeck;
    [SerializeField] MeshRenderer LED_Truck;



    void Start()
    {
        photonView = GetComponent<PhotonView>();

        playerRB = GetComponent<Rigidbody>();

        string nickname = photonView.Owner.NickName;
        string username = nickname.Substring(0, nickname.Length-3);
        string usercolor = nickname.Substring(nickname.Length-3, 3);
        Debug.Log("User joined! " + "username: " + username + " usercolor: " + usercolor);
        userName.text = username;
        Color c = HSBColor.StringToHue(usercolor).ToColor();
        userName.color = c;
        userName.UpdateFontAsset();
        //userName.color = PlayerPrefs.GetString(LogInController.userColor_Pointer);

        setColors(c);

        chatRoomCollider = GameObject.Find("chatArea");

        if (photonView.IsMine)
        {
            // this is the local player, can't be overwritten.
            localPlayer = this;
        } 

        playerNameGuiStyle.fontSize = 30;
    }


    void setColors(Color c) {
        LED_Head.material.color = c;
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

        if (chatRoomCollider.GetComponent<ToggleChatGui>().isChatGuiVisible){
            //ignore player movement if chat window is active
            return;
        }

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

        if (moveInput.magnitude > 0f) {
        Vector3 direction = Camera.main.transform.TransformDirection(new Vector3(moveInput.y, 0f, moveInput.x));
        direction = Vector3.ProjectOnPlane(direction, Vector3.up);

        Vector3 pos = this.transform.position;
        pos += direction.RemoveDiagonal().normalized * Time.deltaTime; // map the inputs onto the grid 

        transform.position = pos;
        }
        playerRB.velocity = Vector3.zero;
     

        if (Input.GetKey(KeyCode.U))
        {
            // if uuuuu fall "U" will catch uuuuu

            if (lastPressedTime + 0.2 < Time.time)
            {
                lastPressedTime = Time.time;
                transform.RotateAround(transform.position, transform.right, 90f);

            }


        }
       
       
    }

}
