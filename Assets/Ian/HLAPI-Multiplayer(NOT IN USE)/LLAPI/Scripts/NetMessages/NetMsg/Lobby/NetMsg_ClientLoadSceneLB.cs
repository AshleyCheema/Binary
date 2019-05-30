using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LLAPI
{
    //client has loaded scene message
    [System.Serializable]
    public class NetMsg_ClientLoadSceneLB : NetMsg
    {
        public NetMsg_ClientLoadSceneLB()
        {
            OP = NetOP.CLIENT_LOAD_SCENE_LB;
        }

        public int ConnectionID { get; set; }
        public int SceneToLoad { get; set; }
    }
}
