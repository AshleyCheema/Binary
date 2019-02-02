using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.right * 0.05f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.right * -0.05f);
        }

        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.forward * 0.05f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.forward * -0.05f);
        }
    }
}
