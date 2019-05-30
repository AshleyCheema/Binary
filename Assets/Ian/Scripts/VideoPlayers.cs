using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoPlayers : MonoBehaviour
{
    [SerializeField]
    private VideoPlayer mercVideo;
    [SerializeField]
    private VideoPlayer spyVideo;

    float videoLength;

    // Start is called before the first frame update
    void Awake()
    {
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

        if(HostManager.Instance != null)
        {
            mercVideo.enabled = false;
            spyVideo.enabled = false;
            Invoke(nameof(VideoFinshed), 70.0f);
        }
    }


    void VideoFinshed()
    {
        //load new level
        if(HostManager.Instance != null)
        {
            HostManager.Instance.ServerChangeScene("NewLevel");
        }
    }
}
