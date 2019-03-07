/*
 * Author: Ian Hudson
 * Description: If an object can be interacted with then inherit from this
 * Created: 06/02/2019
 * Edited By: Ian
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Interactable_Net : NetworkBehaviour
{
    public virtual void Interact()
    {

    }
}
