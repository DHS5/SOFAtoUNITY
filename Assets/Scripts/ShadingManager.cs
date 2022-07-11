using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public enum ObjectShadingType { SHADED, WIREFRAME, SHADED_WIREFRAME }

public class ShadingManager : MonoBehaviour
{
    private ObjectManager objectManager;



    [Header("Shading")]
    [SerializeField] private Toggle shadedToggle;
    [SerializeField] private Toggle wireframeToggle;
    [SerializeField] private Toggle shadedWireframeToggle;
    [SerializeField] private GameObject cullToggleGroup;
    [SerializeField] private Toggle frontCullToggle;
    [SerializeField] private Toggle backCullToggle;
    [Header("Shading advanced")]
    [SerializeField] private FlexibleColorPicker wireframeFCP;
    [SerializeField] private Slider wireframeLineSizeSlider;



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


    //public int MaterialIndex
    //{
    //    set { objectManager.currentObject.CurrentMaterial = }
    //}


    // ### Built-in functions ###

    private void Awake()
    {
        objectManager = GetComponent<ObjectManager>();
    }


    // ### Functions ###

    public void ActuShadingUI()
    {
        Debug.Log(Cull);
        if (ShadingType == ObjectShadingType.SHADED) shadedToggle.isOn = true;
        else if (ShadingType == ObjectShadingType.WIREFRAME) wireframeToggle.isOn = true;
        else shadedWireframeToggle.isOn = true;

        if (Cull) frontCullToggle.isOn = true;
        else backCullToggle.isOn = true;

        wireframeFCP.color = WireframeColor;
        wireframeLineSizeSlider.value = WireframeSize;

        cullToggleGroup.SetActive(!shadedToggle.isOn);
    }
}
