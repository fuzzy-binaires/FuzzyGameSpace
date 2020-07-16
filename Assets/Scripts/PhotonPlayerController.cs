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

    [SerializeField] MeshRenderer LED_Head_Capsule;
    [SerializeField] MeshRenderer LED_Neck;
    [SerializeField] MeshRenderer LED_LowerNeck;
    [SerializeField] MeshRenderer LED_Truck;

    Color playerColor = Color.red;



    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView == null){
            Debug.LogError("PhotonPlayerController::Start photonView is null!!!");
        }

        

        playerRB = GetComponent<Rigidbody>();

        chatRoomCollider = GameObject.Find("chatArea");


        //Next is potentially unsafe code
        string nickname = "Anonymous_user111";
        if (photonView.Owner == null || photonView.Owner.NickName == null){
            Debug.LogError("PhotonPlayerController::Start photonView.Owner is null!!!");
        } else {
            
            if (string.IsNullOrEmpty(photonView.Owner.NickName) || (photonView.Owner.NickName.Length-3) <= 0){
                Debug.Log("Username " + photonView.Owner.NickName + " is too short");
             
            } else {
                nickname = photonView.Owner.NickName;
            }
        }
        
         
        string username = nickname.Substring(0, nickname.Length-3);
        string usercolor = nickname.Substring(nickname.Length-3, 3);

        Debug.Log("User joined! " + "username: " + username + " usercolor: " + usercolor);
        userName.text = username;
        gameObject.name = "Player_" + username;
        
        playerColor = HSBColor.StringToHue(usercolor).ToColor();
        userName.color = playerColor;
        userName.UpdateFontAsset();

        setColors(playerColor);

        

        if (photonView.IsMine)
        {
            // this is the local player, can't be overwritten.
            localPlayer = this;
        } 

        playerNameGuiStyle.fontSize = 30;
        LED_Head_Capsule.GetComponent<Light>().color = playerColor;

        
    }

    [PunRPC]
    public void LightOn()
    {
        LED_Head_Capsule.GetComponent<Light>().intensity = 5;
        
        if (!LED_Head_Capsule.material.IsKeywordEnabled("_EmissionColor")){
            Debug.Log("This is not a material property");
        } else {

        }
        Color colour = LED_Head_Capsule.material.GetColor("_EmissionColor");
        colour *= 4.0f; //  4X brighter
        LED_Head_Capsule.material.SetColor("_EmissionColor", colour);

        //LED_Head_Capsule.material.SetColor("_EmissionColor", playerColor); 
    }

    [PunRPC] // make this function shared among networked participants
    public void LightOff()
    {
        LED_Head_Capsule.GetComponent<Light>().intensity = 0;
        LED_Head_Capsule.material.SetColor("_EmissionColor", Color.black); 
    }


    void setColors(Color c) {
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


        if (Input.GetKeyDown(KeyCode.Space))
        {
               
            // turn the light on and tell everyone on the network
            photonView.RPC("LightOn", RpcTarget.All);
        } else if (Input.GetKeyUp(KeyCode.Space))
        {
            // turn the light off and tell everyone on the network
            photonView.RPC("LightOff", RpcTarget.All);
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
