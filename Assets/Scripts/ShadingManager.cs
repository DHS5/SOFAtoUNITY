using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public enum ObjectShadingType { SHADED, WIREFRAME, CULLED_WIRERAME, SHADED_WIREFRAME }

public class ShadingManager : MonoBehaviour
{
    private ObjectManager objectManager;


    [Header("Shading UI components")]
    [SerializeField] private TMP_Dropdown shadingDropdownn;
    [SerializeField] private Slider wireframeThicknessSlider;
    [SerializeField] private Slider wireframeSmoothnessSlider;
    [SerializeField] private FlexibleColorPicker wireframeFCP;


    [Header("Textures")]
    [SerializeField] private TextureContainerSO textureContainer;

    [Header("Texture UI components")]
    [SerializeField] private TMP_Dropdown textureDropdown;
    [SerializeField] private Slider tilingSlider;
    [SerializeField] private Slider smoothnessSlider;
    [SerializeField] private Slider normalSlider;



    // ### Properties ###

    public int IntShadingType 
    { 
        get { return (int)ShadingType; }
        set { ShadingType = (ObjectShadingType)value; } 
    }
    public ObjectShadingType ShadingType
    {
        get { return GetShading(); }
        set { SetShading(value); }
    }
    public Color WireframeColor
    {
        get { return objectManager.currentSubObject.GetComponent<Renderer>().material.GetColor("_WireColor"); }
        set { objectManager.currentSubObject.GetComponent<Renderer>().material.SetColor("_WireColor", value); }
    }
    public float WireframeThickness
    {
        get { return objectManager.currentSubObject.GetComponent<Renderer>().material.GetFloat("_WireThickness"); }
        set { objectManager.currentSubObject.GetComponent<Renderer>().material.SetFloat("_WireThickness", value); }
    }
    public float WireframeSmoothness
    {
        get { return objectManager.currentSubObject.GetComponent<Renderer>().material.GetFloat("_WireSmoothness"); }
        set { objectManager.currentSubObject.GetComponent<Renderer>().material.SetFloat("_WireSmoothness", value); }
    }


    public int MaterialIndex
    {
        set 
        {
            if (value == 0) objectManager.currentObject.CurrentMaterial = objectManager.currentObject.OriginalMaterial;
            else
            {
                objectManager.currentObject.CurrentMaterial = textureContainer.materials[value - 1];
            }
            objectManager.currentObject.MaterialIndex = value;
            ActuTextureUI();
        }
    }
    public float MaterialTiling
    {
        get 
        {
            if (ShadingType == ObjectShadingType.SHADED)
                return objectManager.currentObject.MaterialTiling;
            else return 0;
        }
        set { objectManager.currentObject.MaterialTiling = value; }
    }
    public float MaterialSmoothness
    {
        get 
        {
            if (ShadingType == ObjectShadingType.SHADED)
                return objectManager.currentObject.MaterialSmoothness;
            else return 0;
        }
        set { objectManager.currentObject.MaterialSmoothness = value; }
    }
    public float MaterialNormal
    {
        get 
        {
            if (ShadingType == ObjectShadingType.SHADED)
                return objectManager.currentObject.MaterialNormal;
            else return 0;
        }
        set { objectManager.currentObject.MaterialNormal = value; }
    }


    // ### Built-in functions ###

    private void Awake()
    {
        objectManager = GetComponent<ObjectManager>();
    }



    // ### Functions ###

    /// <summary>
    /// Initialize the texture dropdown choices
    /// </summary>
    public void InitTextureUI()
    {
        textureDropdown.options = new();
        textureDropdown.options.Add(new TMP_Dropdown.OptionData("Default"));

        foreach (Material mat in textureContainer.materials)
        {
            textureDropdown.options.Add(new TMP_Dropdown.OptionData(mat.name));
        }
        // Then actualize the UI in consequence
        ActuTextureUI();
    }

    /// <summary>
    /// Actualize the shading UI components according to the current object
    /// </summary>
    public void ActuShadingUI()
    {
        shadingDropdownn.value = IntShadingType;
        if (IntShadingType != 0)
        {
            wireframeFCP.color = WireframeColor;
            wireframeThicknessSlider.value = WireframeThickness;
            wireframeSmoothnessSlider.value = WireframeSmoothness;
        }
    }

    /// <summary>
    /// Actualize the texture UI components according to the current object
    /// </summary>
    public void ActuTextureUI()
    {
        float tiling = MaterialTiling;
        float smooth = MaterialSmoothness;
        float normal = MaterialNormal;
        textureDropdown.value = objectManager.currentObject.MaterialIndex;
        textureDropdown.RefreshShownValue();

        MaterialTiling = tiling;
        MaterialSmoothness = smooth;
        MaterialNormal = normal;
        tilingSlider.value = MaterialTiling;
        smoothnessSlider.value = MaterialSmoothness;
        normalSlider.value = MaterialNormal;
    }


    /// <summary>
    /// Sets the shader of an object's material according to the current shading type
    /// </summary>
    /// <param name="type">Shading type</param>
    private void SetShading(ObjectShadingType type)
    {
        switch (type)
        {
            case ObjectShadingType.SHADED:
                objectManager.currentSubObject.GetComponent<Renderer>().material.shader = Shader.Find("Universal Render Pipeline/Lit");
                break;
            case ObjectShadingType.WIREFRAME:
                objectManager.currentSubObject.GetComponent<Renderer>().material.shader = Shader.Find("SuperSystems/Wireframe-Transparent");
                break;
            case ObjectShadingType.CULLED_WIRERAME:
                objectManager.currentSubObject.GetComponent<Renderer>().material.shader = Shader.Find("SuperSystems/Wireframe-Transparent-Culled");
                break;
            case ObjectShadingType.SHADED_WIREFRAME:
                objectManager.currentSubObject.GetComponent<Renderer>().material.shader = Shader.Find("SuperSystems/Wireframe-Shaded-Unlit");
                break;
        }     
    }

    /// <summary>
    /// Gets the current shading type of an object's material
    /// </summary>
    /// <returns></returns>
    private ObjectShadingType GetShading()
    {
        switch (objectManager.currentSubObject.GetComponent<Renderer>().material.shader.name)
        {
            case "SuperSystems/Wireframe-Transparent":
                return ObjectShadingType.WIREFRAME;
            case "SuperSystems/Wireframe-Transparent-Culled":
                return ObjectShadingType.CULLED_WIRERAME;
            case "SuperSystems/Wireframe-Shaded-Unlit":
                return ObjectShadingType.SHADED_WIREFRAME;
            default:
                return ObjectShadingType.SHADED;
        }
    }
}
