using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    [System.Serializable]
    public class NetMsg_ClientConfirmSceneLoadLB : NetMsg
    {
        public NetMsg_ClientConfirmSceneLoadLB()
        {
            OP = NetOP.CLIENT_CONFIRM_LOAD_SCENE_LB;
        }

        public int ConnectionID { get; set; }
        public bool SceneLoaded { get; set; }
    }
}
