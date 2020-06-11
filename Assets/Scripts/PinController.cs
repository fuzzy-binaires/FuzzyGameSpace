using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinController : MonoBehaviour
{
    int selectedPin = -1;
    public GameObject pinSelectedGizmo;

    void Start()
    {
        setupPins();

        //pinSelectedGizmo = GameObject.Find("pinSelectedGizmo");
        pinSelectedGizmo.transform.position = new Vector3 (0, -1, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setupPins()
    {
        foreach(Transform childPin in transform)
        {
            // ADD Pin CLASS TO PIN GOs
            childPin.gameObject.AddComponent<Pin>();

            // LINK THIS CLASS AS OBSERVER
            childPin.GetComponent<Pin>().setController(this.GetComponent<PinController>());

        }
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
        pinSelectedGizmo.transform.position = pinPos + new Vector3(0,0.4f,0);

        Debug.Log("Updating Gizmo to Pin: " + selectedPin);

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
}
