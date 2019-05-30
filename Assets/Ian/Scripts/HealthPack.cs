using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour
{
    public int ID;

    private void OnTriggerEnter(Collider other)
    {
        //check if the spy enters a health pack trigger
        if (other.tag == "Spy")
        {
            Debug.Log("Health pack picked up");
            //if this spy is hurt then change it's state to
            //normal
            if (other.transform.GetChild(0).gameObject.GetComponent<SpyController>().CurrentState == SpyState.Hurt)
            {
                other.transform.GetChild(0).gameObject.GetComponent<SpyController>().CurrentState = SpyState.Normal;

                gameObject.SetActive(false);

                if(ClientManager.Instance != null)
                {
                    Msg_ClientDestroyHealth cdh = new Msg_ClientDestroyHealth();
                    cdh.ConnectID = ClientManager.Instance.LocalPlayer.connectionId;
                    cdh.ID = ID;
                    ClientManager.Instance.client.Send(MSGTYPE.CLIENT_DESTROY_HEALTH, cdh);
                }
            }
        }
    }
}
