using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    public class Network_Object : MonoBehaviour
    {
        [Tooltip("This is the ID for this object. THIS MUST BE THE SAME ON CLIENT AND SERVER")]
        [SerializeField]
        private int id;
        public int ID
        { get { return id; } }

        //[Tooltip("Is this object controlled by the server")]
        //[SerializeField]
        //private bool shouldRun = true;
        //public bool ShouldRun
        //{ get { return shouldRun; } }

        /// <summary>
        /// This function 
        /// </summary>
        public virtual void Recive(NetMsg a_netMsg)
        {
        }
    }
}
