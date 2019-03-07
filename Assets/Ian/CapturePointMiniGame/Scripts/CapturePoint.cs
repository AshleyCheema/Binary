/*
 * Author: Ian Hudson
 * Description: HLAPI - This is a capture point object. This object keeps track as to if it has 
 * been captured.
 * Created: 19/02/2019
 * Edited By: Ian
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class CapturePoint : Interactable_Net
{
    [SerializeField]
    private GameObject percentageUI;

    [SerializeField]
    private CapturePointMiniGame miniGame;

    [SerializeField]
    [SyncVar]
    private bool spyIsCapturing = false;

    [SerializeField]
    private bool showMiniGame = false;

    [SerializeField]
    private float capturePercentage = 0.0f;

    [SerializeField]
    [SyncVar]
    private float captureMulitiplier = 3.0f;

    [SerializeField]
    private int spyCount = 0;

    [SerializeField]
    private TextMeshProUGUI text;

    [SyncVar]
    public int testInput = 0;

    // Update is called once per frame
    void Update()
    {
        if (text != null)
        {
            text.text = spyCount.ToString();
        }
    }

    public override void Interact()
    {
        CmdIncreatePercentage(10);
    }

    [Command]
    public void CmdIncreatePercentage(float a_value)
    {
        capturePercentage += a_value;
        RpcIncreatePercentage(capturePercentage);
    }

    [ClientRpc]
    public void RpcIncreatePercentage(float a_value)
    {
        capturePercentage = a_value;
    }

    [Command]
    public void CmdIncreaseMultiplier()
    {
        captureMulitiplier += 0.5f;
    }

    public void ResetMulitiplier()
    {
        captureMulitiplier = 3.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Spy")
        {
            if(other.GetComponent<PlayerController_Net>().hasAuthority)
            {
                spyCount++;
                //other.GetComponent<PlayerController_Net>().CmdSetInteractable(this);
            }
            //if (!spyIsCapturing)
            //{
            //    spyIsCapturing = true;
            //    showMiniGame = true;
            //
            //    if (showMiniGame)
            //    {
            //        if (other.GetComponent<PlayerController_Net>().hasAuthority)
            //        {
            //            miniGame.Show();
            //        }
            //    }
            //}
            //percentageUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Spy")
        {
            if (other.GetComponent<PlayerController_Net>().hasAuthority)
            {
                spyCount--;
                //showMiniGame = false;
                //miniGame.Hide();
                //if (!showMiniGame)
                //{
                //    percentageUI.SetActive(false);
                //
                //}
            }

            //Collider[] allObjects = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);
            //bool noSpy = true;
            //for (int i = 0; i < allObjects.Length; ++i)
            //{
            //    if(allObjects[i].tag == "Spy")
            //    {
            //        noSpy = false;
            //    }
            //}
            //
            //if (noSpy)
            //{
            //    spyIsCapturing = false;
            //    capturePercentage = Mathf.RoundToInt(capturePercentage);
            //}
        }
    }
}
