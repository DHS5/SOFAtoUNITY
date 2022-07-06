using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    private ObjectManager objectManager;

    [Header("Animation buttons")]
    [SerializeField] private Toggle playToggle;
    [SerializeField] private Button speedUpButton;
    [SerializeField] private Button speedDownButton;
    [SerializeField] private TextMeshProUGUI animationSpeedText;
    [SerializeField] private Slider motionTimeSlider;


    private void Awake()
    {
        objectManager = GetComponent<ObjectManager>();
    }


    // ### Functions ###

    public void SetPlayToggle(bool state)
    {
        playToggle.isOn = state;
    }

    public void UpdateSpeedText(float speed)
    {
        animationSpeedText.text = "Speed : x" + speed;
    }

    public void UpdateMotionTimeSlider(float time)
    {
        motionTimeSlider.value = time;
    }
}
