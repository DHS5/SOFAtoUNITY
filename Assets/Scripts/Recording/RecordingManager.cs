using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecordingManager : MonoBehaviour
{
    private ObjectManager objectManager;
    private SettingsManager settingsManager;

    private RecorderFBX recorderFBX;


    [Header("Recording UI Components")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TextMeshProUGUI stopRecordText;
    [SerializeField] private Toggle resumeToggle;


    private Recorder activeRecorder;

    private bool recording;


    private void Awake()
    {
        objectManager = GetComponent<ObjectManager>();
        settingsManager = GetComponent<SettingsManager>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            StopRecord();
        }
    }

    // ### Functions ###

    /// <summary>
    /// Starts the scene recording
    /// </summary>
    public void StartRecord()
    {
        if (resumeToggle.isOn) settingsManager.ResumeSimulation();

        stopRecordText.gameObject.SetActive(true);
        
        recorderFBX = FindObjectOfType<RecorderFBX>();
        recorderFBX.StartRecording();

        activeRecorder = recorderFBX;
        recording = true;
    }

    /// <summary>
    /// Stops the scene recording
    /// </summary>
    private void StopRecord()
    {
        stopRecordText.gameObject.SetActive(false);

        if (recording)
            activeRecorder.StopRecording(inputField.text);
    }
}
