using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using NaughtyAttributes;
using UnityEngine.Networking;
using System;
using Photon.Pun;

public class PinController : SingletonMonoBehaviour<PinController>
{
    private List<Pin> allPins; // hold references to ALL THE PINS
    private List<Pin> AllPins
    {
        get
        {
            if (allPins == null)
                allPins = new List<Pin>(); // if there is no list, make one

            return allPins;
        }
    } 
    Pin selectedPin = null;

    public Transform AllPinsContainer; // contain the references to all the pins in the editor

    public GameObject pinSelectedGizmo; // lightning marker

    private bool userTyping = false;

    public GameObject pinGui; // canvas to display text

    public TMP_InputField pinInputText; // input field for user text from within the canvas

    private bool chooseServerPath = true;
    // DATABASE STUFF
    [SerializeField] PinData pinDataEditor;
    [SerializeField] GameObject pinContentPrefab;
    //static string serverPath() => "http://134.122.74.56/space/appdata/pinDatabase.json";
    static string serverPath() => "http://134.122.74.56/borders_flask_server/pindata_to_server";
    static string uploadPath() => "http://134.122.74.56/borders_flask_server/pindata_to_server";



    #region Startup

    public void Setup()
    {
        // loads all local pins in the Scene
        loadPins();

        // loads the database from the server, so we can update the pins to their current state
        RequestJsonFromServer();
    }


    private void loadPins()
    {
        Transform[] localPinSlots = AllPinsContainer.GetComponentsInChildren<Transform>(); // get all the pins

        // sort the pins so that they are always in the same order no matter what the server or Unity does
        bubbleSort(localPinSlots);

        for (int i = 0; i < localPinSlots.Length; i++)
        {
            Pin pin = new Pin(localPinSlots[i].gameObject.name, "", localPinSlots[i].gameObject, null);
            AllPins.Add(pin);
        }
        /* for (int i = 0; i < AllPins.Length; i++) {
        Debug.Log("All Pins" + " " + i + " " + AllPins[i].gameObject.name);
        } */
    }

    private void bubbleSort(Transform[] arr)
    {
        Transform temp = null;

        for (int write = 0; write < arr.Length; write++)
        {
            for (int sort = 0; sort < arr.Length - 1; sort++)
            {
                if (string.Compare(arr[sort].gameObject.name, arr[sort + 1].gameObject.name) > 1)
                {
                    temp = arr[sort + 1];
                    arr[sort + 1] = arr[sort];
                    arr[sort] = temp;
                }
            }
        }
    }

    void RequestJsonFromServer()
    {
        StartCoroutine(DownloadString(serverPath(), OnReceivedPinData));
    }

    IEnumerator DownloadString(string uri, Action<string> callback)
    {

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // request and wait for page
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.LogError(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                callback(webRequest.downloadHandler.text);
            }
        }
    }

    void OnReceivedPinData(string json)
    {
        Debug.Log("--|| RECEIVING JSON");
        Debug.Log(json);

        pinDataEditor = JsonUtility.FromJson<PinData>(json);

        //Debug.Log("--|| AFTER SAVING IN PinData OBJECT");
        //Debug.Log(JsonUtility.ToJson(pinDataEditor));

        // sets the current state from the database to the local pins
        initializePinState(pinDataEditor);
    }

    public void initializePinState(PinData pinData)
    {
        // update the pins that are missing or not set in the downloaded data base
        for (int i = 0; i < AllPins.Count; i++)
        {
            if (AllPins == null || AllPins[i] == null) { continue; } // safety check in case something was deleted

            if (!pinData.containsPinDescription(AllPins[i].PinName))
            {
                //Debug.Log("Needs to add pin description: " + AllPins[i].PinName + " empty");
                pinData.addPinDescription(AllPins[i].PinName, "empty"); // add empty
            }
        }

        // setup prefabs for pins that are already with data
        float fancyDelay = 0f;
        const float delayPerPin = 0.033f;
        for (int i = 0; i < AllPins.Count; i++)
        {
            if (!pinData.isNullOrEmptyText(AllPins[i].PinName))
            {   
                //Debug.Log("Creating Startup Pin Content: " + AllPins[i].PinName);
                createStartupPinContent(AllPins[i], pinData.GetPinDescription(AllPins[i].PinName).text, fancyDelay);

                fancyDelay += delayPerPin;
            }
        }
    }

    public void createStartupPinContent(Pin pin, string text, float delay)
    {
        if (pin == null) return;

        GameObject connector = PhotonNetwork.Instantiate("Board/PinContent", pin.pinGO.transform.position, Quaternion.identity, 0);
        pin.pinConnector = connector; // put the pinConnector on the pin
        pin.Text = text; // put the text on the pin

        FlyDown flyDown = connector.GetComponentInChildren<FlyDown>();
        if (flyDown != null) flyDown.Setup(delay);

        pin.pinConnector = connector;
    }

    #endregion

    #region Runtime

    void Update()
    {
        if (pinInputText.isFocused) PhotonPlayerController.SetUIInFocusLastFrame();

        // figure out which pin is the closest in minimum range to the Player (without colliders)
        Pin closestPin = getClosestPin();

        if (closestPin != selectedPin)
        {
            selectedPin = closestPin;

            if (selectedPin != null) {
                //Debug.Log("Selected Pin: " + selectedPin.PinName);
                pinInputText.text = selectedPin.Text;
            }

            // mark nearest Pin with a gizmo
            updatePinSelection(selectedPin);

            // if there is a Pin nearby, show the text field
            pinGui.SetActive(selectedPin == null ? false : true); // set the text field active or not
        }

        if (Input.GetKeyUp(KeyCode.P))
        {
            //createRuntimePinContent(selectedPin);
        }

        // ---- USER ACTS AGAINST A PIN - BEGIN
        if (Input.GetKeyUp(KeyCode.Return))
        {
            if (selectedPin != null)
            {
                selectedPin.Text = pinInputText.text;
                savePinDataToFile();
            }

        }
    }

    private Pin getClosestPin()
    {
        if (PhotonPlayerController.localPlayer == null) return null;

        const float minDistance = 0.2f;
        Pin nearestPin = null;
        float closestDistance = Mathf.Infinity;
        for (int i = 0; i < AllPins.Count; i++)
        {
            float currentDistance = Vector3.Distance(PhotonPlayerController.localPlayer.gameObject.transform.position, AllPins[i].pinGO.transform.position);

            if (currentDistance > minDistance) continue;

            if (currentDistance < closestDistance)
            {
                closestDistance = currentDistance;
                nearestPin = AllPins[i];
            }
        }

        return nearestPin;
    }

    public void updatePinSelection(Pin pin)
    {
        const float gizmoHeight = 0.52f;

        if (pin == null || pin.pinGO.transform == null)
        {
            pinSelectedGizmo.transform.position = new Vector3(0f, 999f, 0f);
        }
        else
        {
            pinSelectedGizmo.transform.position = pin.pinGO.transform.position + new Vector3(0f, gizmoHeight, 0f);
        }
    }

    [Button] // FROM NAUGHTYATTRIBUTES
    private void savePinDataToFile()
    {
        for (int i = 0; i < AllPins.Count; i++)
        {
            if (pinDataEditor.containsPinDescription(AllPins[i].PinName))
            {
                pinDataEditor.GetPinDescription(AllPins[i].PinName).text = AllPins[i].Text;
            }
        }

        // TO FILE
        string json = JsonUtility.ToJson(pinDataEditor);
        Debug.Log("Updated local database: " + json);

        StartCoroutine(UploadData(json, OnUploadedDatabase));
    }

    public void OnUploadedDatabase() // upload new pin info to the database finished
    {
        if (selectedPin == null) return;

        PhotonPlayerController.localPlayer.gameObject.GetComponent<PhotonView>().RPC("UpdatedPinDescription", RpcTarget.All, selectedPin.PinName, selectedPin.Text);
    }

    // share updates Pin info on the network at runtime
    public void UpdatedPinDescription(string pinName, string text)
    {
        // local database
        PinData.PinDescription pinDes = pinDataEditor.GetPinDescription(pinName);
        if (pinDes != null)
        {
            pinDes.text = text;
        }

        // local pin GameObject
        for (int i = 0; i < AllPins.Count; i++)
        {
            if (AllPins[i].PinName == pinName && AllPins[i].pinConnector == null)
            {
                createStartupPinContent(AllPins[i], text, 0f);
            }
        }
    }

    IEnumerator UploadData(string jsonString, Action callback)
    {
        Debug.Log("----||| TRYING TO UPLOAD DATA ");
        Debug.Log(jsonString);

        WWWForm form = new WWWForm();
        form.AddField("fieldWithJsonString", jsonString);

        using (UnityWebRequest www = UnityWebRequest.Post(uploadPath(), form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                callback();
            }
        }
    }

    #endregion
}
