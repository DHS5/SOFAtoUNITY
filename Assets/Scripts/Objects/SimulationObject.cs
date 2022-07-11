using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationObject : MonoBehaviour
{
    public GameObject[] children;
    public WireframeRendererv2[] childRenderers;

    public Animator animator;


    public int subObjectIndex;


    // ### Properties ###
    public bool Shaded
    {
        get { return childRenderers[subObjectIndex].Shaded; }
        set
        {
            childRenderers[subObjectIndex].Shaded = value;
            ActuRenderer();
        }
    }
    public bool Wireframed
    {
        get { return childRenderers[subObjectIndex].Wireframed; }
        set
        {
            childRenderers[subObjectIndex].Wireframed = value;
            ActuRenderer();
        }
    }
    public bool Cull
    {
        get { return !childRenderers[subObjectIndex].ShowBackFaces; }
        set
        {
            childRenderers[subObjectIndex].ShowBackFaces = !value;
            ActuRenderer();
        }
    }
    public Color LineColor
    {
        get { return childRenderers[subObjectIndex].LineColor; }
        set
        {
            childRenderers[subObjectIndex].LineColor = value;
            ActuRenderer();
        }
    }
    public float LineSize
    {
        get { return childRenderers[subObjectIndex].LineSize; }
        set
        {
            childRenderers[subObjectIndex].LineSize = value;
            ActuRenderer();
        }
    }


    public Material CurrentMaterial
    {
        set { childRenderers[subObjectIndex].SetMaterial(value); }
    }



    private void Awake()
    {
        int size = transform.GetComponentsInChildren<WireframeRendererv2>().Length;
        children = new GameObject[size];
        childRenderers = new WireframeRendererv2[size];

        int index = 0;
        foreach (WireframeRendererv2 wr in transform.GetComponentsInChildren<WireframeRendererv2>())
        {
            children[index] = wr.gameObject;
            childRenderers[index] = wr;
            index++;
        }

        animator = GetComponent<Animator>();
    }


    public void ActuRenderer()
    {
        foreach (var child in childRenderers)
            child.ActualizeRenderer();
    }
}
