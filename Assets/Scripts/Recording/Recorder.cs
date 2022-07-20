using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;

public class Recorder : MonoBehaviour
{
    private ObjectManager objectManager;
    private LightManager lightManager;
    private CameraController cameraController;


    private AnimationClip clip;
    private GameObjectRecorder m_Recorder;

    private bool recording = false;


    readonly string modelsPath = "Assets/RecordedModels/";



    private void Start()
    {
        objectManager = FindObjectOfType<ObjectManager>();
        lightManager = FindObjectOfType<LightManager>();
        cameraController = FindObjectOfType<CameraController>();
    }


    // ### Functions ###

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

    private void Bind()
    {
        m_Recorder.BindComponentsOfType<Transform>(lightManager.lightContainer, true);
        m_Recorder.BindComponentsOfType<Light>(lightManager.lightContainer, true);
        m_Recorder.BindComponentsOfType<Transform>(cameraController.cameraPivot, true);
        m_Recorder.BindComponentsOfType<Transform>(objectManager.currentObject.gameObject, true);
        foreach (SubSimulationObject sub in objectManager.currentObject.children)
            m_Recorder.BindAll(sub.gameObject, false);
    }


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

    private void NameVerif(ref string name)
    {
        bool done = false;
        bool back;
        while (!done)
        {
            back = false;
            foreach (string folderName in Directory.GetFiles(modelsPath))
            {
                if (folderName.Remove(folderName.LastIndexOf('.'))[(folderName.LastIndexOf('/')+1)..] == name)
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


    void LateUpdate()
    {
        if (clip != null && recording)
        {
            // Take a snapshot and record all the bindings values for this frame.
            m_Recorder.TakeSnapshot(Time.deltaTime);
        }
    }


    private AnimationClip SaveClip(AnimationClip clip, string path)
    {
        clip.legacy = true;
        AssetDatabase.CreateAsset(clip, path + "Clip.anim");

        return clip;
    }

    private void SaveModel(AnimationClip clip, string name)
    {
        AssetDatabase.CreateFolder(modelsPath.TrimEnd('/'), name);
        string path = modelsPath + name + "/" + name;
        
        SavePrefab(path, SaveClip(clip, path));
    }

    private void SavePrefab(string path, AnimationClip clip)
    {
        List<GameObject> toDestroy = new List<GameObject>();

        // Instantiate new GameObject
        GameObject go = Instantiate(gameObject);

        // Destroys useless objects
        for (int i = 0; i < go.transform.childCount; i++)
        {
            if (go.transform.GetChild(i).GetComponent<SimulationObject>() != null && !go.transform.GetChild(i).gameObject.activeSelf)
            {
                toDestroy.Add(go.transform.GetChild(i).gameObject);
            }
        }
        foreach (GameObject g in toDestroy)
            DestroyImmediate(g);

        // Gets materials
        foreach (SubSimulationObject sub in go.GetComponent<SimulationObject>().children)
        {
            AssetDatabase.CreateAsset(sub.GetMaterial(), path + sub.gameObject.name + "Mat.mat");
        }

        // Apply animation
        go.AddComponent<Animation>().clip = clip;

        // Save prefab
        PrefabUtility.SaveAsPrefabAsset(go, path + ".prefab");

        Destroy(go);
    }
}
