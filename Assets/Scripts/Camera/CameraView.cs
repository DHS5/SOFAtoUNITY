using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


/// <summary>
/// Controls the camera's type of view
/// </summary>
public class CameraView : MonoBehaviour
{
    private Camera mainCamera;


    [SerializeField] private Volume volume;
    private DepthOfField depthOfField;



    public bool DOFEnabled
    {
        set { depthOfField.active = value; }
    }
    public float FocusDistance
    {
        set { depthOfField.focusDistance.value = value; }
    }
    public float FocalLength
    {
        set { depthOfField.focalLength.value = value; }
    }
    public float Aperture
    {
        set { depthOfField.aperture.value = value; }
    }



    private void Awake()
    {
        mainCamera = GetComponent<Camera>();

        volume.profile.TryGet(out depthOfField);
    }
}
