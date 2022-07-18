using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class PreciseSlider : MonoBehaviour
{
    public Slider slider;
    public TMP_InputField inputField;

    public float Value
    {
        get { return SliderValue; }
        set
        {
            onValueChange.Invoke(value);
        }
    }

    public float SliderValue
    {
        get { return slider.value; }
        set { inputField.text = value.ToString(); Value = value; }
    }
    public string InputValue
    {
        set 
        {
            if (inputField.text != "")
                slider.value = float.Parse(inputField.text);
            else
                slider.value = slider.minValue;
        }
    }




    public FloatUpdateEvent onValueChange;

    [System.Serializable]
    public class FloatUpdateEvent : UnityEvent<float> { }


    private void Awake()
    {
        inputField.text = slider.value.ToString();
    }
}
