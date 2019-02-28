using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody rb;
        public Client client;

        [SerializeField]
        private Vector3 velocity;
        [SerializeField]
        private Vector3 rawVelocity;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            float xMov = Input.GetAxis("Horizontal");
            float yMov = Input.GetAxis("Vertical");
            //tansform.Translate(xMov * Time.deltaTime, 0, yMov * Time.deltaTime);

            if (xMov != 0 || yMov != 0)
            {
                rawVelocity = new Vector3(xMov, 0, yMov);
                velocity = new Vector3(xMov, 0, yMov) * 5 * Time.deltaTime;
                //transform.Translate(velocity);
                //rb.MovePosition(rb.position + rawVelocity * 5 * Time.fixedDeltaTime);


                NetMsg_PlayerMovement serverVelocity = new NetMsg_PlayerMovement();
                serverVelocity.xMove = rb.position.x;//rawVelocity.x;
                serverVelocity.yMove = rb.position.z;//rawVelocity.z;

                client.Send(serverVelocity, Client.Instance.ReliableChannel);
            }
            else
            {
                rawVelocity = new Vector3(0, 0, 0);
            }
        }

        private void FixedUpdate()
        {

            rb.MovePosition(rb.position + rawVelocity * 5 * Time.fixedDeltaTime);
        }
    }
}
