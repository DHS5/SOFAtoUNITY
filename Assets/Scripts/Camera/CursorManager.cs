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

    /// <summary>
    /// Binds the cursor management keys and mouse clicks
    /// </summary>
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

    /// <summary>
    /// Locks or unlocks the cursor given the state parameter
    /// </summary>
    /// <param name="state">If true : lock / If false : Unlock</param>
    public void SetCursor(bool state)
    {
        Cursor.lockState = state ? CursorLockMode.Locked : CursorLockMode.None;

        locked = state;
    }
}
