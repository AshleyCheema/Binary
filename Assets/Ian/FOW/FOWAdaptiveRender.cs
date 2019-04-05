using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOWAdaptiveRender : MonoBehaviour
{
    [SerializeField]
    private float countdownMesh = 5.0f;

    List<Coroutine> activeMeshes = new List<Coroutine>();

    private void OnTriggerEnter(Collider other)
    {
        if(ClientManager.Instance?.LocalPlayer.playerTeam == LLAPI.Team.Merc)
        {
            if (other.tag == "Spy")
            {
                Vector3 dir = (other.gameObject.transform.position - transform.position).normalized;
                float angle = Vector3.Angle(dir, transform.forward);

                RaycastHit hit;
                if (Physics.Raycast(gameObject.transform.position,
                                    dir,
                                    out hit))
                {
                    if (angle <= 120)
                    {
                        other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
                    }
                    else if (hit.distance <= 2.5f)
                    {
                        other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
                    }
                }
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
                //other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
                Coroutine c = StartCoroutine(CountdownMesh(other.transform.GetChild(0).gameObject));
                activeMeshes.Add(c);
            }
        }
        else if (ClientManager.Instance?.LocalPlayer.playerTeam == LLAPI.Team.Spy)
        {
            if (other.tag == "Merc")
            {
                //other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
                Coroutine c = StartCoroutine(CountdownMesh(other.transform.GetChild(0).gameObject));
                activeMeshes.Add(c);
            }
            if (other.tag == "Spy")
            {
                //other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
                Coroutine c = StartCoroutine(CountdownMesh(other.transform.GetChild(0).gameObject));
                activeMeshes.Add(c);
            }
        }
    }

    IEnumerator CountdownMesh(GameObject mesh)
    {
        float step = countdownMesh;
        while(step > 0.0f)
        {
            step -= Time.deltaTime;
            yield return null;
        }
        //turn mesh off
        mesh.GetComponent<MeshRenderer>().enabled = false;
    }
}
