using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimulationObject : MonoBehaviour
{
    public SubSimulationObject[] children;

    public Animator animator;


    [HideInInspector] public int subObjectIndex;


    // ### Properties ###

    public int MaterialIndex
    {
        get { return children[subObjectIndex].materialIndex; }
        set { children[subObjectIndex].materialIndex = value; }
    }
    public Material CurrentMaterial
    {
        set { children[subObjectIndex].SetMaterial(value); }
    }
    public Material OriginalMaterial
    {
        get { return children[subObjectIndex].GetOriginalMaterial(); }
    }
    public float MaterialTiling
    {
        get { return children[subObjectIndex].Tiling; }
        set { children[subObjectIndex].Tiling = value; }
    }
    public float MaterialSmoothness
    {
        get { return children[subObjectIndex].Smoothness; }
        set { children[subObjectIndex].Smoothness = value; }
    }
    public float MaterialNormal
    {
        get { return children[subObjectIndex].Normal; }
        set { children[subObjectIndex].Normal = value; }
    }



    private void Awake()
    {
        children = GetComponentsInChildren<SubSimulationObject>();

        animator = GetComponent<Animator>();
    }
}
