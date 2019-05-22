/*
 * Author: Ian Hudson
 * Description: This script is used for a mini game which is shown to the player.
 * The mini game is based on Simon Says. 
 * Created: 20/02/2019
 * Edited By: Ian
 */
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CapturePointMiniGame : MonoBehaviour
{
    //Parent ref
    [SerializeField]
    private GameObject parent = null;

    //Scroll rect ref
    [SerializeField]
    private ScrollRect scroll = null;

    //Define the inputs needed to complete the mini game
    [SerializeField]
    private KeyCode[] inputsNeeded;

    //Deifne which inputs are allowed
    [SerializeField]
    private KeyAllowed[] inputsAllowed;

    //The UI image elemetns 
    [SerializeField]
    private GameObject[] elements;

    //The Image for the feedback
    [SerializeField]
    private Image feedback;

    [SerializeField]
    private Image timerImage;

    //Which input is the player at
    [SerializeField]
    private int inputsIndex = 0;

    //Has the mini game been completed
    [SerializeField]
    private bool isCompleted = false;
    public bool IsCompleted
    { get { return isCompleted; } }

    //Parent capture point this mini game is attahced to
    [SerializeField]
    private NO_CapturePoint parentCapturePoint = null;

    //Keep track if we are in a coroutine
    private bool inCoroutine = false;

    private SpyController spyController;

    public AudioSO hackingSound;
    public AudioSO completeSound;
    private AudioSource audioSource;

    private Coroutine c = null;
    private float maxTime = 8.0f;

    /// <summary>
    /// Show this mini game
    /// </summary>
    public void Show()
    {
        ResetGame();
       // parent.SetActive(true);
       // parent.transform.LookAt(parent.transform.position+ Camera.main.transform.rotation * Vector3.forward,
       //     Camera.main.transform.rotation * Vector3.up);
    }

    /// <summary>
    /// Hide this mini game
    /// </summary>
    public void Hide()
    {
        parent.SetActive(false);
        if(c != null)
        {
            StopCoroutine(c);
        }
        if(!isCompleted)
        {
            inputsIndex = 0;
        }
    }

    /// <summary>
    /// Start
    /// </summary>
    private void Start()
    {
        spyController = GameObject.FindGameObjectWithTag("Spy").GetComponentInChildren<SpyController>();
        audioSource = gameObject.GetComponent<AudioSource>();
        SetInputs();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (!inCoroutine)
        {
            bool doneInput = false;
            if (!isCompleted && Input.GetKeyDown(inputsNeeded[inputsIndex]))
            {
                if(ClientManager.Instance != null && ClientManager.Instance.LocalPlayer.gameAvatar.tag == "Spy")
                {                 
                    spyController.animator.SetBool("isHacking", true);
                    spyController.isHacking = parentCapturePoint.IsBeingCaptured;
                }
                //Need to test if the if statement is needed and then increment min + max distacne and volume
                if (!audioSource.isPlaying)
                {
                    hackingSound.SetSourceProperties(audioSource);
                    audioSource.Play();
                }
                if (25 % parentCapturePoint.capturePercentage == 0)
                {
                    hackingSound.audioMaxDistance += 5;
                }
                if(inputsIndex == 0 && c == null)
                {
                    c = StartCoroutine(FailTimer());
                }
                //GameObject.FindGameObjectWithTag("Spy").GetComponentInChildren<Animator>().SetBool("isHacking", true);
                inCoroutine = true;
                //start coroutine to change colour
                StartCoroutine(Feedback(true));
                doneInput = true;
            }
            if (!doneInput && !isCompleted && CheckInput())
            {
                inCoroutine = true;
                StartCoroutine(Feedback(false));
                inputsIndex = 0;
                scroll.horizontalNormalizedPosition = 0;

                parentCapturePoint.ResetCaptureAmount();

                //error. Wrong key pressed
                Msg_ClientMercFeedback cmf = new Msg_ClientMercFeedback();
                cmf.Location = transform.position;

                ClientManager.Instance?.client.Send(MSGTYPE.CLIENT_FEEDBACK, cmf);
            }
            //This will probably make it go off more then once. NEED TO TEST
            if(isCompleted)
            {
                completeSound.SetSourceProperties(audioSource);
                audioSource.Play();
            }
            foreach (KeyCode item in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(item))
                {
                    Debug.Log("KeyCode Down: " + item);
                }
            }
        }
    }

    private void IncerrmentIndex()
    {
        inputsIndex++;
        if (inputsIndex == 1)
        {
            scroll.horizontalNormalizedPosition = 0;
        }

        scroll.horizontalNormalizedPosition += 0.9f / (inputsNeeded.Length + 1);

        if (inputsIndex > inputsNeeded.Length - 1)
        {
            
            isCompleted = true;
            //Mini Game is completed
            //transform.parent.gameObject.SetActive(false);

            //incrase capturerate
            parentCapturePoint.IncreaseCaptureAmount();

            //reset
            ResetGame();
        }
    }

    private IEnumerator FailTimer()
    {
        float duration;
        duration = maxTime;
        while (duration > 0)
        {
            duration -= Time.deltaTime;
            timerImage.fillAmount = duration / maxTime;
            yield return null;
        }

        inCoroutine = true;
        StartCoroutine(Feedback(false));
        parentCapturePoint.ResetCaptureAmount();
        
        ResetGame();
    }

    /// <summary>
    /// Feedback for the key press
    /// </summary>
    /// <returns></returns>
    private IEnumerator Feedback(bool aInput)
    {
        if (aInput)
        {
            feedback.color = Color.green;
        }
        else
        {
            feedback.color = Color.red;
        }
        float step = 0.5f;
        while(step >= 0.0f)
        {
            step -= Time.deltaTime;
            yield return null;
        }
        if (aInput)
        {
            IncerrmentIndex();
        }
        feedback.color = Color.white;
        inCoroutine = false;
    }

    /// <summary>
    /// Reset the mini game
    /// </summary>
    private void ResetGame()
    {
        if(c != null)
        {
            StopCoroutine(c);
            c = null;
        }
        isCompleted = false;
        scroll.horizontalNormalizedPosition = 0;
        timerImage.fillAmount = 1f;
        inputsIndex = 0;
        SetInputs();
    }

    /// <summary>
    /// Set all the needed inputs for the mini game
    /// </summary>
    private void SetInputs()
    {
        inputsNeeded = new KeyCode[5];

        for (int i = 0; i < inputsNeeded.Length; i++)
        {
            int rand = UnityEngine.Random.Range(0, 4);
            inputsNeeded[i] = inputsAllowed[rand].KeyCode;
            elements[i].GetComponent<Image>().sprite = inputsAllowed[rand].Sprite;
        }
    }

    /// <summary>
    /// Check if the corect input has been processed
    /// </summary>
    /// <returns></returns>
    private bool CheckInput()
    {
        for (int i = 0; i < inputsAllowed.Length; i++)
        {
            if (Input.GetKeyDown(inputsAllowed[i].KeyCode))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Check if element is within an array 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="a_array"></param>
    /// <param name="a_value"></param>
    /// <returns></returns>
    private bool ArrayContains<T>(T[] a_array, T a_value)
    {
        for (int i = 0; i < a_array.Length; i++)
        {
            if(a_array[i].Equals(a_value))
            {
                return true;
            }
        }
        return false;
    }
}

/// <summary>
/// Struct: Define the keycode which is allowed with it's corisponding sprite
/// </summary>
[System.Serializable]
public struct KeyAllowed
{
    public KeyCode KeyCode;
    public Sprite Sprite;
}
