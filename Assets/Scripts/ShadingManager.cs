using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public enum ObjectShadingType { SHADED, WIREFRAME, SHADED_WIREFRAME }

public class ShadingManager : MonoBehaviour
{
    private ObjectManager objectManager;


    private WireframeRendererv2 wireframeRenderer;


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
                wireframeRenderer.Shaded = true;
                wireframeRenderer.Wireframed = false;
            }
            else if (value == ObjectShadingType.WIREFRAME)
            {
                wireframeRenderer.Shaded = false;
                wireframeRenderer.Wireframed = true;
            }
            else if (value == ObjectShadingType.SHADED_WIREFRAME)
            {
                wireframeRenderer.Shaded = true;
                wireframeRenderer.Wireframed = true;
            }
            wireframeRenderer.ActualizeRenderer();
        }
    }
    public bool Cull { set { wireframeRenderer.ShowBackFaces = !value; wireframeRenderer.ActualizeRenderer(); } }
    public Color WireframeColor { set { wireframeRenderer.LineColor = value; wireframeRenderer.ActualizeRenderer(); } }
    public float WireframeSize { set { wireframeRenderer.LineSize = value; wireframeRenderer.ActualizeRenderer(); } }


    private void Awake()
    {
        objectManager = GetComponent<ObjectManager>();

        wireframeRenderer = objectManager.simulationObject.GetComponentInChildren<WireframeRendererv2>();
    }

    private void Start()
    {
        ActuShading();
    }

    // ### Functions ###

    private void ActuShading()
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
