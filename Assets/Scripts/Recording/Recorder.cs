using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;
using System.IO;

public abstract class Recorder : MonoBehaviour
{
    protected ObjectManager objectManager;
    protected LightManager lightManager;
    protected CameraController cameraController;
    

    protected AnimationClip clip;
    protected GameObjectRecorder m_Recorder;
    
    protected bool recording = false;
    protected string modelsPath;


    protected virtual void Start()
    {
        objectManager = FindObjectOfType<ObjectManager>();
        lightManager = FindObjectOfType<LightManager>();
        cameraController = FindObjectOfType<CameraController>();
    }





    // ### Functions ###

    /// <summary>
    /// Starts the recording of the scene clip
    /// </summary>
    public void StartRecording()
    {
        recording = true;

        // Create new clip
        clip = new AnimationClip();

        // Create recorder and record the script GameObject.
        m_Recorder = new GameObjectRecorder(gameObject);

        // Bind all the Transforms on the GameObject and all its children.
        Bind();
    }

    /// <summary>
    /// Stops the recording
    /// </summary>
    /// <param name="name"></param>
    public void StopRecording(string name)
    {
        if (name == "") name = "model" + Random.Range(0, 100000);

        NameVerif(ref name);

        recording = false;

        if (clip == null)
            return;

        if (m_Recorder.isRecording)
        {
            // Save the recorded session to the clip.
            m_Recorder.SaveToClip(clip);

            SaveModel(clip, name);
        }
    }

    /// <summary>
    /// Verify that the user' chosen nam is valid
    /// </summary>
    /// <param name="name">User chosen name</param>
    private void NameVerif(ref string name)
    {
        bool done = false;
        bool back;
        while (!done)
        {
            back = false;
            foreach (string folderName in Directory.GetFiles(modelsPath))
            {
                if (folderName.Remove(folderName.LastIndexOf('.'))[(folderName.LastIndexOf('/') + 1)..] == name)
                {
                    name += "1";
                    back = true;
                    break;
                }
            }

            if (!back)
                done = true;
        }
    }



    /// <summary>
    /// Binds the attributes to record in the scene clip
    /// </summary>
    protected abstract void Bind();

    /// <summary>
    /// Saves the model as a prefab and the clip
    /// </summary>
    /// <param name="clip">Scene clip to save</param>
    /// <param name="name">Name of the prefab</param>
    protected abstract void SaveModel(AnimationClip clip, string name);
}
