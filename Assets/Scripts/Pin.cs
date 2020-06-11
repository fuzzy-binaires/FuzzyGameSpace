using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pin : MonoBehaviour
{
    private int pinId;
    private bool isEmpty;
    private string description;
    private Collider thisCollider;

    public PinController pinController;


    private void Awake()
    {

    }


    void Start()
    {
        // Set pinId by GO name
        pinId = int.Parse(this.gameObject.name.TrimStart('p', 'i', 'n', '_'));
        description = "== " + pinId + " IS EMPTY" + " ==";

        thisCollider = GetComponent<Collider>();


    }


    void Update()
    {
        
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



}
