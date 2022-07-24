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



    [Tooltip("Whether the objects are ready to be used by other managers")]
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

    /// <summary>
    /// Gets an array of all the simulation objects present in the scene
    /// Actualize the UI according to the first object (only object kept active)
    /// </summary>
    public void GetAllObjects()
    {
        simulationObjects = simulationObject.GetComponentsInChildren<SimulationObject>();
        foreach (SimulationObject so in simulationObjects)
            so.gameObject.SetActive(false);

        InitObjectUI();

        SetCurrentObject(0);

        shadingManager.InitTextureUI();
    }

    /// <summary>
    /// Gets the main objects dropdown choices
    /// </summary>
    private void InitObjectUI()
    {
        mainObjectDropdown.options = new List<TMP_Dropdown.OptionData>();
        foreach (SimulationObject so in simulationObjects)
            mainObjectDropdown.options.Add(new TMP_Dropdown.OptionData(so.gameObject.name));
    }
    /// <summary>
    /// Gets the sub objects dropdown choices according to the active object
    /// </summary>
    private void ActuSubObjectUI()
    {
        subObjectsDropdown.options = new List<TMP_Dropdown.OptionData>();
        foreach (SubSimulationObject sub in currentObject.children)
            subObjectsDropdown.options.Add(new TMP_Dropdown.OptionData(sub.gameObject.name));

        subObjectsDropdown.value = 0;
        subObjectsDropdown.RefreshShownValue();

        subObjectEnableToggle.isOn = currentSubObject.gameObject.activeSelf;
    }

    /// <summary>
    /// Sets the current object by dropdown index
    /// </summary>
    /// <param name="index">Index of the object in the objects array</param>
    private void SetCurrentObject(int index)
    {
        if (currentObject != null) currentObject.gameObject.SetActive(false);
        currentObject = simulationObjects[index];
        currentObject.gameObject.SetActive(true);
        currentSubObject = currentObject.children[0];

        ObjectsReady();

        ActuSubObjectUI();
    }
    /// <summary>
    /// Signify to other managers that objects are ready to be used
    /// </summary>
    private void ObjectsReady()
    {
        animatorManager.ActuAnimator();
        shadingManager.ActuShadingUI();
        shadingManager.ActuTextureUI();
        objectsReady = true;
    }
}
