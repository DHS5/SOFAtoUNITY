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
    [SerializeField] private PreciseSlider backgroundSlider;




    private MeshRenderer meshRenderer;

    public bool BackgroundEnabled
    {
        set { background.SetActive(value); }
    }

    public float Distance
    {
        set 
        { 
            //background.transform.localScale = new Vector3(value, 20, value);
            background.transform.localPosition = new Vector3(0, -1, value);
        }
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
        SetSliderValues(-10, 10, 0);
    }

    private void Start()
    {
        //SetParent(objectManager.simulationObject.transform);
    }

    private void Update()
    {
        background.transform.rotation = Quaternion.Euler(0, cameraPivot.transform.rotation.eulerAngles.y + 180, 0);
    }

    public void SetParent(Transform transform)
    {
        background.transform.SetParent(transform);
        background.transform.SetPositionAndRotation(new Vector3(0, -1, 0), Quaternion.identity);
    }

    private void SetSliderValues(float min, float max, float start)
    {
        backgroundSlider.slider.minValue = min;
        backgroundSlider.slider.maxValue = max;
        backgroundSlider.slider.value = start;
    }
}
