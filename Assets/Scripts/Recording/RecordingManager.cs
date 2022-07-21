using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecordingManager : MonoBehaviour
{
    private RecorderFBX recorder;


    [Header("Recording UI Components")]
    [SerializeField] private TMP_InputField inputField;



    public void Record(bool start)
    {
        if (start)
        {
            recorder = FindObjectOfType<RecorderFBX>();
            recorder.StartRecording();
        }
        else recorder.StopRecording(inputField.text);
    }
}
