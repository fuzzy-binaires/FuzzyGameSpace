using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Pin
{

    private string pinName;
    public string PinName {
        get { return pinName;}
        set { pinName = value;}
    }
    private string text;
    public string Text {
        get { return text;}
        set { text = value;}
    }

    public GameObject pinGO;
    public GameObject pinConnector;

    public Pin(string pinName, string text, GameObject pinGO, GameObject pinConnector) {
        this.pinName = pinName;
        this.text = text;
        this.pinGO = pinGO;
        this.pinConnector = pinConnector;
    }

      public bool isNullOrEmptyText()
    {

        if (text == "" || text == "empty") return true;

        return false;
    }

/*
    private void triggerPinConnectorAnimation()
    {
        Vector3 whereItStarts = transform.gameObject.transform.position + new Vector3(0,4f,0);
        pinConnector = PhotonNetwork.InstantiateSceneObject("Board/PinConnector", whereItStarts, Quaternion.identity);
        pinConnector.transform.parent = this.transform;
        pinConnectorMoving = true;

    }
*/


}
