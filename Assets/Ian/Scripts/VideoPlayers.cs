using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoPlayers : MonoBehaviour
{
    //video players 
    [SerializeField]
    private VideoPlayer mercVideo;
    [SerializeField]
    private VideoPlayer spyVideo;

    // Start is called before the first frame update
    void Awake()
    {
        //set the video for the player depending on the team
        if (ClientManager.Instance != null)
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

        //if server, invoke function
        if(HostManager.Instance != null)
        {
            mercVideo.enabled = false;
            spyVideo.enabled = false;
            Invoke(nameof(VideoFinshed), 1.0f);
        }
    }


    void VideoFinshed()
    {
        //load new level
        if(HostManager.Instance != null)
        {
            //change scene
            HostManager.Instance.ServerChangeScene("NewLevel");
        }
    }
}
