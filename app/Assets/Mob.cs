using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


namespace MoreMobs.Model
{
    [System.Serializable]
    public class Mob 
    {

        public string inpack_id;
        public string id;
        public string imglink;
        public string title;
                
    }

    public class MobSprite
    {
        public string inpack_id;
        public Sprite Sprite;
    }

    public class DeadMob
    {
        public string id;
        public string mobid;
        public string playerid;
        public string timestamp;
    }


}

