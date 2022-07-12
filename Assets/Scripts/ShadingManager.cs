using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[System.Serializable]
public enum ObjectShadingType { SHADED, WIREFRAME, SHADED_WIREFRAME }

public class ShadingManager : MonoBehaviour
{
    private ObjectManager objectManager;


    [Header("Shading UI components")]
    [SerializeField] private Toggle shadedToggle;
    [SerializeField] private Toggle wireframeToggle;
    [SerializeField] private Toggle shadedWireframeToggle;
    [SerializeField] private GameObject cullToggleGroup;
    [SerializeField] private Toggle frontCullToggle;
    [SerializeField] private Toggle backCullToggle;
    [Header("Shading advanced")]
    [SerializeField] private FlexibleColorPicker wireframeFCP;
    [SerializeField] private Slider wireframeLineSizeSlider;


    [Header("Textures")]
    [SerializeField] private TextureContainerSO textureContainer;

    [Header("Texture UI components")]
    [SerializeField] private TMP_Dropdown textureDropdown;
    [SerializeField] private Slider tilingSlider;
    [SerializeField] private Slider smoothnessSlider;
    [SerializeField] private Slider normalSlider;



    // ### Properties ###

    public int IntShadingType { set { ShadingType = (ObjectShadingType)value; } }
    public ObjectShadingType ShadingType
    {
        get
        {
            if (objectManager.currentObject.Shaded && !objectManager.currentObject.Wireframed) return ObjectShadingType.SHADED;
            else if (!objectManager.currentObject.Shaded && objectManager.currentObject.Wireframed) return ObjectShadingType.WIREFRAME;
            else return ObjectShadingType.SHADED_WIREFRAME;
        }
        set
        {
            if (value == ObjectShadingType.SHADED)
            {
                objectManager.currentObject.Shaded = true;
                objectManager.currentObject.Wireframed = false;
            }
            else if (value == ObjectShadingType.WIREFRAME)
            {
                objectManager.currentObject.Shaded = false;
                objectManager.currentObject.Wireframed = true;
            }
            else if (value == ObjectShadingType.SHADED_WIREFRAME)
            {
                objectManager.currentObject.Shaded = true;
                objectManager.currentObject.Wireframed = true;
            }
        }
    }
    public bool Cull
    {
        get { return objectManager.currentObject.Cull; }
        set { objectManager.currentObject.Cull = value; }
    }
    public Color WireframeColor
    {
        get { return objectManager.currentObject.LineColor; }
        set { objectManager.currentObject.LineColor = value; } }
    public float WireframeSize
    {
        get { return objectManager.currentObject.LineSize; }
        set { objectManager.currentObject.LineSize = value; }
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
        get { return objectManager.currentObject.MaterialTiling; }
        set { objectManager.currentObject.MaterialTiling = value; }
    }
    public float MaterialSmoothness
    {
        get { return objectManager.currentObject.MaterialSmoothness; }
        set { objectManager.currentObject.MaterialSmoothness = value; }
    }
    public float MaterialNormal
    {
        get { return objectManager.currentObject.MaterialNormal; }
        set { objectManager.currentObject.MaterialNormal = value; }
    }


    // ### Built-in functions ###

    private void Awake()
    {
        objectManager = GetComponent<ObjectManager>();
    }



    // ### Functions ###

    public void InitTextureUI()
    {
        textureDropdown.options = new();
        textureDropdown.options.Add(new TMP_Dropdown.OptionData("Default"));

        foreach (Material mat in textureContainer.materials)
        {
            textureDropdown.options.Add(new TMP_Dropdown.OptionData(mat.name));
        }

        ActuTextureUI();
    }


    public void ActuShadingUI()
    {
        if (ShadingType == ObjectShadingType.SHADED) shadedToggle.isOn = true;
        else if (ShadingType == ObjectShadingType.WIREFRAME) wireframeToggle.isOn = true;
        else shadedWireframeToggle.isOn = true;

        if (Cull) frontCullToggle.isOn = true;
        else backCullToggle.isOn = true;

        wireframeFCP.color = WireframeColor;
        wireframeLineSizeSlider.value = WireframeSize;

        cullToggleGroup.SetActive(!shadedToggle.isOn);
    }

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
}
