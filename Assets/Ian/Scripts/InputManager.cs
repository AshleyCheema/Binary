﻿/*
 * Author: Ian Hudson
 * Description: InputManager. This is used to allow for 
 * controller and mouse and keyboard support using Unity input system
 * Created: 04/02/2019
 * Edited By: Ian
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    /// <summary>
    /// Return Horizontal value
    /// </summary>
    /// <returns></returns>
    public static float Horizontal()
    {
        float v = Input.GetAxis("Horizontal");
        v = Mathf.Clamp(v, -1.0f, 1.0f);
        return v;
    }

    /// <summary>
    /// Return Vertical value
    /// </summary>
    /// <returns></returns>
    public static float Vertical()
    {
        float v = Input.GetAxis("Vertical");
        v = Mathf.Clamp(v, -1.0f, 1.0f);
        return v;
    }

    /// <summary>
    /// Return both Horizontal & Vertical values
    /// </summary>
    /// <returns></returns>
    public static Vector3 Joystick()
    {
        return new Vector3(Horizontal(), 0, Vertical());
    }
}
