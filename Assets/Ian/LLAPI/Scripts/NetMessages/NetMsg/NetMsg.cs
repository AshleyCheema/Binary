namespace LLAPI
{
    public static class NetOP
    {
        public const int NONE = 0;
        public const int CONNECTION = 1;
        public const int NAME = 2;
        public const int POSITION = 3;
        public const int ROTATION = 4;
        public const int PLAYERMOVEMENT = 5;
        public const int SPAWNOBJECT = 6;
        public const int SENDCONNECTIONID = 7;
        public const int CP_CAPTURE = 8;
        public const int NETWORK_OBJECT = 9;
        public const int SPAWN_PLAYER_LB = 10;
        public const int NAME_CHANGE_LB = 11;
        public const int DISCONNECTION = 12;
        public const int TEAM_CHANGE_LB = 13;
        public const int PLAYER_READY_LB = 14;
        public const int IS_READY_LB = 15;
        public const int CLIENT_LOAD_SCENE_LB = 16;
        public const int CLIENT_CONFIRM_LOAD_SCENE_LB = 17;
        public const int AB_SPRINT = 18;
        public const int AB_FIRE = 19;
        public const int AB_TRACKER = 20;
        public const int AB_STUN = 21;
        public const int AB_TRIGGER = 22;

    }

    [System.Serializable]
    public abstract class NetMsg
    {
        public byte OP { get; set; }

        public NetMsg()
        {
            OP = NetOP.NONE;
        }
    }
}
