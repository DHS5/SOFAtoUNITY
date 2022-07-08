using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class AssetManager : MonoBehaviour
{
    private AnimatorManager animatorManager;
    private ObjectManager objectManager;


    readonly string modelsPath = "Assets/Models/";
    readonly string animatedModelsPath = "Assets/AnimatedModels/";

    public ModelContainerSO modelContainer;

    public List<string> formats = new() { ".blend" };


    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        objectManager = GetComponent<ObjectManager>();
    }

    private void Start()
    {
        LoadModels();

        StoreModels();

        InstantiateModels();
    }

    // ### Tools ###

    private List<string> GetModelsNames()
    {
        List<string> fieldEntries = new();
        foreach (string fieldEntry in Directory.GetFiles(modelsPath))
        {
            if (formats.Contains(fieldEntry[(fieldEntry.LastIndexOf("."))..]))
            {
                int index = fieldEntry.LastIndexOf("/") + 1;
                fieldEntries.Add(fieldEntry[index..]);
            }
        }

        return fieldEntries;
    }
    private List<string> GetAnimatedModelsNames()
    {
        List<string> fieldEntries = new();
        foreach (string fieldEntry in Directory.GetFiles(animatedModelsPath))
        {
            int index = fieldEntry.LastIndexOf(".");
            int index2 = fieldEntry.LastIndexOf("/") + 1;
            fieldEntries.Add(fieldEntry.Remove(index)[index2..]);
        }

        return fieldEntries;
    }
    private List<string> GetPrefabsNames()
    {
        List<string> fieldEntries = new();
        foreach (GameObject g in modelContainer.modelPrefabs)
        {
            fieldEntries.Add(g.name);
        }

        return fieldEntries;
    }


    // ### First step : LOADING ###

    private void LoadModels()
    {
        List<string> modelsNames = GetModelsNames();
        List<string> animatedModelsNames = GetAnimatedModelsNames();

        foreach (string name in modelsNames)
        {
            if (!animatedModelsNames.Contains(name.Remove(name.LastIndexOf("."))))
            {
                Debug.Log("Load model : " + name);
                LoadModel(name);
            }
        }
    }

    private void LoadModel(string name)
    {
        // Gets the path
        var path = modelsPath + name;

        GameObject go = Instantiate(AssetDatabase.LoadMainAssetAtPath(path)) as GameObject;
        string gameObjectName = go.name.Remove(go.name.Length - 7);

        AssetDatabase.CreateFolder(animatedModelsPath.TrimEnd('/'), gameObjectName);
        string folderPath = animatedModelsPath + gameObjectName + "/";

        AnimationClip clip = null;
        var assetRepresentationsAtPath = AssetDatabase.LoadAllAssetRepresentationsAtPath(path);

        foreach (var assetRepresentation in assetRepresentationsAtPath)
        {
            var animationClip = assetRepresentation as AnimationClip;

            if (animationClip != null)
            {
                var settings = AnimationUtility.GetAnimationClipSettings(animationClip);
                settings.loopTime = true;
                AnimationUtility.SetAnimationClipSettings(animationClip, settings);

                clip = Instantiate(animationClip);

                AssetDatabase.CreateAsset(clip, folderPath + gameObjectName + "Clip.anim");
            }
        }

        if (clip != null)
        {
            AnimatorOverrideController AOC = new(animatorManager.controller);
            var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            foreach (var a in AOC.animationClips)
                anims.Add(new(a, clip));
            AOC.ApplyOverrides(anims);

            go.AddComponent<Animator>().runtimeAnimatorController = AOC;

            AssetDatabase.CreateAsset(AOC, folderPath + gameObjectName + "AOC.overrideController");
        }

        AddWireframeRenderer(go);

        AddSimulationObject(go);

        PrefabUtility.SaveAsPrefabAsset(go, folderPath + gameObjectName + ".prefab", out bool success);
        if (success)
            Destroy(go);
    }



    private void AddWireframeRenderer(GameObject go)
    {
        foreach (MeshRenderer mr in go.GetComponentsInChildren<MeshRenderer>())
        {
            WireframeRendererv2 wr = mr.gameObject.AddComponent<WireframeRendererv2>();
            wr.DestroyWireframeRenderer();
        }

        foreach (SkinnedMeshRenderer smr in go.GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            WireframeRendererv2 wr = smr.gameObject.AddComponent<WireframeRendererv2>();
            wr.DestroyWireframeRenderer();
        }
    }

    private void AddSimulationObject(GameObject go)
    {
        go.AddComponent<SimulationObject>();
    }




    // ### Second Step : STORING ###

    private void StoreModels()
    {
        List<string> prefabsNames = GetPrefabsNames();
        List<string> animatedModelsNames = GetAnimatedModelsNames();

        foreach (string name in animatedModelsNames)
        {
            if (!prefabsNames.Contains(name))
            {
                Debug.Log("Store model : " + name);
                StoreModel(name);
            }
        }
    }

    private void StoreModel(string name)
    {
        string path = animatedModelsPath + name + "/" + name + ".prefab";

        GameObject g = AssetDatabase.LoadMainAssetAtPath(path) as GameObject;

        modelContainer.modelPrefabs.Add(g);
    }


    // ### Third Step : INSTANTIATING ###

    private void InstantiateModels()
    {
        foreach (GameObject g in modelContainer.modelPrefabs)
        {
            Instantiate(g, objectManager.simulationObject.transform);
            objectManager.GetAllObjects();
        }
    }
}
