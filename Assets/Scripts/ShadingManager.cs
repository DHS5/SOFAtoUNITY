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
    public bool Cull { set { objectManager.currentObject.Cull = value; } }
    public Color WireframeColor { set { objectManager.currentObject.LineColor = value; } }
    public float WireframeSize { set { objectManager.currentObject.LineSize = value; } }


    private void Awake()
    {
        objectManager = GetComponent<ObjectManager>();
    }


    // ### Functions ###

    public void ActuShading()
    {
        if (shadedToggle.isOn) ShadingType = ObjectShadingType.SHADED;
        else if (wireframeToggle.isOn) ShadingType = ObjectShadingType.WIREFRAME;
        else if (shadedWireframeToggle.isOn) ShadingType = ObjectShadingType.SHADED_WIREFRAME;

        Cull = frontCullToggle.isOn;

        WireframeColor = wireframeFCP.color;
        WireframeSize = wireframeLineSizeSlider.value;

        cullToggleGroup.SetActive(!shadedToggle.isOn);
    }
}
