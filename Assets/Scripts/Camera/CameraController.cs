using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Handles the camera movements and rotations
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private SettingsManager settingsManager;
    private CursorManager cursorManager;
    private ObjectManager objectManager;


    enum CameraControlMode { FIXED, MOVING }

    public GameObject cameraPivot;

    public GameObject cameraHolder;

    private GameObject simulationObject;


    public float xMouseSensitivity;
    public float yMouseSensitivity;
    public float scrollSensitivity;
    public float yUpDownSensitivity;

    public float smoothRotation;
    public float maxAngle;
    [Range(0.01f, 0.5f)] public float snap;


    private bool xAxis;
    private bool yAxis;


    private CameraControlMode controlMode;

    private Quaternion cameraRotation;

    private bool locked;


    // ### Built-in functions ###

    private void Awake()
    {
        settingsManager = FindObjectOfType<SettingsManager>();
        cursorManager = FindObjectOfType<CursorManager>();
        objectManager = FindObjectOfType<ObjectManager>();

        simulationObject = objectManager.simulationObject;
    }

    private void Start()
    {
        InitCameraPos();

        controlMode = CameraControlMode.FIXED;
    }

    /// <summary>
    /// Binds the keys and mouse clicks used to move and rotate the camera
    /// </summary>
    private void Update()
    {
        if (Input.GetMouseButton(1))
            controlMode = CameraControlMode.MOVING;
        else controlMode = CameraControlMode.FIXED;

        // Resets
        if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.Y))
            ResetLookRotation();

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            ResetMoveRotation();

        if (Input.GetKeyDown(KeyCode.Delete))
            ResetAll();

        yAxis = !Input.GetKey(KeyCode.X);
        xAxis = !Input.GetKey(KeyCode.Y);

        Zoom();
        UpDownMove();

        cameraHolder.transform.LookAt(cameraPivot.transform.position);
    }

    /// <summary>
    /// Update in a smooth way the move and rotation of the camera
    /// </summary>
    private void LateUpdate()
    {
        if (controlMode == CameraControlMode.FIXED && cursorManager.locked) LookRotation();
        else if (controlMode == CameraControlMode.MOVING && cursorManager.locked) MoveRotation();
    }


    // ### Functions ###

    // # Initialization #

    /// <summary>
    /// Initialize the camera position
    /// </summary>
    private void InitCameraPos()
    {
        cameraPivot.transform.parent = simulationObject.transform;
        gameObject.transform.localPosition = Vector3.zero;

        ResetAll();
    }
    /// <summary>
    /// Resets all position and rotation of the camera
    /// </summary>
    private void ResetAll()
    {
        ResetPos();
        ResetZoom();
        ResetLookRotation();
        cameraPivot.transform.localEulerAngles = Vector3.zero;
    }


    // # Calcul #

    /// <summary>
    /// Makes the camera look around thanks to mouse movements
    /// </summary>
    private void LookRotation()
    {
        // Gets the mouse X and Y position and clamps it
        float yRotation = Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * xMouseSensitivity;
        float xRotation = Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * yMouseSensitivity;

        if (Mathf.Abs(xRotation) < snap || !xAxis) xRotation = 0;
        if (Mathf.Abs(yRotation) < snap || !yAxis) yRotation = 0;

        // Gets the new camera's rotation
        cameraRotation *= Quaternion.Euler(-xRotation, yRotation, 0f);
        cameraRotation = ClampLookRotation(cameraRotation);

        // Slerps to the new rotation
        gameObject.transform.localRotation = Quaternion.Slerp(gameObject.transform.localRotation, cameraRotation, smoothRotation * Time.deltaTime);
        // Fixes the z-rotation to 0
        gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, gameObject.transform.rotation.eulerAngles.y, 0);
    }

    /// <summary>
    /// Clamps the look rotation of the camera
    /// </summary>
    /// <param name="rot"></param>
    /// <returns></returns>
    private Quaternion ClampLookRotation(Quaternion rot)
    {
        // Normalize the original quaternion
        rot.y /= rot.w;
        rot.w = 1;

        // Clamps the X rotation in the angleMax range
        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(rot.x);
        angleX = Mathf.Clamp(angleX, -maxAngle, maxAngle);
        rot.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);
        
        // Clamps the Y rotation in the angleMax range
        float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan(rot.y);
        angleY = Mathf.Clamp(angleY, -maxAngle, maxAngle);
        rot.y = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleY);

        // Returns the usable quaternion
        return rot;
    }

    /// <summary>
    /// Resets the look rotation
    /// </summary>
    private void ResetLookRotation()
    {
        gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
        cameraRotation = transform.localRotation;
    }



    /// <summary>
    /// Makes the camera move around the object thanks to mouse movements
    /// </summary>
    private void MoveRotation()
    {
        // Gets the mouse X and Y position and clamps it
        float yRotation = Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * xMouseSensitivity;
        float xRotation = Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * yMouseSensitivity;

        xRotation = (Mathf.Abs(xRotation) > Mathf.Abs(yRotation) && xAxis) ? xRotation : 0;
        yRotation = (Mathf.Abs(yRotation) >= Mathf.Abs(xRotation) && yAxis) ? yRotation : 0;

        cameraPivot.transform.localRotation *= Quaternion.Euler(xRotation, yRotation, 0);
        cameraPivot.transform.localRotation = ClampMoveRotation(cameraPivot.transform.localRotation);

        cameraPivot.transform.rotation = Quaternion.Euler(cameraPivot.transform.rotation.eulerAngles.x, cameraPivot.transform.rotation.eulerAngles.y, 0);
    }

    /// <summary>
    /// Clamps the move rotation of the camera
    /// </summary>
    /// <param name="rot"></param>
    /// <returns></returns>
    private Quaternion ClampMoveRotation(Quaternion rot)
    {
        Vector3 eulerRot = rot.eulerAngles;
        eulerRot.x = eulerRot.x > 180 ? eulerRot.x - 360 : eulerRot.x;

        if (Mathf.Abs(eulerRot.x) > maxAngle)
        {
            eulerRot.x = maxAngle * (eulerRot.x / Mathf.Abs(eulerRot.x));
        }

        // Returns the usable quaternion
        return Quaternion.Euler(eulerRot);
    }
    /// <summary>
    /// Resets the move rotation
    /// </summary>
    private void ResetMoveRotation()
    { 
        cameraPivot.transform.localEulerAngles = new Vector3(0, cameraPivot.transform.localEulerAngles.y, 0);

        ResetLookRotation();
    }


    /// <summary>
    /// Makes the camera zoom in/out on the object thanks to mouse scroll wheel
    /// </summary>
    private void Zoom()
    {
        float mouseDelta = Input.GetAxis("Mouse ScrollWheel");

        if (mouseDelta != 0)
        {
            float dist = Vector3.Distance(cameraHolder.transform.position, cameraPivot.transform.position);

            cameraHolder.transform.localPosition += new Vector3(0, 0, mouseDelta * scrollSensitivity * dist * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(2))
            ResetZoom();
    }
    /// <summary>
    /// Resets the zoom of the camera
    /// </summary>
    private void ResetZoom()
    {
        cameraHolder.transform.localPosition = new Vector3(0, 0, -5);
    }

    /// <summary>
    /// Makes the camera go up/down thanks to keyboard keys
    /// </summary>
    private void UpDownMove()
    {
        float y = Input.GetAxis("Vertical");

        if (y != 0)
        {
            float dist = Vector3.Distance(cameraHolder.transform.position, cameraPivot.transform.position);

            cameraPivot.transform.localPosition += new Vector3(0, y * yUpDownSensitivity * dist * Time.deltaTime, 0);
        }
    }
    /// <summary>
    /// Resets the position of the camera
    /// </summary>
    private void ResetPos()
    {
        cameraPivot.transform.localPosition = Vector3.zero;
    }
}
