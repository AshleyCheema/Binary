using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayers : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer mercVideo;
    [SerializeField]
    private VideoPlayer spyVideo;

    float videoLength;

    // Start is called before the first frame update
    void Start()
    {
        if (ClientManager.Instance.LocalPlayer.playerTeam == LLAPI.Team.Merc)
        {
            mercVideo.enabled = true;
            spyVideo.enabled = false;

           

        }
        else
        {
            mercVideo.enabled = false;
            spyVideo.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
