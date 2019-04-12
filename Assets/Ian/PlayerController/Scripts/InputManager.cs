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
    public static float Horizontal(Player a_player = Player.PlayerOne)
    {
        float v = Input.GetAxis("P1Horizontal");//("P" + ((int)a_player + 1) + "Horizontal");
        //v = Mathf.Clamp(v, -1.0f, 1.0f);
        return v;
    }

    /// <summary>
    /// Return Vertical value
    /// </summary>
    /// <returns></returns>
    public static float Vertical(Player a_player = Player.PlayerOne)
    {
        float v = Input.GetAxis("P1Vertical");//("P" + ((int)a_player + 1) + "Vertical");
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

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            //Vector3 forward = hit.point - a_pos;
            //forward.y = 0;
            //forward.Normalize();
            //Vector3 right = Vector3.Cross(forward, Vector3.up);
            //right.Normalize();

            Vector3 returnV = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                //returnV += forward;
                returnV.z += 1f;
            }
            if (Input.GetKey(KeyCode.A))
            {
                //returnV += right;
                returnV.x -= 1f;
            }
            if (Input.GetKey(KeyCode.D))
            {
                //returnV += -right;
                returnV.x += 1f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                //returnV += -forward;
                returnV.z -= 1f;
            }
            return returnV.normalized;
        }
        return Vector3.zero;
        // return new Vector3(Horizontal(a_player), 0, Vertical(a_player));

    }

    public static Vector3 MovementRelativeToCamera(Vector3 a_input)
    {
        Camera cam = Camera.main;
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward = camForward.normalized;
        camRight = camRight.normalized;
        return (a_input.x * camRight + a_input.z * camForward);
    }

    //Camera cam = Camera.main;
    //Vector3 camForward = cam.transform.forward;
    //Vector3 camRight = cam.transform.right;
    //camForward.y = 0f;
    //    camRight.y = 0f;
    //    camForward = camForward.normalized;
    //    camRight = camRight.normalized;
    //return (a_pos.x* camRight + a_pos.z* camForward);

    public static Vector3 Joystick1(Player a_player, Vector3 a_pos)
    {
        return new Vector3(Horizontal(), 0, Vertical());
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
