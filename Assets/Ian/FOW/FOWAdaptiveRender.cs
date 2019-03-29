using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOWAdaptiveRender : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(ClientManager.Instance?.LocalPlayer.playerTeam == LLAPI.Team.Merc)
        {
            if(other.tag == "Spy")
            {
                other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        else if(ClientManager.Instance?.LocalPlayer.playerTeam == LLAPI.Team.Spy)
        {
            if (other.tag == "Merc")
            {
                other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
            if (other.tag == "Spy")
            {
                other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (ClientManager.Instance?.LocalPlayer.playerTeam == LLAPI.Team.Merc)
        {
            if (other.tag == "Spy")
            {
                other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
        else if (ClientManager.Instance?.LocalPlayer.playerTeam == LLAPI.Team.Spy)
        {
            if (other.tag == "Merc")
            {
                other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            if (other.tag == "Spy")
            {
                other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
        }
    }
}
