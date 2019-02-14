using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatScript : MonoBehaviour
{
    float theta = 0;
    float radius = 0;
    int direction = 1;

    Vector3 offset;

    bool begin = false;

    public GameObject player;
    public bool DemonModeActive = false;

    // Use this for initialization
    void Start()
    {
        DemonModeActive = player.GetComponent<DemonTransformScript>().DemonModeActive;

        offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.7f), -1);

        if (DemonModeActive)
        {
            radius = 0;
            direction = -1;
            GetComponent<Animator>().SetBool("Explode", true);
            transform.position = player.transform.position + offset;
        }
        else
        {
            radius = 10;
            direction = 1;
            GetComponent<Animator>().SetBool("Explode", false);
        }




        int randomInt = (int)Random.Range(0f, 13f);

        if (randomInt == 0)
            theta = 0;
        else if (randomInt == 1)
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


        StartCoroutine("DelayBegin");
    }


    void Update()
    {
        if (begin)
        {
            GetComponent<TrailRenderer>().enabled = true;

            // Decrementing to create spiral path
            theta += 3 * Time.deltaTime * direction;
            radius -= 6 * Time.deltaTime * direction;

            if (radius < 0)
            {
                GetComponent<Animator>().SetBool("Explode", true);
            }
            else
            {
                transform.position = player.transform.position + new Vector3(radius * Mathf.Sin(theta), radius * Mathf.Cos(theta)) + offset;
            }
        }
        else if (DemonModeActive)
            transform.position = player.transform.position + offset;

        DemonModeActive = player.GetComponent<DemonTransformScript>().DemonModeActive;
    }

    IEnumerator DelayBegin()
    {
        float x;

        if (DemonModeActive)
            x = Random.Range(2f, 3f);
        else
            x = Random.Range(0f, 1f);

        yield return new WaitForSeconds(x);

        begin = true;
        GetComponent<Animator>().SetBool("Explode", false);
        GetComponent<TrailRenderer>().material.color = new Color(Random.Range(0.5f, 1f), 0, Random.Range(0, 0.2f));


        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
}
