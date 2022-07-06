using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ObjectManager : MonoBehaviour
{
    private SettingsManager settingsManager;
    private UIManager uiManager;
    private AnimatorManager animatorManager;


    [Tooltip("Game Object of the simulation's object")]
    public GameObject simulationObject;

    public bool multipleObjects;

    [Tooltip("Fill only if the simulation object contains multiple child objects\n" +
        "Fill only with the child objects having a mesh/skinned mesh renderer")]
    public GameObject[] simulationObjects;



    // ### Built-in Functions ###

    private void Awake()
    {
        settingsManager = GetComponent<SettingsManager>();
        uiManager = GetComponent<UIManager>();
        animatorManager = GetComponent<AnimatorManager>();

        simulationObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }


    // ### Functions ###

}
