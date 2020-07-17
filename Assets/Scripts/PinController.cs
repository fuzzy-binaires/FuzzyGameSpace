using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using TMPro;
using NaughtyAttributes;
using System.IO;
using UnityEngine.Networking;
using System;

public class PinController : MonoBehaviour
{
    private int selectedPin = -1;
    public GameObject pinSelectedGizmo;

    private bool userTyping = false;
    public GameObject pinConnector;

    public GameObject pinGui;
    //public TextMeshProUGUI pinInputText;
    public TMP_InputField pinInputText;

    private bool chooseServerPath = true;
    // DATABASE STUFF
    [SerializeField] PinData pinDataEditor;
    static string serverPath() => "http://134.122.74.56/space/appdata/pinDatabase.json";
    //static string credentialsLocalPath() => Application.dataPath + "/Resources/" + "pinDatabase.json";
    //static string credentialsServerPath() => serverPath + "pinDatabase.json"; // to put hard coded server path



    void Start()
    {
        setupPins();

        pinSelectedGizmo = GameObject.Find("pinSelectGizmo");
        pinSelectedGizmo.transform.position = new Vector3(0, -1, 0);
            StartCoroutine("rotatePinGizmoForeeeeever");

        pinGui = GameObject.Find("pinConnectorGUI");

        GameObject pinTextGO = pinGui.transform.Find("Canvas/pinInputField").gameObject;

        //Component[] list = aaaa.GetComponents(typeof(Component));
        //for (int i = 0; i < list.Length; i++)
        //{
        //    Debug.Log(list[i].name);
        //}

        pinInputText = pinTextGO.GetComponent<TMP_InputField>();
        //pinInputText.text = "[ENTER to Save]";

        pinGui.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        // ---- USER ACTS AGAINST A PIN - BEGIN
        if (Input.GetKeyUp(KeyCode.Return))
        {

            if (selectedPin != -1)
            {
                Pin pin = getPinById(selectedPin).GetComponent<Pin>();

                if (!userTyping)
                {
                    //Debug.Log("Pin " + selectedPin + " isEmpty = " + pin.getIsEmpty());

                    if (pin.getIsEmpty())
                    {
                        // LAUNCH HTML INTERFACE FOR => ADDING A RESOURCE
                        //Debug.Log("-|| USER WANTS TO ADD TO PIN " + selectedPin);
                        userTyping = true;
                        pinInputText.text = "[ENTER to Save]";

                        pinGui.SetActive(true);

                        // DISABLE WASD MOVEMENT


                    }
                    else
                    {
                        // LAUNCH HTML INTERFACE FOR => READING A RESOURCE
                        //Debug.Log("-|| USER READS PIN " + selectedPin);
                    }


                    //Debug.Log("Pin " + selectedPin + " isEmpty = " + pin.getIsEmpty());
                }
                else
                {
                    // USER IS TYPING, AND SINCE WE ARE INSIDE DE GetKeyUp.Return, it means she confirms input
                    pin.setIsEmpty(false);
                    userTyping = false;
                    pinGui.SetActive(false);

                    pin.setDescription(pinInputText.text);

                    savePinDataToFile();


                    //Debug.Log(getPinById(3).GetComponent<Pin>().getDescription());

                }
            }

        }



        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if (userTyping)
            {
                userTyping = false;
                pinGui.SetActive(false);

            }

        }
        // ---- USER ACTS AGAINST A PIN - END


    }

    private void setupPins()
    {

        pinDataEditor.setup(transform.childCount);

        foreach (Transform childPin in transform)
        {
            // ADD Pin CLASS TO PIN GOs
            childPin.gameObject.AddComponent<Pin>();

            // LINK THIS CLASS AS OBSERVER
            childPin.GetComponent<Pin>().setController(this.GetComponent<PinController>());

        }
    }

    public void initializePinState(PinData pinData)
    {

        // I do this here because the pinsConnectors is a Photon shared object (everyone can see realtime if somebody post),
        // But it cannot be initialize before any Player enters a room. Thus I call it from PhotonBoot when PlayerEnters
        //Debug.Log(Database.getPinData(0));

        //pinData = readPinDataFromFile();

        for (int i = 0; i < transform.childCount; i++)
        {

            //JSONNode pinData = PinDatabase.getPinData(i);
            string description = pinData.pinDescriptions[i].text;

            Pin pin = transform.GetChild(i).gameObject.GetComponent<Pin>();
            pin.setDescription(description);

            if (!pin.getDescription().Equals("empty"))
            {
                //pin.setIsEmpty(false);
                StartCoroutine(setPinIsEmptyWithDelay(i / 20.0f, pin, false));


            }


            //Debug.Log("Pin : " + i + !description.Equals("empty"));
        }
    }



    IEnumerator setPinIsEmptyWithDelay(float time, Pin pin, bool state)
    {
        yield return new WaitForSeconds(time);

        pin.setIsEmpty(state);


    }





    public void updatePinSelection(int pinId)
    {
        selectedPin = pinId;

        if (pinId != -1)
        {
            updatePinSelectionGizmo();
        }
        else
        {
            pinSelectedGizmo.transform.position = new Vector3(0, -1, 0);
        }
    }

    private void updatePinSelectionGizmo()
    {
        Vector3 pinPos = getPinByName(selectedPin).transform.position;
        pinSelectedGizmo.transform.position = pinPos + new Vector3(0, 0.5f, 0);

        //Debug.Log("Updating Gizmo to Pin: " + selectedPin);

    }


    public void showPinData()
    {
        // ALWAYS ON SELECTED PIN

        string pinDescription = getPinByName(selectedPin).GetComponent<Pin>().getDescription();
        //Debug.Log(pinDescription);
    }

    public GameObject getPinByName(int id)
    {
        return this.gameObject.GetFirstChild("pin_" + id);
    }

    public GameObject getPinById(int id)
    {
        return this.gameObject.transform.GetChild(id).gameObject;
    }


    private IEnumerator rotatePinGizmoForeeeeever()
    {

        GameObject thunderBolt = pinSelectedGizmo.transform.GetChild(0).gameObject;

        while (true)
        {
            thunderBolt.transform.Rotate(0, 5, 0);

            // WAIT SO MANY SECONDS TO RE-EXECUTE
            yield return new WaitForSeconds(0.01f);

        }
    }

    [Button] // FROM NAUGHTYATTRIBUTES
    private void savePinDataToFile()
    {

        // ASSEMBLE PIN DATA INTO ARRAY
        //string[]  escriptions = new string[transform.childCount];

        Debug.Log("--|| Starting Saving Process:");

        for (int i=0; i< transform.childCount; i++)
        { 
            pinDataEditor.pinDescriptions[i].text = getPinById(i).gameObject.GetComponent<Pin>().getDescription();
            Debug.Log("--|| Pin " + i + ": " + pinDataEditor.pinDescriptions[i].text);
        }


        // TO FILE
        string json = JsonUtility.ToJson(pinDataEditor);
        Debug.Log(json);

        StartCoroutine(UploadData(json));


        //string json = JsonUtility.ToJson(pinDataEditor);
        //StreamWriter writer = new StreamWriter(chooseServerPath ? serverPath() : credentialsLocalPath());
        //writer.WriteLine(json);
        //writer.Close();
        //AssetDatabase.ImportAsset(credentialsLocalPath());
    }

    //[Button]
    //private PinData readPinDataFromFile()
    //{
    //    StreamReader reader = new StreamReader(chooseServerPath ? serverPath() : credentialsLocalPath());
    //    string json = reader.ReadToEnd();
    //    reader.Close();
    //    //Debug.Log("json: " + json);
    //    return JsonUtility.FromJson<PinData>(json);

    //}


    public void initFromJson()
    {
        RequestJsonFromServer();

    }

    void RequestJsonFromServer()
    {
        StartCoroutine(DownloadString(serverPath(), OnReceivedPinData));
    }

    void OnReceivedPinData(string json)
    {
        Debug.Log("--|| RECIEVING RAW JSON");
        Debug.Log(json);

        pinDataEditor = JsonUtility.FromJson<PinData>(json);

        Debug.Log("--|| AFTER SAVING IN PinData OBJECT");
        Debug.Log( JsonUtility.ToJson(pinDataEditor));
        initializePinState(pinDataEditor);



        //LCDText.text = data.text;


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
                //PinData data = JsonUtility.FromJson<PinData>(webRequest.downloadHandler.text);
                pinDataEditor = JsonUtility.FromJson<PinData>(webRequest.downloadHandler.text);
                Debug.Log(JsonUtility.ToJson(pinDataEditor));


                for (int i=0; i < transform.childCount; i++)
                {
                    Pin pin = getPinById(i).GetComponent<Pin>();
                    pin.setDescription(pinDataEditor.pinDescriptions[i].text);

                }

            }
        }
    }

    IEnumerator UploadData(string jsonString)
    {
        Debug.Log("----||| TRYING TO UPLOAD DATA ");
        Debug.Log(jsonString);

        WWWForm form = new WWWForm();
        form.AddField("fieldWithJsonString", jsonString);

        using (UnityWebRequest www = UnityWebRequest.Post("http://134.122.74.56/borders_flask_server/pindata_to_server", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
            }
        }
    }

    public bool isUserTyping() { return userTyping; }

}
