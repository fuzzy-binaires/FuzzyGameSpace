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

    private Vector3 lastMousePos;

    // camera values 
    private Vector3 camMaxZoomed = new Vector3(6f, 6f, 6f);
    private Vector3 camMinZoomed = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 camPos;

    // camera rotation values
    private float cameraRotOffset; // angle for camera
    [SerializeField] float rotYAngle = 40f;
    [SerializeField] float rotXAngle = 180f;
    [SerializeField] float rotSpeed = 3f;

    [SerializeField] Vector3 camOffset = new Vector3 (0f, 0.5f, -8f);

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
        cameraRotOffset = rotXAngle;
    }


    void LateUpdate()
    {

        updateCameraZoom();
        updateCameraRotation();
    }

    private void updateCameraZoom(){
        float currentMouseZoom = Input.mouseScrollDelta.y; // read out mouse scroll data 
        currentMouseZoom = Mathf.Clamp(currentMouseZoom, -scrollClamp, scrollClamp); // clamp data 

        zoomLerp = Mathf.Clamp(zoomLerp + currentMouseZoom * zoomSpeed, 0f, 1f);

        float zoomSize = Mathf.Lerp(orthographicMinSize, orthographicMaxSize, zoomLerp);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, zoomSize, Time.deltaTime * zoomSmoothing);
    }

    private void updateCameraRotation()
    {
        Vector3 playerPos = getPlayerPos();

        Vector3 mouseDelta = Input.mousePosition - lastMousePos;
        lastMousePos = Input.mousePosition;

        cameraRotOffset += mouseDelta.x * rotSpeed;

        Quaternion rotation = Quaternion.Euler(rotYAngle, cameraRotOffset, 0f);
        Camera.main.transform.rotation = rotation;

        Vector3 position = rotation * camOffset + playerPos;
        Camera.main.transform.position = position;
    }
 
    private Vector3 getPlayerPos() => (PhotonPlayerController.localPlayer != null) ? PhotonPlayerController.localPlayer.transform.position : getBoardCenter();
    private Vector3 getBoardCenter () => (boardCenterPosition != null) ? boardCenterPosition.position : Vector3.zero;

}
