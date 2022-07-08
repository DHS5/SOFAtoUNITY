using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    private ObjectManager objectManager;
    private CursorManager cursorManager;

    [Header("Screens")]
    [SerializeField] private GameObject settingsScreen;


    //[SerializeField] private


    public bool SimulationOn { get { return !settingsScreen.activeSelf; } }


    private void Awake()
    {
        objectManager = GetComponent<ObjectManager>();
        cursorManager = GetComponent<CursorManager>();
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (settingsScreen.activeSelf) ResumeSimulation();
            else StopSimulation();
        }
    }



    // ### Functions ###


    public void StopSimulation()
    {
        settingsScreen.SetActive(true);
        Time.timeScale = 0;
        cursorManager.SetCursor(false);
    }

    public void ResumeSimulation()
    {
        settingsScreen.SetActive(false);
        Time.timeScale = 1;
        cursorManager.SetCursor(true);
    }
}
