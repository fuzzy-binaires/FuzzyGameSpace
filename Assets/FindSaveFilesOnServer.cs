using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FindSaveFilesOnServer : MonoBehaviour
{
    TextMeshProUGUI textField;
    void Start()
    {
        textField = GetComponentInChildren<TextMeshProUGUI>();
        //textField.text = "Hello Darkness my old friend";
        textField.text = LogInController.credentialsServerPath();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
