using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CameraZoom : MonoBehaviour
{
    // mouse values 

    private float minZoom = -5;
    private float maxZoom = 5;

    // camera values 
    private Vector3 camMaxZoomed = new Vector3(6f, 6f, 6f);
    private Vector3 camMinZoomed = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 camPos;

    // settings
    public bool orthographic = true;

    private float zoomSize = 5f;


    [BoxGroup("Settings"), Range(1.0f, 20.0f)] public float zoomSpeed = 20f;
    [BoxGroup("Settings"), Range(0.0f, 20.0f)] public float zoomSmoothing = 10f;

    [BoxGroup("Settings"), Range(-1f, 10f)] public float orthographicMinSize = 1f;
    [BoxGroup("Settings"), Range(-1f, 10f)] public float orthographicMaxSize = 10f;

    void Start()
    {
        camPos = this.transform.position; // read out camera position
        zoomSize = Camera.main.orthographicSize;
    }


    void Update()
    {
        float currentMouseZoom = Input.mouseScrollDelta.y; // read out mouse scroll data 
        currentMouseZoom = Mathf.Clamp(currentMouseZoom, minZoom, maxZoom); // clamp data 
        Debug.Log("Clamped current Zoom" + currentMouseZoom);


        if (!orthographic)
        {
            //camera for perspective view
            camPos += new Vector3(currentMouseZoom, currentMouseZoom, currentMouseZoom);
            camPos = new Vector3(Mathf.Clamp(camPos.x, camMinZoomed.x, camMaxZoomed.x), Mathf.Clamp(camPos.y, camMinZoomed.y, camMaxZoomed.y), Mathf.Clamp(camPos.z, camMinZoomed.z, camMaxZoomed.z));


            transform.position = Vector3.Lerp(transform.position, camPos, Time.deltaTime * 10f);
        }

        else
        {
            //camera for orthographic view
            zoomSize = Mathf.Clamp(zoomSize + currentMouseZoom * zoomSpeed, orthographicMinSize, orthographicMaxSize);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoomSize, Time.deltaTime * zoomSmoothing);
        }
    }
}
