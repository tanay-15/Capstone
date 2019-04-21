using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHook : MonoBehaviour
{
    public GameObject Boss;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player" /*&& Boss.GetComponent<SkeletonBoss>().currentState != SkeletonBoss.BossStates.Idle*/)
        {
            

            collision.gameObject.SendMessage("GetHit", -12f);
        }
    }
}
