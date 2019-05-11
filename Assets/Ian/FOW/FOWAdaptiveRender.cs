using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOWAdaptiveRender : MonoBehaviour
{
    [SerializeField]
    private float countdownMesh = 5.0f;

    List<CoutDownMeshObj> activeMeshes = new List<CoutDownMeshObj>();

    private void OnTriggerEnter(Collider other)
    {
        if (ClientManager.Instance?.LocalPlayer.playerTeam == LLAPI.Team.Merc)
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
                        other.transform.GetChild(0).transform.GetChild(5).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
                    }
                    else if (hit.distance <= 2.5f)
                    {
                        other.transform.GetChild(0).transform.GetChild(5).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
                    }
                }
                CheckForGameObject(other.transform.GetChild(5).gameObject);
            }
        }
        else if (ClientManager.Instance?.LocalPlayer.playerTeam == LLAPI.Team.Spy)
        {
            if (other.tag == "Merc")
            {
                other.transform.GetChild(0).transform.GetChild(7).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
                CheckForGameObject(other.transform.GetChild(0).transform.GetChild(7).gameObject);
            }
            if (other.tag == "Spy")
            {
                other.transform.GetChild(0).transform.GetChild(5).gameObject.GetComponent<SkinnedMeshRenderer>().enabled = true;
                CheckForGameObject(other.transform.GetChild(0).transform.GetChild(5).gameObject);
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
                CoutDownMeshObj c = new CoutDownMeshObj
                {
                    GameObject = other.transform.GetChild(0).transform.GetChild(5).gameObject,
                    Coroutine = StartCoroutine(CountdownMesh(other.transform.GetChild(0).transform.GetChild(5).gameObject))
                };
                activeMeshes.Add(c);
            }
        }
        else if (ClientManager.Instance?.LocalPlayer.playerTeam == LLAPI.Team.Spy)
        {
            if (other.tag == "Merc" && other.name == "Merc(Clone)")
            {
                //other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
                CoutDownMeshObj c = new CoutDownMeshObj
                {
                    GameObject = other.transform.GetChild(0).transform.GetChild(7).gameObject,
                    Coroutine = StartCoroutine(CountdownMesh(other.transform.GetChild(0).transform.GetChild(7).gameObject))
                };
                activeMeshes.Add(c);
            }
            if (other.tag == "Spy")
            {
                //other.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
                CoutDownMeshObj c = new CoutDownMeshObj
                {
                    GameObject = other.transform.GetChild(0).transform.GetChild(5).gameObject,
                    Coroutine = StartCoroutine(CountdownMesh(other.transform.GetChild(0).transform.GetChild(5).gameObject))
                };
                activeMeshes.Add(c);
            }
        }
    }

    private void CheckForGameObject(GameObject aGameObject)
    {
        for (int i = 0; i < activeMeshes.Count; i++)
        {
            if (activeMeshes[i].GameObject == aGameObject)
            {
                StopCoroutine(activeMeshes[i].Coroutine);
                activeMeshes.Remove(activeMeshes[i]);
            }
        }
    }

    IEnumerator CountdownMesh(GameObject mesh)
    {
        float step = countdownMesh;
        while (step > 0.0f)
        {
            step -= Time.deltaTime;
            yield return null;
        }
        //turn mesh off
        mesh.GetComponent<SkinnedMeshRenderer>().enabled = false;

        for (int i = 0; i < activeMeshes.Count; i++)
        {
            if (activeMeshes[i].GameObject == mesh)
            {
                activeMeshes.Remove(activeMeshes[i]);
            }
        }
    }
}

struct CoutDownMeshObj
{
    public GameObject GameObject;
    public Coroutine Coroutine;
}