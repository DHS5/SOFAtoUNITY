using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;


public class ObjectManager : MonoBehaviour
{
    private SettingsManager settingsManager;
    private UIManager uiManager;
    private AnimatorManager animatorManager;


    [Tooltip("Game Object of the simulation's object")]
    public GameObject simulationObject;

    [Tooltip("All childs of the simulationObject containing a mesh/skinned mesh renderer")]
    private GameObject[] objectChildren;



    

    // ### Built-in Functions ###

    private void Awake()
    {
        settingsManager = GetComponent<SettingsManager>();
        uiManager = GetComponent<UIManager>();
        animatorManager = GetComponent<AnimatorManager>();

        simulationObject.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    private void Start()
    {
        
    }

    // ### Functions ###


    
}
