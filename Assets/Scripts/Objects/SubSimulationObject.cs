using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubSimulationObject : MonoBehaviour
{

    [HideInInspector] public int materialIndex;


	private Renderer rend;
	private Material originalMat;


	// ### Properties ###

	public float Tiling
	{
		get { return rend.material.mainTextureScale.x; }
		set { rend.material.mainTextureScale = new Vector2(value, value); }
	}
	public float Smoothness
	{
		get { return rend.material.GetFloat("_Smoothness"); }
		set { rend.material.SetFloat("_Smoothness", value); }
	}
	public float Normal
	{
		get { return rend.material.GetFloat("_BumpScale"); }
		set { rend.material.SetFloat("_BumpScale", value); }
	}


    private void Awake()
    {
		rend = GetComponent<Renderer>();

		rend.material.SetFloat("_Cull", 0.0f);
	}


	public void SetMaterial(Material mat)
	{
		rend.material = mat;
	}
	public Material GetMaterial()
	{
		return rend.material;
	}
	public Material GetOriginalMaterial()
	{
		return originalMat;
	}
}
