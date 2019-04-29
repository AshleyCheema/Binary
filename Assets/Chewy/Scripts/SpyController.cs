using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpyState
{
    Normal,
    Hurt,
    Dead
}

public class SpyController : PlayerController
{
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    public GameObject stun;
    private Trigger bulletTrigger;
    private SpyState currentState;
    public SpyState CurrentState
    { get { return currentState; } set { currentState = value; } }

    public bool stunDrop;

    //Audio
    private AudioSource audioSource;
    public AudioSO walking;
    public AudioSO run;

    // Start is called before the first frame update
    public override void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        bullet = GameObject.Find("Bullet");
        if (stun == null)
        {
            stun = GameObject.Find("StunG");
        }
        bulletTrigger = bullet.GetComponent<Trigger>();
        currentState = SpyState.Normal;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (stun.GetComponent<StunAbility>().IsActive == false)
            {
                stun.GetComponent<StunAbility>().IsActive = true;
                stunDrop = true;
            }
        }
        
    }

    public void Shot()
    {
        if (currentState == SpyState.Normal)
        {
            //Change animation
            //Maybe drip blood?
            Debug.Log("Hurt State");
            currentState = SpyState.Hurt;
        }
        else if (currentState == SpyState.Hurt)
        {
            bulletTrigger.hasShot = false;
            currentState = SpyState.Dead;
            Debug.Log("Dead");

            //Send message to host
            //spy is daed
            Msg_ClientState cs = new Msg_ClientState();
            cs.connectId = (int)ClientManager.Instance?.LocalPlayer.connectionId;
            cs.state = currentState;
            ClientManager.Instance?.client.Send(MSGTYPE.CLIENT_STATE, cs);
        }
    }

    private void Step()
    {
        walking.SetSourceProperties(audioSource);
        audioSource.Play();
    }
    
    private void Run()
    {
        run.SetSourceProperties(audioSource);
        audioSource.Play();
    }
}
