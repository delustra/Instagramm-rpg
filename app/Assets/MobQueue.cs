using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using MoreMobs.Model;
using MoreMobs.AWS;

namespace MoreMobs.MobQ
{

public class MobQueue : MonoBehaviour
{
        public Text mobDisplayText = null;
        public Text DebugQSize = null;
        public int MobsinQMin = 15; //Minimum number of downloaded mobs
        public Image MobImage = null;
        public Mob CurrentMob = new Mob { };
        public Mob DefaultMob = new Mob { id = "defaultmob1", title="Def Mob", inpack_id = "defaultmob"};
        public Sprite DefaultSpite;
        public Queue<Mob> MobsToKillQueue = new Queue<Mob>();
        public AWSConnector aWSConnector;
        public List<MobSprite> MobSprites = new List<MobSprite>();

    void Start() 
    {
           SetDefaultMob(); 
        
    }


    public void KillMob()
    {
            
            Debug.Log("MobKilled");
            CurrentMob = MobsToKillQueue.Dequeue();
            aWSConnector.KillMob(CurrentMob);
            CurrentMobRefreshView();

            if (MobsToKillQueue.Count < MobsinQMin) { aWSConnector.Invoke(); }
     }


    private void SetDefaultMob()
    {

        MobSprites.Add(new MobSprite {inpack_id = "defaultmob", Sprite = DefaultSpite});  
        MobsToKillQueue.Enqueue(DefaultMob);
        
        CurrentMobRefreshView();
     }

    public void DeliverNewMobs(List<Mob> newMobsList)

    {

            newMobsList.ForEach(delegate (Mob deliveredMob)
            {
                deliveredMob.inpack_id = Guid.NewGuid().ToString();
                GetComponent<ImageLoader>().DownloadTexture(deliveredMob.inpack_id, deliveredMob.imglink);
                MobsToKillQueue.Enqueue(deliveredMob);
            });



        }

    public void CurrentMobRefreshView ()
        {
            MobImage.sprite = MobSprites.Find( x=> x.inpack_id.Contains(CurrentMob.inpack_id)).Sprite;
            mobDisplayText.text = CurrentMob.title;
            DebugQSize.text = MobsToKillQueue.Count.ToString();
        }
}
}