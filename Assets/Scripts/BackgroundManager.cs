using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    private ObjectManager objectManager;


    [Header("Object")]
    public GameObject background;
    [SerializeField] private GameObject cameraPivot;


    [Header("UI components")]
    [SerializeField] private FlexibleColorPicker backgroundFCP;




    private MeshRenderer meshRenderer;

    public bool BackgroundEnabled
    {
        set { background.SetActive(value); }
    }

    public float Distance
    {
        get { return background.transform.localPosition.z; }
        set { background.transform.localPosition = new Vector3(0, Altitude, value); }
    }
    public float Altitude
    {
        get { return background.transform.localPosition.y; }
        set { background.transform.localPosition = new Vector3(0, value, Distance); }
    }

    public Color Color
    {
        get { return meshRenderer.material.color; }
        set { meshRenderer.sharedMaterial.color = value; }
    }


    private void Awake()
    {
        objectManager = GetComponent<ObjectManager>();

        meshRenderer = background.GetComponent<MeshRenderer>();

        backgroundFCP.color = Color;
    }


    /// <summary>
    /// Keeps the background in front of the camera
    /// </summary>
    private void Update()
    {
        background.transform.rotation = Quaternion.Euler(0, cameraPivot.transform.rotation.eulerAngles.y + 180, 0);
    }
}
