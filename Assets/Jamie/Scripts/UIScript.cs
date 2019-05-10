using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIScript : MonoBehaviour
{
    public TMP_Text PlayerText;
    public TMP_Text TimeText;
    public string PlayerName;
    public bool isSpy;
    public bool isHurt;

    public GameObject Borders;
    public GameObject MercAbil;
    public GameObject SpyAbil;
    public GameObject MercName;
    public GameObject SpyName;

    // Start is called before the first frame update
    private void Start()
    {
        if (!isSpy)
        {
            PlayerText.text = "Merc 01 - " + PlayerName;
            MercAbil.SetActive(true);
        }
        else
        {
            PlayerText.text = "Spy 01 - " + PlayerName;
            SpyAbil.SetActive(true);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateTime();
        UpdateHealth();
    }

    private void UpdateTime()
    {
        //Make time equal to now
        DateTime time = DateTime.Now;

        //Create a 12 hour clock, rather than 24 hour
        int twelveHour = time.Hour;
        if (time.Hour >= 12)
        {
            twelveHour = time.Hour - 12;
        }
        //make string using time
        string hour = LeadingZero(twelveHour);
        string minute = LeadingZero(time.Minute);
        //set format to AM or PM
        string timeformat = "PM";
        if (time.Hour < 11)
        {
            timeformat = "AM";
        }

        //Concatinate string together
        TimeText.text = hour + ":" + minute + " " + timeformat;
    }

    private void UpdateHealth()
    {
        if (isHurt)
        {
            Image[] allChildren = Borders.GetComponentsInChildren<Image>();
            foreach (Image child in allChildren)
            {
                child.color = new Color32(135, 40, 20, 127);
            }

            if (isSpy)
            {
                SpyAbil.SetActive(false);
            }
        }
        else
        {
            Image[] allChildren = Borders.GetComponentsInChildren<Image>();
            foreach (Image child in allChildren)
            {
                child.color = new Color32(0, 0, 0, 153);
            }

            if (isSpy)
            {
                SpyAbil.SetActive(true);
            }
        }
    }

    private string LeadingZero(int n)
    {
        return n.ToString().PadLeft(2, '0');
    }
}