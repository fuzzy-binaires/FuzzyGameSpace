using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class CameraZoom : MonoBehaviour
{

    // References
    [BoxGroup("References")] public Transform cameraGroundPos;
    [BoxGroup("References")] public Transform boardCenterPosition;


    // mouse values 
    private float scrollClamp = 5f;

    // camera values 
    private Vector3 camMaxZoomed = new Vector3(6f, 6f, 6f);
    private Vector3 camMinZoomed = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 camPos;

    // settings
    public bool orthographic = true; // set camera as orthographic or perspective

    [BoxGroup("Settings"), Range(0.0f, 1.0f)] public float zoomSpeed = 0.1f;
    [BoxGroup("Settings"), Range(0.0f, 20.0f)] public float zoomSmoothing = 10f;
    [BoxGroup("Settings"), Range(-1f, 10f)] public float orthographicMinSize = 1f;
    [BoxGroup("Settings"), Range(-1f, 10f)] public float orthographicMaxSize = 10f;

    private float zoomLerp = 1f;
  


    void Start()
    {
        camPos = this.transform.position; // read out camera position
        Camera.main.orthographic = this.orthographic;
    }


    void Update()
    {
        // Player Setup and null checks
        Vector3 boardCenter = (boardCenterPosition != null) ? boardCenterPosition.position : Vector3.zero;
        Vector3 playerPos = (PhotonPlayerController.localPlayer != null) ? PhotonPlayerController.localPlayer.transform.position : boardCenter;
        
        // Zoom 
        float currentMouseZoom = Input.mouseScrollDelta.y; // read out mouse scroll data 
        currentMouseZoom = Mathf.Clamp(currentMouseZoom, -scrollClamp, scrollClamp); // clamp data 

        zoomLerp = Mathf.Clamp(zoomLerp + currentMouseZoom * zoomSpeed, 0f, 1f);
        Vector3 targetPos = Vector3.Lerp(playerPos, boardCenter, zoomLerp);
        cameraGroundPos.position = Vector3.Lerp(cameraGroundPos.position, targetPos, Time.deltaTime * zoomSmoothing);
        

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
            float zoomSize = Mathf.Lerp(orthographicMinSize, orthographicMaxSize, zoomLerp);
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoomSize, Time.deltaTime * zoomSmoothing);
        }
    }
}
