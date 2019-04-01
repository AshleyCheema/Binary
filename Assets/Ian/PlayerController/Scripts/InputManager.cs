/*
 * Author: Ian Hudson
 * Description: InputManager. This is used to allow for 
 * controller and mouse and keyboard support using Unity input system
 * Created: 04/02/2019
 * Edited By: Ian
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Player
{
    PlayerOne,
    PlayerTwo
}

public static class InputManager
{
    /// <summary>
    /// Return Horizontal value
    /// </summary>
    /// <returns></returns>
    public static float Horizontal(Player a_player)
    {
        float v = Input.GetAxis("P" + ((int)a_player + 1) + "Horizontal");
        //v = Mathf.Clamp(v, -1.0f, 1.0f);
        return v;
    }

    /// <summary>
    /// Return Vertical value
    /// </summary>
    /// <returns></returns>
    public static float Vertical(Player a_player)
    {
        float v = Input.GetAxis("P" + ((int)a_player + 1) + "Vertical");
        //v = Mathf.Clamp(v, -1.0f, 1.0f);
        return v;
    }

    /// <summary>
    /// Return both Horizontal & Vertical values
    /// </summary>
    /// <returns></returns>
    public static Vector3 Joystick(Player a_player, Vector3 a_pos)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * Mathf.Infinity, Color.magenta, 5f);

        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetKey(KeyCode.W))
            {
                Vector3 newDir = hit.point - a_pos;
                newDir.y = 0;
                
                return newDir.normalized;
            }
        }
        return Vector3.zero;
        // return new Vector3(Horizontal(a_player), 0, Vertical(a_player));

    }


    /// <summary>
    /// Return true or false for if the return key has been pressed
    /// </summary>
    /// <returns></returns>
    public static bool GetReturn()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            return true;
        }
        return false;
    }
}
