using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    private ObjectManager objectManager;
    private UIManager uiManager;


    public RuntimeAnimatorController controller;
    private Animator animator;


    private bool playing = false;
    private float animationSpeed = 1f;
    public float speedHighLimit;
    public float speedLowLimit;
    readonly float multiplier = 2f;


    // ### Properties ###
    public bool Play 
    { 
        set 
        {
            animator.SetBool("Motion Mode", !value);
            if (value && !playing)
                animator.Play("Animation", 0, MotionTime);

            playing = value;
        } 
    }

    public float Speed
    {
        get { return animationSpeed; }
        set
        {
            if (Mathf.Abs(value) <= speedHighLimit)
            {
                animationSpeed = value;
                animator.SetFloat("Speed", animationSpeed);
                uiManager.UpdateSpeedText(animationSpeed);
            }
        }
    }

    public float MotionTime
    {
        get
        {
            if (playing)
            {
                AnimatorStateInfo asi = animator.GetCurrentAnimatorStateInfo(0);
                float time = asi.normalizedTime >= 1 ? asi.normalizedTime % Mathf.FloorToInt(asi.normalizedTime) : 
                    asi.normalizedTime <= -1 ? 1 + (asi.normalizedTime % Mathf.CeilToInt(asi.normalizedTime)) :
                    asi.normalizedTime >= 0 ? asi.normalizedTime : 1 + asi.normalizedTime;
                return time;
            }
            else
            {
                return animator.GetFloat("MotionTime");
            }
        }
        set
        {
            animator.SetFloat("MotionTime", value);
        }
    }



    private void Awake()
    {
        uiManager = GetComponent<UIManager>();
        objectManager = GetComponent<ObjectManager>();
    }


    /// <summary>
    /// Binds the animation control keys
    /// </summary>
    private void Update()
    {
        if (objectManager.objectsReady)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Play = !playing;
                uiManager.SetPlayToggle(playing);
            }

            if (Input.GetKeyDown(KeyCode.RightArrow))
                SpeedUp();
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                SpeedDown();


            if (playing)
                uiManager.UpdateMotionTimeSlider(MotionTime);
        }
    }


    // ### Functions ###

    /// <summary>
    /// Actualize the animator according to the current object
    /// </summary>
    public void ActuAnimator()
    {
        animator = objectManager.currentObject.animator;

        Play = true;
        Speed = 1;
    }

    /// <summary>
    /// Increase the animation speed
    /// </summary>
    public void SpeedUp()
    {
        if (animationSpeed > 0)
            Speed *= multiplier;
        else if (animationSpeed == -speedLowLimit)
            Speed = speedLowLimit;
        else
            Speed /= multiplier;
    }
    /// <summary>
    /// Decrease the animation speed
    /// </summary>
    public void SpeedDown()
    {
        if (animationSpeed < 0)
            Speed *= multiplier;
        else if (animationSpeed == speedLowLimit)
            Speed = -speedLowLimit;
        else
            Speed /= multiplier;
    }
}
