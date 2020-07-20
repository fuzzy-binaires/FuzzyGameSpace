using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PinData
{
    [System.Serializable]
    public class PinDescription
    {
        public string pinName;
        public string text;

        // set up the PinDescription with two strings for pinName and text
        public PinDescription(string pinName, string text) {
            this.pinName = pinName;
            this.text = text;
        }

        public string Log()
        {
            return pinName + ", " + text;
        }
    }
    public List<PinDescription> pinDescriptions;

    public PinDescription GetPinDescription(string pinName)
    {
        for (int i=0; i < pinDescriptions.Count; i++) {
            if (pinDescriptions[i].pinName == pinName) {
                return pinDescriptions[i];
            }
        }

        return null;
    }

    // compare JSON data from server with existing local pins
    public bool containsPinDescription(string pinName) {
        //Debug.Log("Check if pin exists: " + pinName);
        for (int i=0; i < pinDescriptions.Count; i++) {

            if (pinDescriptions[i].pinName == pinName) {
                //Debug.Log("Did contain: " + pinName);
                return true;
            }
        }
        //Debug.Log("Did not contain: " + pinName);
        return false;
    }

    // add pin name and text from local Pin if it's not overwritten or not existant from the JSON
    public void addPinDescription(string pinName, string text) {
        if (pinDescriptions == null) {
            //Debug.LogError("pinDescriptions was null and will be instantiated!");
            pinDescriptions = new List<PinDescription>(); // if no descriptions exist make a new list (e. g. corrupted text file or offline)
        }

        if (pinName != "")
        {
            pinDescriptions.Add(new PinDescription(pinName, text)); // add the description and text to the Pin
        }
    }

    public bool isNullOrEmptyText(string pinName)
    {
        PinDescription des = GetPinDescription(pinName);

        if (des == null) return true;

        if (des.text == "" || des.text == "empty") return true;

        return false;
    }
}