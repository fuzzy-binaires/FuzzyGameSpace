using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Pin : MonoBehaviour
{
    private int pinId;
    private bool isEmpty;
    private string description;
    private Collider thisCollider;

    private GameObject pinConnector;
    private bool pinConnectorMoving;

    public PinController pinController;


    private void Awake()
    {

    }


    void Start()
    {
        // Set pinId by GO name
        pinId = int.Parse(this.gameObject.name.TrimStart('p', 'i', 'n', '_'));
        description = "empty";

        thisCollider = GetComponent<Collider>();

        isEmpty = true;

        pinConnectorMoving = false;


    }


    void Update()
    {
        // ANIMATION OF PinConnector - BEGIN
        if (pinConnectorMoving)
        {
            pinConnector.transform.Translate(0, -0.02f, 0); // HOW ABOUT A NICE ACCELERATION CURVE HERE, HEIN..!!!!

            if(pinConnector.transform.position.y <= 0)
            {
                pinConnectorMoving = false;
            }
        }
        // ANIMATION OF PinConnector - END

    }


    public void setController(PinController control)
    {
        pinController = control;
    }

    private void notifySelection()
    {
        pinController.updatePinSelection(pinId);
    }

    private void notifySelectionClear()
    {
        pinController.updatePinSelection(-1);
    }

    public string getDescription()
    {
        return description;
    }

    public void setDescription(string text)
    {
        description = text;
    }

    public bool getIsEmpty()
    {
        return isEmpty;
    }

    public void setIsEmpty(bool state)
    {
        isEmpty = state;

        if (!isEmpty)// && !pinConnectorMoving)
        {
            triggerPinConnectorAnimation();
        }
    }

    // COLLISION --------------

    // private void OnCollisionEnter(Collision collision) => It was behaviorally incompatible with CollisionExit

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            notifySelection();
            //Debug.Log("PIN " + pinId + " HITTED BY PLAYER: " + collision.gameObject.name);

        }
    }


    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            notifySelectionClear();
            //Debug.Log("PIN " + pinId + " HITTED BY PLAYER: " + collision.gameObject.name);

        }
    }

    private void triggerPinConnectorAnimation()
    {
        Vector3 whereItStarts = transform.gameObject.transform.position + new Vector3(0,4f,0);
        pinConnector = PhotonNetwork.InstantiateSceneObject("Board/PinConnector", whereItStarts, Quaternion.identity);
        pinConnector.transform.parent = this.transform;
        pinConnectorMoving = true;

    }


}
