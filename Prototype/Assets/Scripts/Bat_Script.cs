using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat_Script : MonoBehaviour {

    float theta = 0;
    float radius = 10;

    Vector3 offset;

    bool begin = false;

    public GameObject player;

	// Use this for initialization
	void Start () {

        offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.7f), 0);

        int randomInt = (int)Random.Range(0f, 13f);

        if (randomInt == 0)
            theta = 0;
        else if(randomInt == 1)
            theta = 30;
        else if (randomInt == 2)
            theta = 60;
        else if (randomInt == 3)
            theta = 90;
        else if (randomInt == 4)
            theta = 120;
        else if (randomInt == 5)
            theta = 150;
        else if (randomInt == 6)
            theta = 180;
        else if (randomInt == 7)
            theta = 210;
        else if (randomInt == 8)
            theta = 240;
        else if (randomInt == 9)
            theta = 270;
        else if (randomInt == 10)
            theta = 300;
        else if (randomInt == 11)
            theta = 330;
        else if (randomInt == 12)
            theta = 360;



        Debug.Log(randomInt);

        StartCoroutine("DelayBegin");
    }
	
	// Update is called once per frame
	void Update () {

        if (begin)
        {
            GetComponent<TrailRenderer>().enabled = true;

            theta += 3 * Time.deltaTime;
            radius -= 6 * Time.deltaTime;

            if (radius < 0)
            {
                GetComponent<Animator>().SetBool("Explode", true);
                transform.localScale = new Vector3(2, 2, 1);
            }
            else
            {
                transform.position = player.transform.position + new Vector3(radius * Mathf.Sin(theta), radius * Mathf.Cos(theta)) + offset;
            }
        }
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {

        Destroy(gameObject);
    }
    */

    IEnumerator DelayBegin()
    {
        yield return new WaitForSeconds(Random.Range(0f,1.5f));

        begin = true;

        yield return new WaitForSeconds(3);

        Destroy(gameObject);
    }
}
