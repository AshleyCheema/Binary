﻿/*
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
    private GameObject percentageUI;

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

    private List<GameObject> spys = new List<GameObject>();

    private NetworkConnection authoirty;

    [SerializeField]
    private TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        //if (hasAuthority)
        //{
            Debug.LogWarning("CapturePoint updating");
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
        //}
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
            if (!spyIsCapturing)
            {
                spyIsCapturing = true;
                showMiniGame = true;

                if (showMiniGame)
                {
                    if (other.GetComponent<PlayerController_Net>().hasAuthority)
                    {
                        miniGame.Show();
                    }
                }
            }
            percentageUI.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Spy")
        {
            if (other.GetComponent<PlayerController_Net>().hasAuthority)
            {
                showMiniGame = false;
                miniGame.Hide();
                if (!showMiniGame)
                {
                    percentageUI.SetActive(false);

                }
            }

            Collider[] allObjects = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);
            bool noSpy = true;
            for (int i = 0; i < allObjects.Length; ++i)
            {
                if(allObjects[i].tag == "Spy")
                {
                    noSpy = false;
                }
            }

            if (noSpy)
            {
                spyIsCapturing = false;
                capturePercentage = Mathf.RoundToInt(capturePercentage);
            }
        }
    }
}
