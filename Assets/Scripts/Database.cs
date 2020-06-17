using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using System.IO;


public class Database : MonoBehaviour
{

    // MAKE IT A SINGLETON
    public static Database instance = null; 

    public TextAsset jsonPath;
    private static JSONNode db;


    void Awake()
    {
        // TO CORRECTLY INIT LE SINGLETON
        if (instance == null) {instance = this;}
        else if(instance != this){Destroy(gameObject);}


        db = JSON.Parse(jsonPath.ToString());
        //Debug.Log(db["pinData"[0]["name"]]);

    }

    public JSONNode getDB()
    {
        return db;
    }

    public static JSONNode getPinData(int pinId) {
        return db["pinData"][pinId];
    }

}
