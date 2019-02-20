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

public class CapturePoint : NetworkBehaviour
{
    [SerializeField]
    private CapturePointMiniGame miniGame;

    [SerializeField]
    private bool spyIsCapturing = false;

    [SerializeField]
    private bool showMiniGame = false;

    [SerializeField]
    private float capturePercentage = 0.0f;

    [SerializeField]
    private float captureMulitiplier = 3.0f;

    private NetworkConnection authoirty;

    // Update is called once per frame
    void Update()
    {
        if (hasAuthority)
        {
            if (spyIsCapturing)
            {
                capturePercentage += (1.0f * captureMulitiplier) * Time.deltaTime;

                if (capturePercentage > 100.0f)
                {
                    capturePercentage = 100.0f;
                }
            }
        }
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

            authoirty = other.gameObject.GetComponent<PlayerController_Net>().SetAuthoirty(GetComponent<NetworkIdentity>());
            other.gameObject.GetComponent<PlayerController_Net>().SetAuthoirty(miniGame.gameObject.GetComponent<NetworkIdentity>());

            Debug.LogWarning("HAS: " + authoirty);

            miniGame.Show();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Spy")
        {
            spyIsCapturing = false;
            showMiniGame = false;
            capturePercentage = Mathf.RoundToInt(capturePercentage);

            miniGame.Hide();
        }
    }
}
