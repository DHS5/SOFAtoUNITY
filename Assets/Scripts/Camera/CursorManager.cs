using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CursorManager : MonoBehaviour
{
    private SettingsManager settingsManager;



    [HideInInspector] public bool focused;
    [HideInInspector] public bool locked;



    private void Awake()
    {
        settingsManager = GetComponent<SettingsManager>();
    }

    private void Start()
    {
        SetCursor(true);
    }


    private void Update()
    {
        if (focused && Input.GetKeyDown(KeyCode.LeftControl) && settingsManager.SimulationOn)
        {
            SetCursor(!locked);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            focused = false;
            SetCursor(false);
        }
        else if (!focused && Input.GetMouseButtonDown(0))
        {
            focused = true;
            SetCursor(true);
        }
    }


    public void SetCursor(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;

        locked = state;
    }
}
