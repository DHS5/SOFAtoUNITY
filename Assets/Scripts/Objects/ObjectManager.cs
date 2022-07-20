using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class ObjectManager : MonoBehaviour
{
    private SettingsManager settingsManager;
    private UIManager uiManager;
    private AnimatorManager animatorManager;
    private ShadingManager shadingManager;
    private BackgroundManager backgroundManager;


    [Tooltip("Game Object of the simulation's object container")]
    public GameObject simulationObject;

    [Tooltip("All simulation objects")]
    private SimulationObject[] simulationObjects;

    [HideInInspector] public SimulationObject currentObject;
    [HideInInspector] public SubSimulationObject currentSubObject;


    [Header("UI Components")]
    [SerializeField] private TMP_Dropdown mainObjectDropdown;
    [SerializeField] private TMP_Dropdown subObjectsDropdown;
    [SerializeField] private Toggle subObjectEnableToggle;




    [HideInInspector] public bool objectsReady = false;

    // ### Properties ###

    public int ObjectIndex { set { SetCurrentObject(value); } }

    public int SubObjectIndex 
    { 
        set 
        { 
            currentSubObject = currentObject.children[value];
            subObjectEnableToggle.isOn = currentSubObject.gameObject.activeSelf;
            currentObject.subObjectIndex = value;

            shadingManager.ActuShadingUI();
            shadingManager.ActuTextureUI();
        } 
    }
    public bool SetSubObject { set { currentSubObject.gameObject.SetActive(value); } }


    public float Altitude
    {
        set { backgroundManager.background.transform.position = new Vector3(0, -value, 0); }
    }
    

    // ### Built-in Functions ###

    private void Awake()
    {
        settingsManager = GetComponent<SettingsManager>();
        uiManager = GetComponent<UIManager>();
        animatorManager = GetComponent<AnimatorManager>();
        shadingManager = GetComponent<ShadingManager>();
        backgroundManager = GetComponent<BackgroundManager>();

        simulationObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }



    // ### Functions ###

    public void GetAllObjects()
    {
        simulationObjects = simulationObject.GetComponentsInChildren<SimulationObject>();
        foreach (SimulationObject so in simulationObjects)
            so.gameObject.SetActive(false);

        InitObjectUI();

        SetCurrentObject(0);

        shadingManager.InitTextureUI();
    }

    private void InitObjectUI()
    {
        mainObjectDropdown.options = new List<TMP_Dropdown.OptionData>();
        foreach (SimulationObject so in simulationObjects)
            mainObjectDropdown.options.Add(new TMP_Dropdown.OptionData(so.gameObject.name));
    }

    private void ActuSubObjectUI()
    {
        subObjectsDropdown.options = new List<TMP_Dropdown.OptionData>();
        foreach (SubSimulationObject sub in currentObject.children)
            subObjectsDropdown.options.Add(new TMP_Dropdown.OptionData(sub.gameObject.name));

        subObjectsDropdown.value = 0;
        subObjectsDropdown.RefreshShownValue();

        subObjectEnableToggle.isOn = currentSubObject.gameObject.activeSelf;
    }

    private void SetCurrentObject(int index)
    {
        if (currentObject != null) currentObject.gameObject.SetActive(false);
        currentObject = simulationObjects[index];
        currentObject.gameObject.SetActive(true);
        currentSubObject = currentObject.children[0];

        ObjectsReady();

        ActuSubObjectUI();
    }

    private void ObjectsReady()
    {
        animatorManager.ActuAnimator();
        shadingManager.ActuShadingUI();
        shadingManager.ActuTextureUI();
        objectsReady = true;
    }


    public SimulationObject[] GiveSimulationObjects()
    {
        return simulationObjects;
    }
}
