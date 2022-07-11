using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;


[System.Serializable]
public struct LightPreset
{
    public float xRot;
    public float yRot;
    public float dist;
    public float altitude;
    public float intensity;
    public float range;
    public Color color;
    public bool enabled;
};

public class LightManager : MonoBehaviour
{
    private SettingsManager settingsManager;
    private ObjectManager objectManager;

    [SerializeField] private LightPresetContainerSO lightPresetsContainer;


    public GameObject lightContainer;
    private GameObject[] lightPivots;
    private Light[] lights;

    private GameObject currentPivot;
    private Light currentLight;


    readonly string presetPath = "Assets/ScriptableObjects/Presets/Light/";



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

    [Header("Light Preset UI components")]
    [SerializeField] private TMP_Dropdown lightPresetDropdown;
    [SerializeField] private Button savePresetButton;
    [SerializeField] private TMP_InputField createPresetInputField;
    [SerializeField] private Button createPresetButton;


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
        get { return currentLight.transform.localEulerAngles.x; }
        set { SetXRotation(value); }
    }
    public float YRotation
    {
        get { return currentPivot.transform.rotation.eulerAngles.y; }
        set { SetPivotRotation(value); }
    }
    public float Distance
    {
        get { return -currentLight.transform.localPosition.z; }
        set { SetDistance(value); }
    }
    public float Altitude
    {
        get { return currentPivot.transform.localPosition.y; }
        set { SetAltitude(value); }
    }

    public float LightIntensity
    {
        get { return currentLight.intensity; }
        set { currentLight.intensity = value; }
    }
    public float LightRange
    {
        get { return currentLight.range; }
        set { currentLight.range = value; }
    }

    public Color LightColor
    {
        get { return currentLight.color; }
        set { currentLight.color = value; }
    }

    public bool LightEnable
    {
        get { return currentLight.enabled; }
        set { currentLight.enabled = value; }
    }


    // # Presets #
    public int PresetIndex
    {
        get { return lightPresetDropdown.value; }
        set { ApplyPresets(value); }
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
        PresetIndex = 0;
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

        lightPresetDropdown.options = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < lightPresetsContainer.presets.Count; i++)
        {
            lightPresetDropdown.options.Add(new TMP_Dropdown.OptionData(lightPresetsContainer.presets[i].name));
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





    // ### Light Preset ###

    private void SetLight(int index, LightPreset preset)
    {
        LightIndex = index;

        XRotation = preset.xRot;
        YRotation = preset.yRot;
        Distance = preset.dist;
        Altitude = preset.altitude;
        LightIntensity = preset.intensity;
        LightRange = preset.range;
        LightColor = preset.color;
        LightEnable = preset.enabled;
    }

    public void ApplyPresets(int index)
    {
        for (int i = 0; i < lights.Length; i++)
            SetLight(i, lightPresetsContainer.presets[index].presets[i]);

        LightIndex = 0;
    }


    public void CreatePresets()
    {
        string name = createPresetInputField.text;
        LightPresetSO presets = ScriptableObject.CreateInstance("LightPresetSO") as LightPresetSO;
        SavePresets(presets);
        AssetDatabase.CreateAsset(presets, presetPath + name + ".asset");
        lightPresetsContainer.presets.Add(presets);
        lightPresetDropdown.options.Add(new TMP_Dropdown.OptionData(name));
        lightPresetDropdown.value = lightPresetDropdown.options.Count - 1;
    }

    private void SavePreset(int index, ref LightPreset preset)
    {
        LightIndex = index;

        preset.xRot = XRotation;
        preset.yRot = YRotation;
        preset.dist = Distance;
        preset.altitude = Altitude;
        preset.intensity = LightIntensity;
        preset.range = LightRange;
        preset.color = LightColor;
        preset.enabled = LightEnable;
    }

    private void SavePresets(LightPresetSO presets)
    {
        int index = lightDropdown.value;

        for (int i = 0; i < lights.Length; i++)
            SavePreset(i, ref presets.presets[i]);

        LightIndex = index;
    }
    public void SavePresets()
    {
        SavePresets(lightPresetsContainer.presets[PresetIndex]);
    }
}
