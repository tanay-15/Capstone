using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBossProjectile : MonoBehaviour {

	// Use this for initialization
	void Start () {

        StartCoroutine("DelayedDestroy");

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(transform.name[0] == 'G')
                PlayerLife.sharedInstance.AddLife(-10);
            else if (transform.name[0] == 'B')
                PlayerLife.sharedInstance.AddLife(-30);

            Destroy(gameObject);
        }
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
