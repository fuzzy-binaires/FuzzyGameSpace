using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextFaceCamera : MonoBehaviour
{
    void LateUpdate()
    {
        this.transform.LookAt(Camera.main.transform); // make the user name always face the camera
    }
}
