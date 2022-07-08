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


    [Tooltip("Game Object of the simulation's object")]
    public GameObject simulationObject;

    [Tooltip("All childs of the simulationObject containing a mesh/skinned mesh renderer")]
    private SimulationObject[] simulationObjects;

    [HideInInspector] public SimulationObject currentObject;
    [HideInInspector] public GameObject currentSubObject;


    [Header("UI Components")]
    [SerializeField] private TMP_Dropdown mainObjectDropdown;
    [SerializeField] private TMP_Dropdown subObjectsDropdown;
    [SerializeField] private Toggle subObjectEnableToggle;




    [HideInInspector] public bool objectsReady = false;

    // ### Properties ###

    public int ObjectIndex { set { SetCurrentObject(value); } }

    public int SubObjectIndex { set { currentSubObject = currentObject.children[value]; subObjectEnableToggle.isOn = currentSubObject.activeSelf; } }
    public bool SetSubObject { set { currentSubObject.SetActive(value); } }
    

    // ### Built-in Functions ###

    private void Awake()
    {
        settingsManager = GetComponent<SettingsManager>();
        uiManager = GetComponent<UIManager>();
        animatorManager = GetComponent<AnimatorManager>();
        shadingManager = GetComponent<ShadingManager>();

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
        foreach (GameObject g in currentObject.children)
            subObjectsDropdown.options.Add(new TMP_Dropdown.OptionData(g.name));

        subObjectsDropdown.value = 0;
        subObjectsDropdown.RefreshShownValue();

        subObjectEnableToggle.isOn = currentSubObject.activeSelf;
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
        shadingManager.ActuShading();
        objectsReady = true;
    }
}
