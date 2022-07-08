using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationObject : MonoBehaviour
{
    public GameObject[] children;
    public WireframeRendererv2[] childRenderers;

    public Animator animator;



    // ### Properties ###
    public bool Shaded
    {
        set
        {
            foreach (var child in childRenderers)
                child.Shaded = value;
            ActuRenderer();
        }
    }
    public bool Wireframed
    {
        set
        {
            foreach (var child in childRenderers)
                child.Wireframed = value;
            ActuRenderer();
        }
    }
    public bool Cull
    {
        set
        {
            foreach (var child in childRenderers)
                child.ShowBackFaces = !value;
            ActuRenderer();
        }
    }
    public Color LineColor
    {
        set
        {
            foreach (var child in childRenderers)
                child.LineColor = value;
            ActuRenderer();
        }
    }
    public float LineSize
    {
        set
        {
            foreach (var child in childRenderers)
                child.LineSize = value;
            ActuRenderer();
        }
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
