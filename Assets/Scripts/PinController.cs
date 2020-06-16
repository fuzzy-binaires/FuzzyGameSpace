using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class PinController : MonoBehaviour
{
    private int selectedPin = -1;
    public GameObject pinSelectedGizmo;

    void Start()
    {
        setupPins();

        //pinSelectedGizmo = GameObject.Find("pinSelectedGizmo");
        pinSelectedGizmo.transform.position = new Vector3 (0, -1, 0);
        StartCoroutine("rotatePinGizmoForeeeeever");
    }

    // Update is called once per frame
    void Update()
    {

        // ---- USER ACTS AGAINST A PIN - BEGIN
        if (Input.GetKeyUp(KeyCode.Return))
        {
            if(selectedPin != -1)
            {
                Pin pin = getPinById(selectedPin).GetComponent<Pin>();
                Debug.Log("Pin " + selectedPin + " isEmpty = " + pin.getIsEmpty());

                if (pin.getIsEmpty())
                {
                    // LAUNCH HTML INTERFACE FOR => ADDING A RESOURCE
                    Debug.Log("-|| USER WANTS TO ADD TO PIN " + selectedPin);
                    pin.setIsEmpty(false);

                }
                else
                {
                    // LAUNCH HTML INTERFACE FOR => READING A RESOURCE
                    Debug.Log("-|| USER READS PIN " + selectedPin);
                }


                Debug.Log("Pin " + selectedPin + " isEmpty = " + pin.getIsEmpty());
            }
        }
        // ---- USER ACTS AGAINST A PIN - END


    }

    private void setupPins()
    {


        foreach (Transform childPin in transform)
        {
            // ADD Pin CLASS TO PIN GOs
            childPin.gameObject.AddComponent<Pin>();

            // LINK THIS CLASS AS OBSERVER
            childPin.GetComponent<Pin>().setController(this.GetComponent<PinController>());

        }
    }

    public void initializePinState()
    {

        // I do this here because the pinsConnectors is a Photon shared object (everyone can see realtime if somebody post),
        // But it cannot be initialize before any Player enters a room. Thus I call it from PhotonBoot when PlayerEnters
        //Debug.Log(Database.getPinData(0));

        for (int i = 0; i < transform.childCount; i++)
        {

            JSONNode pinData = Database.getPinData(i);

            Pin pin = transform.GetChild(i).gameObject.GetComponent<Pin>();

            if (!pinData["description"].Equals("empty"))
            {
                //pin.setIsEmpty(false);
                StartCoroutine(setPinIsEmptyWithDelay(i / 20.0f, pin, false));

            }
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
        pinSelectedGizmo.transform.position = pinPos + new Vector3(0,0.5f,0);

        //Debug.Log("Updating Gizmo to Pin: " + selectedPin);

    }


    public void showPinData()
    {
        // ALWAYS ON SELECTED PIN

        string pinDescription = getPinByName(selectedPin).GetComponent<Pin>().getDescription();
        Debug.Log(pinDescription);
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


}
