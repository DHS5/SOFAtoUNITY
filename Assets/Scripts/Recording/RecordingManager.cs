using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RecordingManager : MonoBehaviour
{
    private Recorder recorder;


    [Header("Recording UI Components")]
    [SerializeField] private TMP_InputField inputField;



    public void Record(bool start)
    {
        if (start)
        {
            recorder = FindObjectOfType<Recorder>();
            recorder.StartRecording();
        }
        else recorder.StopRecording(inputField.text);
    }
}
