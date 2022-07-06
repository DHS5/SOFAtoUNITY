using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controls the camera's type of view
/// </summary>
public class CameraView : MonoBehaviour
{
    /// <summary>
    /// Sets the camera wireframe view
    /// </summary>
    public bool Wireframe
    {
        set
        {
            GL.wireframe = value;

            Debug.Log(GL.wireframe);
        }
    }

    private void OnPreRender()
    {
        //Wireframe = true;
        GL.wireframe = true;
        Debug.Log(GL.wireframe);
    }

    private void OnPostRender()
    {
        //Wireframe = false;
        //GL.wireframe = false;
    }
}
