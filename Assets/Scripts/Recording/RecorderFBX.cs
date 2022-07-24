using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.IO;

public class RecorderFBX : Recorder
{


    // ### Functions ###

    protected override void Start()
    {
        base.Start();

        modelsPath = "Assets/RecordedModels/";
    }

    protected override void Bind()
    {
        m_Recorder.BindComponentsOfType<Transform>(lightManager.lightContainer, false);
        m_Recorder.BindComponentsOfType<Transform>(cameraController.cameraPivot, true);
        //m_Recorder.BindComponentsOfType<Transform>(objectManager.currentObject.gameObject, true);
        m_Recorder.BindAll(objectManager.currentObject.gameObject, false);
        for (int i = 0; i < lightManager.lightContainer.transform.childCount; i++)
            if (lightManager.lightContainer.transform.GetChild(i).gameObject.activeSelf)
            {
                m_Recorder.BindComponentsOfType<Transform>(lightManager.lightContainer.transform.GetChild(i).gameObject, true);
                //m_Recorder.BindComponentsOfType<Light>(lightManager.lightContainer.transform.GetChild(i).gameObject, true);
                EditorCurveBinding b1 = EditorCurveBinding.FloatCurve(lightManager.lightContainer.name + "/" + lightManager.lightContainer.transform.GetChild(i).gameObject.name + "/" + lightManager.lightContainer.transform.GetChild(i).GetChild(0).gameObject.name, typeof(Light), "m_Intensity");
                EditorCurveBinding b2 = EditorCurveBinding.FloatCurve(lightManager.lightContainer.name + "/" + lightManager.lightContainer.transform.GetChild(i).gameObject.name + "/" + lightManager.lightContainer.transform.GetChild(i).GetChild(0).gameObject.name, typeof(Light), "m_Color.r");
                EditorCurveBinding b3 = EditorCurveBinding.FloatCurve(lightManager.lightContainer.name + "/" + lightManager.lightContainer.transform.GetChild(i).gameObject.name + "/" + lightManager.lightContainer.transform.GetChild(i).GetChild(0).gameObject.name, typeof(Light), "m_Color.g");
                EditorCurveBinding b4 = EditorCurveBinding.FloatCurve(lightManager.lightContainer.name + "/" + lightManager.lightContainer.transform.GetChild(i).gameObject.name + "/" + lightManager.lightContainer.transform.GetChild(i).GetChild(0).gameObject.name, typeof(Light), "m_Color.b");
                EditorCurveBinding b5 = EditorCurveBinding.FloatCurve(lightManager.lightContainer.name + "/" + lightManager.lightContainer.transform.GetChild(i).gameObject.name + "/" + lightManager.lightContainer.transform.GetChild(i).GetChild(0).gameObject.name, typeof(Light), "m_SpotAngle");
                m_Recorder.Bind(b1);
                m_Recorder.Bind(b2);
                m_Recorder.Bind(b3);
                m_Recorder.Bind(b4);
                m_Recorder.Bind(b5);
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

    /// <summary>
    /// Saves the clip in chosen file
    /// </summary>
    /// <param name="clip">Clip to save</param>
    /// <param name="path">File's path</param>
    /// <returns></returns>
    private AnimationClip SaveClip(AnimationClip clip, string path)
    {
        clip.legacy = true;
        clip.wrapMode = WrapMode.Loop;
        AssetDatabase.CreateAsset(clip, path + "Clip.anim");

        return clip;
    }

    protected override void SaveModel(AnimationClip clip, string name)
    {
        AssetDatabase.CreateFolder(modelsPath.TrimEnd('/'), name);
        string path = modelsPath + name + "/" + name;
        
        SavePrefab(path, SaveClip(clip, path));
    }

    /// <summary>
    /// Saves the prefab after destroying useless gameobjects in it
    /// </summary>
    /// <param name="path">File's path</param>
    /// <param name="clip">Clip to add to the prefab's animator</param>
    private void SavePrefab(string path, AnimationClip clip)
    {
        // Instantiate new GameObject
        GameObject go = Instantiate(gameObject);

        DestroyInactiveObjects(go);

        // Gets materials
        foreach (SubSimulationObject sub in go.GetComponentInChildren<SimulationObject>().children)
        {
            AssetDatabase.CreateAsset(sub.GetMaterial(), path + sub.gameObject.name + "Mat.mat");
        }

        // Apply animation
        go.AddComponent<Animation>().clip = clip;

        // Save prefab
        PrefabUtility.SaveAsPrefabAsset(go, path + ".prefab");

        Destroy(go);
    }

    /// <summary>
    /// Destroys useless gameobjects in the futur prefab gameobject
    /// </summary>
    /// <param name="p"></param>
    private void DestroyInactiveObjects(GameObject p)
    {
        Destroy(p.GetComponentInChildren<CameraController>());

        List<GameObject> toDestroy = new List<GameObject>();

        for (int i = 0; i < p.transform.childCount; i++)
        {
            if (!p.transform.GetChild(i).gameObject.activeSelf)
            {
                toDestroy.Add(p.transform.GetChild(i).gameObject);
            }
            else
            {
                DestroyInactiveObjects(p.transform.GetChild(i).gameObject);
            }
        }
        foreach (GameObject g in toDestroy)
            DestroyImmediate(g);
    }
}
