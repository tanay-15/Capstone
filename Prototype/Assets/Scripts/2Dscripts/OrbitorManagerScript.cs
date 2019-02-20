using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitorManagerScript : MonoBehaviour {

    Transform[] Orbitors;
    public GameObject impact;

    Transform Player;

    bool ResetThrow = false;

	// Use this for initialization
	void Start () {

        Player = GameObject.Find("Player").transform;
        Orbitors = new Transform[3];

        int i = 0;
        foreach (Transform child in transform)
        {
            //Debug.Log(child.name);
            Orbitors[i] = child;
            i++;
        }

    }
	
	// Update is called once per frame
	void Update () {

        if (Mathf.Abs(Input.GetAxis("RHorizontal")) < 0.7f && Mathf.Abs(Input.GetAxis("RVertical")) < 0.7f)
            ResetThrow = false;

// Throwing projectiles
        if ((Input.GetMouseButtonDown(0) || Mathf.Abs(Input.GetAxis("RHorizontal")) > 0.7f || Mathf.Abs(Input.GetAxis("RVertical")) > 0.7f) && ResetThrow == false)
        {
            Vector3 offset = new Vector3(0.5f, 0, 0) * (Input.GetAxis("RHorizontal") > 0 ? 1 : -1);
            Vector3 direction = new Vector3();

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                direction = Vector3.Normalize(new Vector3(mouseClick.x, mouseClick.y, 0) - transform.position - offset);
            }
            if ((Mathf.Abs(Input.GetAxis("RHorizontal")) > 0.7f || Mathf.Abs(Input.GetAxis("RVertical")) > 0.7f))
            {
                direction = Vector3.Normalize(new Vector3(Input.GetAxis("RHorizontal"), Input.GetAxis("RVertical"), 0));
                ResetThrow = true;
            }
                

            foreach (Transform t in Orbitors)
            {
                if (t.GetComponent<OrbitorScript>().Status == OrbitorScript.State.HasProjectile)
                {
                                     
                    t.GetComponent<OrbitorScript>().Projectile.GetComponent<Rigidbody2D>().velocity = 12 * direction;
                    t.GetComponent<OrbitorScript>().Projectile.GetComponent<Collider2D>().enabled = true;
                    //t.GetComponent<OrbitorScript>().Projectile.GetComponent<OrbitorObjectScript>().isOrbiting = false;
                    t.GetComponent<OrbitorScript>().Projectile.transform.parent = null;
                    t.GetComponent<OrbitorScript>().Projectile.transform.position = transform.position + offset;
                    t.GetComponent<OrbitorScript>().fracJourney = 0;
                    t.GetComponent<OrbitorScript>().Status = OrbitorScript.State.ShootingProjectile;

                    Instantiate(impact, transform.position + offset, Quaternion.identity);

                    break;
                }
                
            }

            
        }



    }
}
