using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NO_CapturePoint : MonoBehaviour
{
    public int ID;
    
    //variables
    [SerializeField]
    private bool isBeingCaptured = false;
    public bool IsBeingCaptured
    {
        get
        {
            return isBeingCaptured;
        }
        set
        {
            isBeingCaptured = value;

            if(isBeingCaptured)
            {
                if (c_lerpColor == null)
                {
                    c_lerpColor = StartCoroutine(LerpColor(Color.red, Color.green));
                }
            }
            else
            {
                if (c_lerpColor != null)
                {
                    StopCoroutine(c_lerpColor);
                    c_lerpColor = null;
                }
            }
        }
    }
    public float baseCaptureAmount = 0.5f;
    public float maxCaptureAmount = 4.0f;
    [SerializeField]
    private float captureAmount;
    [SerializeField]
    public float capturePercentage = 0.0f;
    [SerializeField]
    private TextMeshProUGUI tm;

    public GameObject[] objectsAround;
    private Coroutine c_lerpColor = null;
    private SpyController spyController;
    private Msg_ClientCapaturePoint ccp = new Msg_ClientCapaturePoint();

    //list of all spies in this capture point
    public List<GameObject> currentSpies = new List<GameObject>();

    [SerializeField]
    private GameObject miniGame;

    //player is currently hacking
    private bool ishacking = false;

    //has this capture point been captured
    public bool IsCaptured
    {
        get
        {
            if ((int)captureAmount == 100 || capturePercentage > 99.9f)
            {
                return true;
            }
            return false;
        }
    }

    private void Start()
    {
        ccp.ID = -1;
        captureAmount = baseCaptureAmount;
    }

    /// <summary>
    /// Update
    /// </summary>
    private void Update()
    {
        if (Input.GetButtonDown("Hacking") && !IsCaptured && !ishacking)
        {
            //if the local player is within this capture point. Allow them to hack
            if (currentSpies.Contains(ClientManager.Instance?.LocalPlayer.gameAvatar))
            {
                StartHacking();

                //send message. This capture point is being hacked
                if (ccp.ID == -1)
                {
                    ccp.connectId = ClientManager.Instance.LocalPlayer.connectionId;
                    ccp.IsBeingCaptured = true;
                    ccp.ID = ID;
                    ClientManager.Instance.client.Send(MSGTYPE.CLIENT_CAPTURE_POINT, ccp);
                }
            }
        }
        //another spy is capturing
        if (Input.GetButtonDown("Hacking") && !IsCaptured && IsBeingCaptured && !ishacking)
        {
            ishacking = true;
            GetComponent<MeshRenderer>().material.color = Color.red;

            //null check
            if (spyController != null)
            {
                //spyController.cooldownScript.gameObject.SetActive(false);
                spyController.isHacking = true;

                //if spy contoller is local player 
                //active mini game
                if (ClientManager.Instance.LocalPlayer.gameAvatar == spyController.transform.parent.gameObject)
                {
                    miniGame.SetActive(true);
                    miniGame.GetComponentInChildren<CapturePointMiniGame>().Show();
                    if (c_lerpColor == null)
                    {
                        //c_lerpColor = StartCoroutine(LerpColor(Color.red, Color.green));
                    }
                }
            }
        }

        //check if spy is dead
        if (ClientManager.Instance != null)
        {
            if (currentSpies.Contains(ClientManager.Instance.LocalPlayer.gameAvatar))
            {
                if (ClientManager.Instance.LocalPlayer.gameAvatar.GetComponentInChildren<SpyController>().CurrentState == SpyState.Dead)
                {
                    currentSpies.Remove(ClientManager.Instance.LocalPlayer.gameAvatar);

                    bool reset = true;
                    if (currentSpies.Count > 0)
                    {
                        reset = false;
                    }
                    miniGame.SetActive(false);
                    spyController.cooldownScript.canHack = false;
                    spyController.isHacking = false;
                    ishacking = false;

                    //if no spy is within capture point range
                    //set IsBeingCaptured to false and send message to other clients
                    if (reset)
                    {
                        GetComponent<MeshRenderer>().material.color = Color.white;
                        IsBeingCaptured = false;

                        ccp.connectId = ClientManager.Instance.LocalPlayer.connectionId;
                        ccp.IsBeingCaptured = isBeingCaptured;
                        ccp.ID = ID;
                        ClientManager.Instance.client.Send(MSGTYPE.CLIENT_CAPTURE_POINT, ccp);
                        ccp.ID = -1;

                        Msg_ClientCaptureStats ccs = new Msg_ClientCaptureStats();
                        ccs.CapturePercentage = (int)capturePercentage;
                        ccs.ID = ID;
                        ClientManager.Instance.client.Send(MSGTYPE.CLIENT_CAPTURE_PERCENTAGE, ccs);

                        captureAmount = baseCaptureAmount;
                    }
                }
            }
        }

        //if capture ibeing captured
        //increase capture percentage
        if (isBeingCaptured)
        {
            capturePercentage += captureAmount * Time.deltaTime;

            //increase player stats
            if (capturePercentage < 100.0f)
            {
                PlayerStats.Instance.CaptureedAmount += captureAmount * Time.deltaTime;
            }

            if (tm != null)
            {
                tm.text = ((int)capturePercentage).ToString() + "%";
            }
            //clamp capture percentage
            if (capturePercentage > 100.0f)
            {
                capturePercentage = 100.0f;

                //deactive mini game
                miniGame.SetActive(false);
                IsBeingCaptured = false;

                //set all objects colour to clue
                for (int i = 0; i < objectsAround.Length; i++)
                {
                    objectsAround[i].GetComponent<EmissionChange>().ColourChange(Color.cyan);
                }

                //if we are the host/server
                if (HostManager.Instance != null)
                {
                    //tell exit manager capture point has been captured
                    ExitManager.Instance.CapturePointCaptured();
                }
                else
                {
                    if (ClientManager.Instance != null)
                    {
                        Msg_ClientCaptureStats ccs = new Msg_ClientCaptureStats();
                        ccs.ID = ID;
                        ccs.CapturePercentage = (int)capturePercentage;
                        ClientManager.Instance.client.Send(MSGTYPE.CLIENT_CAPTURE_PERCENTAGE, ccs);
                    }
                }
            }
        }
    } 

    //increase capture amount per second
    public void IncreaseCaptureAmount(bool aSend = true)
    {
        captureAmount += 0.5f;
        if (captureAmount > maxCaptureAmount)
        {
            captureAmount = maxCaptureAmount;
        }

        if (aSend)
        {
            //send message to server and clients 
            Msg_ClientCapturePointIncrease ccpi = new Msg_ClientCapturePointIncrease();
            ccpi.CapturePointAmount = captureAmount;
            ccpi.NOIndex = ID;
            ClientManager.Instance?.client.Send(MSGTYPE.CLIENT_CAPTURE_POINT_INCREASE, ccpi);
        }
    }

    //reset capture amount
    public void ResetCaptureAmount()
    {
        captureAmount = baseCaptureAmount;
    }

    /// <summary>
    /// Enter trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (ClientManager.Instance != null)
        {
            if (other.tag == "Spy")
            {
                spyController = other.gameObject.GetComponentInChildren<SpyController>();
                spyController.cooldownScript.canHack = true;

                currentSpies.Add(other.gameObject);
            }
        }
    }

    //enable the mini game ui, and set objects colours
    private void StartHacking()
    {
        GetComponent<MeshRenderer>().material.color = Color.red;

        if (spyController != null)
        {
            spyController.isHacking = true;

            if (ClientManager.Instance.LocalPlayer.gameAvatar == spyController.transform.parent.gameObject)
            {
                miniGame.SetActive(true);
                miniGame.GetComponentInChildren<CapturePointMiniGame>().Show();
            }
        }
        ishacking = true;
        IsBeingCaptured = true;//ccp.IsBeingCaptured;
    }

    //lerp the objects colour between a start and end colour depending on
    //capture percentage
    private IEnumerator LerpColor(Color start, Color end)
    {
        float lerpTimer = 0f;

        while (lerpTimer <= 1f)
        {
            lerpTimer = capturePercentage / 100;

            Color newColor = Color.Lerp(start, end, lerpTimer);

            for (int i = 0; i < objectsAround.Length; i++)
            {
                objectsAround[i].GetComponent<EmissionChange>().ColourChange(newColor);
            }

            yield return null;
        }

        for (int i = 0; i < objectsAround.Length; i++)
        {
            objectsAround[i].GetComponent<EmissionChange>().ColourChange(end);

            if(objectsAround[i].GetComponent<cakeslice.Outline>() != null)
            {
                objectsAround[i].GetComponent<cakeslice.Outline>().color = 1;
            }
        }

        yield return null;
    }

    /// <summary>
    /// Exit trigger
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (ClientManager.Instance != null)
        {
            if (other.tag == "Spy")
            {
                currentSpies.Remove(other.gameObject);

                bool reset = true;
                if(currentSpies.Count > 0)
                {
                    reset = false;
                }

                //if this is the local player then disable the mini game ui
                if (ClientManager.Instance.LocalPlayer.gameAvatar == other.gameObject)
                {
                    miniGame.SetActive(false);
                    spyController.cooldownScript.canHack = false;            
                    spyController.isHacking = false;
                    ishacking = false;
                }

                //if no spy is within capture point range
                //set IsBeingCaptured to false and send message to other clients
                if (reset)
                {
                    GetComponent<MeshRenderer>().material.color = Color.white;
                    IsBeingCaptured = false;
                   
                    ccp.connectId = ClientManager.Instance.LocalPlayer.connectionId;
                    ccp.IsBeingCaptured = isBeingCaptured;
                    ccp.ID = ID;
                    ClientManager.Instance.client.Send(MSGTYPE.CLIENT_CAPTURE_POINT, ccp);
                    ccp.ID = -1;

                    Msg_ClientCaptureStats ccs = new Msg_ClientCaptureStats();
                    ccs.CapturePercentage = (int)capturePercentage;
                    ccs.ID = ID;
                    ClientManager.Instance.client.Send(MSGTYPE.CLIENT_CAPTURE_PERCENTAGE, ccs);

                    captureAmount = baseCaptureAmount;
                }
            }
        }
    }
}