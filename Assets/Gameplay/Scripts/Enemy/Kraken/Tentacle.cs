using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tentacle : MonoBehaviour
{
    [SerializeField] private TentacleStuff[] tentacleList;
    [SerializeField] float spawnTime;
    // Start is called before the first frame update
    void Start()
    {
       // for (int i = 0; i < tentacleList.Length; i++)
       // {
       //     tentacleList[i].tentacleAnimator.;
       // }
    }
    private void OnEnable()
    {
        for (int i = 0; i < tentacleList.Length; i++)
        {
            tentacleList[i].tentacleModel.SetActive(true);
            tentacleList[i].tentacleAnimator.enabled = true;
        }
    }
    public void KrakenDead()
    {
        for (int i = 0; i < tentacleList.Length; i++)
        {
            tentacleList[i].tentacleAnimator.SetTrigger("KrakenDead");
        }
    }
    public void ActivateHitBoxes()
    {
        for(int i = 0; i < tentacleList.Length; i++)
            tentacleList[i].hitBox.SetActive(true);
    }

    public void PlaySound()
    {
        SoundManager.PlayAudioClip("TentacleSlam",tentacleList[0].tentacleModel.transform.position, 1f);
    }

    [Serializable]
    public class TentacleStuff
    {
        public GameObject tentacleModel;
        public Animator tentacleAnimator;
        public GameObject hitBox;
    }
}
