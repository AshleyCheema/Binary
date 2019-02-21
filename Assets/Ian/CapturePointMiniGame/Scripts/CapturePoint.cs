/*
 * Author: Ian Hudson
 * Description: 
 * Created: 19/02/2019
 * Edited By: Ian
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class CapturePoint : NetworkBehaviour
{
    [SerializeField]
    private CapturePointMiniGame miniGame;

    [SerializeField]
    [SyncVar]
    private bool spyIsCapturing = false;

    [SerializeField]
    private bool showMiniGame = false;

    [SerializeField]
    [SyncVar]
    private float capturePercentage = 0.0f;

    [SerializeField]
    [SyncVar]
    private float captureMulitiplier = 3.0f;

    private NetworkConnection authoirty;

    [SerializeField]
    private TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        text.text = capturePercentage.ToString();
        //if (hasAuthority)
        //{
            if (spyIsCapturing)
            {
                capturePercentage += (1.0f * captureMulitiplier) * Time.deltaTime;

                if (capturePercentage > 100.0f)
                {
                    capturePercentage = 100.0f;
                }
            }
        //}
    }

    public void IncreaseMultiplier()
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
            spyIsCapturing = true;
            showMiniGame = true;

            //authoirty = other.gameObject.GetComponent<PlayerController_Net>().SetAuthoirty(GetComponent<NetworkIdentity>());
            //other.gameObject.GetComponent<PlayerController_Net>().SetAuthoirty(miniGame.gameObject.GetComponent<NetworkIdentity>());

            if (showMiniGame)
            {
                miniGame.Show();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Spy")
        {
            spyIsCapturing = false;
            showMiniGame = false;
            capturePercentage = Mathf.RoundToInt(capturePercentage);

            if (!showMiniGame)
            {
                miniGame.Hide();
            }
        }
    }
}
