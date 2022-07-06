using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LightManager : MonoBehaviour
{
    private SettingsManager settingsManager;
    private ObjectManager objectManager;


    public GameObject lightContainer;
    private GameObject[] lightPivots;
    private Light[] lights;

    private GameObject currentPivot;
    private Light currentLight;



    [Header("Light UI components")]
    [SerializeField] private TMP_Dropdown lightDropdown;
    [SerializeField] private Slider xRotSlider;
    [SerializeField] private Slider yRotSlider;
    [SerializeField] private Slider distanceSlider;
    [SerializeField] private Slider altitudeSlider;
    [SerializeField] private Slider intensitySlider;
    [SerializeField] private Slider rangeSlider;
    [SerializeField] private FlexibleColorPicker lightFCP;
    [SerializeField] private Toggle lightToggle;


    [Header("Start variables")]
    public float startXRotation;
    public Color startColor;


    // ### Properties ###
    public int LightIndex
    {
        set { LightSelection(value); ActuLightUI(); }
    }

    public float XRotation
    {
        set { SetXRotation(value); }
    }
    public float YRotation
    {
        set { SetPivotRotation(value); }
    }
    public float Distance
    {
        set { SetDistance(value); }
    }
    public float Altitude
    {
        set { SetAltitude(value); }
    }

    public float LightIntensity
    {
        set { currentLight.intensity = value; }
    }
    public float LightRange
    {
        set { currentLight.range = value; }
    }

    public Color LightColor
    {
        set { currentLight.color = value; }
    }

    public bool LightEnable
    {
        set { currentLight.enabled = value; }
    }



    // ### Built-in Functions ###


    private void Awake()
    {
        settingsManager = GetComponent<SettingsManager>();
        objectManager = GetComponent<ObjectManager>();
    }

    private void Start()
    {
        InitLightList();
        InitLightPos();
        InitLightUI();
    }




    // ### Functions ###

    private void InitLightList()
    {
        int childCount = lightContainer.transform.childCount;
        lightPivots = new GameObject[childCount];
        lights = new Light[childCount];
        for (int i = 0; i < childCount; i++)
        {
            lightPivots[i] = lightContainer.transform.GetChild(i).gameObject;
            lights[i] = lightPivots[i].GetComponentInChildren<Light>();
        }

        LightIndex = 0;
    }

    private void InitLightPos()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lightPivots[i].transform.SetParent(objectManager.simulationObject.transform);
            lightPivots[i].transform.localPosition = new Vector3(0, 1, 0);
            lightPivots[i].transform.localRotation = Quaternion.identity;

            lights[i].transform.localPosition = new Vector3(0, 0, -10);
            lights[i].transform.localRotation = Quaternion.Euler(startXRotation, 0, 0);
            if (i > 0) lights[i].enabled = false;
        }
    }

    private void InitLightUI()
    {
        lightDropdown.options = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < lights.Length; i++)
        {
            lightDropdown.options.Add(new TMP_Dropdown.OptionData(lights[i].name));
        }
        ActuLightUI();
    }

    private void ActuLightUI()
    {
        xRotSlider.value = currentLight.transform.localEulerAngles.x;
        yRotSlider.value = currentPivot.transform.rotation.eulerAngles.y;
        distanceSlider.value = -currentLight.transform.localPosition.z;
        altitudeSlider.value = currentPivot.transform.localPosition.y;
        intensitySlider.value = currentLight.intensity;
        rangeSlider.value = currentLight.range;
        lightFCP.color = currentLight.color;
        lightToggle.isOn = currentLight.enabled;
    }


    private void LightSelection(int index)
    {
        currentLight = lights[index];
        currentPivot = lightPivots[index];
    }




    private void SetXRotation(float x)
    {
        currentLight.transform.localRotation = Quaternion.Euler(x, 0, 0);
    }

    private void SetPivotRotation(float y)
    {
        currentPivot.transform.rotation = Quaternion.Euler(0, y, 0);
    }

    private void SetDistance(float z)
    {
        currentLight.transform.localPosition = new Vector3(0, 0, -z);
    }

    private void SetAltitude(float y)
    {
        currentPivot.transform.localPosition = new Vector3(0, y, 0);
    }
}
