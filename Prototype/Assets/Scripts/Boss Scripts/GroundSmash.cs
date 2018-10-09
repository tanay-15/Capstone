using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundSmash : MonoBehaviour {

    // Use this for initialization
    public GameObject boss;

	void Start () {

        boss = GameObject.Find("Hound");
	}
	
	// Update is called once per frame
	void Update () {

        this.transform.Translate(-Vector3.right * 5f * Time.deltaTime);

        float destroycounter = 1f;
        destroycounter = destroycounter - Time.deltaTime;

        if(destroycounter <= 0)
        {
            Destroy(this.gameObject);
            boss.GetComponent<Boss1>().SetAttackDone(true);
        }


	}


    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
            boss.GetComponent<Boss1>().SetAttackDone(true);

        }

        if(collision.gameObject.tag == "Grabbable")
        {
            Destroy(this.gameObject);
            boss.GetComponent<Boss1>().SetAttackDone(true);
        }

        if (collision.gameObject.tag == "Wall")
        {
            Destroy(this.gameObject);
            boss.GetComponent<Boss1>().SetAttackDone(true);
        }
    }
}
