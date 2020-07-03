using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PinData
{
    [System.Serializable]
    public class PinDescriptions
    {
        public string text;
    }
    public PinDescriptions[] pinDescriptions;


    public void setup(int size)
    {
        pinDescriptions = new PinDescriptions[size];
        for (int i = 0; i < pinDescriptions.Length; i++)
        {
            pinDescriptions[i] = new PinDescriptions();
            //pinDescriptions[i].text = "empty";

        }
    }

    public void addDescriptionToSlot(int slot, string text)
    {
        pinDescriptions[slot].text = text;
    }
}


/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LogInData
{
    [System.Serializable]
    public class Credentials
    {
        public string email;
        public string password;
    }
    public Credentials[] credentials;

}
*/
