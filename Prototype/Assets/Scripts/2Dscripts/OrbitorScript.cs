using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitorScript : MonoBehaviour {

    GameObject Player;
    GameObject Trail;
    GameObject Projectile;
    public Camera MainCam;

    public float speed = 1.0f;
    public float radius = 2.0f;
    public float theta = 0;

    public bool hasProjectile = false;
    public bool shootingProjectile = false;

    //private float startTime = 0;
    //private float journeyLength = 0;
    float fracJourney = 0;

    enum State
    {
        NoProjectile ,
        HasProjectile ,
        ShootingProjectile 
    };

    State Status;

    void Start () {

        Player = transform.parent.gameObject;
        Trail = transform.GetChild(0).gameObject;
    }


    void Update()
    {

        /*
         
      

        switch(Status)
        {
            case State.HasProjectile:
                {
                    Trail.SetActive(true);
                    Trail.transform.position = Projectile.transform.position;

                    // Lerping 
                    if (!shootingProjectile && Vector3.Distance(Player.transform.position, Projectile.transform.position) < 3f)
                    {
                        // flags before picking up object
                        if (!Projectile.GetComponent<OrbitorObjectScript>().isOrbiting)
                        {
                            Projectile.GetComponent<OrbitorObjectScript>().isOrbiting = true;
                            Projectile.GetComponent<OrbitorObjectScript>().hit = false;
                            Projectile.GetComponent<Collider2D>().enabled = false;
                            Projectile.GetComponent<Rigidbody2D>().gravityScale = 0;

                        }
                        else
                        {
                            fracJourney += Time.deltaTime;

                            Projectile.transform.position = Vector3.Lerp(Projectile.transform.position, Projectile.transform.parent.position, fracJourney);


                        }
                    }

                    // Throw to mouse click position
                    if (Input.GetMouseButtonDown(0))
                    {

                        if (transform.localPosition.x > 1f && !shootingProjectile && transform.localPosition.y > -0.3f)
                        {
                            Vector3 mouseClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                            Vector3 direction = Vector3.Normalize(new Vector3(mouseClick.x, mouseClick.y, 0) - Projectile.transform.position);
                            Projectile.GetComponent<Rigidbody2D>().velocity = 10 * direction;
                            Projectile.GetComponent<Collider2D>().enabled = true;

                            //shootingProjectile = true;
                            Projectile.GetComponent<OrbitorObjectScript>().isOrbiting = false;
                            Projectile.transform.parent = null;
                            fracJourney = 0;

                        }

                    }

                    if (Projectile.GetComponent<OrbitorObjectScript>().hit)
                    {
                        Status = State.NoProjectile;
                        //shootingProjectile = false;
                        //hasProjectile = false;
                        Trail.SetActive(false);
                    }
                    break;
                }
        }

          */




























        // Check for Demon Mode
        if (Player.GetComponent<DemonModeScript>().DemonModeActive)
        {

            // Orbiting path
            theta += 2 * Time.deltaTime;
            transform.localPosition = new Vector3(radius * Mathf.Sin(theta), radius * Mathf.Cos(theta), 0);


            if (transform.childCount > 1)
            {
                hasProjectile = true;
            }


            if (hasProjectile)
            {
                Trail.SetActive(true);
                Trail.transform.position = Projectile.transform.position;

                // Lerping 
                if (!shootingProjectile && Vector3.Distance(Player.transform.position, Projectile.transform.position) < 3f)
                {
                    // flags before picking up object
                    if (!Projectile.GetComponent<OrbitorObjectScript>().isOrbiting)
                    {
                        Projectile.GetComponent<OrbitorObjectScript>().isOrbiting = true;
                        Projectile.GetComponent<OrbitorObjectScript>().hit = false;
                        Projectile.GetComponent<Collider2D>().enabled = false;
                        Projectile.GetComponent<Rigidbody2D>().gravityScale = 0;

                    }
                    else
                    {
                        fracJourney += Time.deltaTime;

                        Projectile.transform.position = Vector3.Lerp(Projectile.transform.position, Projectile.transform.parent.position, fracJourney);


                    }
                }




                // Throw to mouse click position
                if (Input.GetMouseButtonDown(0))
                {

                    if (transform.localPosition.x > 1f && !shootingProjectile && transform.localPosition.y > -0.3f)
                    {
                        Vector3 mouseClick = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                        Vector3 direction = Vector3.Normalize(new Vector3(mouseClick.x, mouseClick.y, 0) - Projectile.transform.position);
                        Projectile.GetComponent<Rigidbody2D>().velocity = 10 * direction;
                        Projectile.GetComponent<Collider2D>().enabled = true;

                        shootingProjectile = true;
                        Projectile.GetComponent<OrbitorObjectScript>().isOrbiting = false;
                        Projectile.transform.parent = null;
                        fracJourney = 0;

                        Trail.SetActive(true);


                    }

                }


                if (Projectile.GetComponent<OrbitorObjectScript>().hit)
                {
                    shootingProjectile = false;
                    hasProjectile = false;
                    Trail.SetActive(false);
                }

            }

            if (hasProjectile || shootingProjectile)
            {
                Trail.transform.position = Projectile.transform.position;
            }



        }

    }

    private void FixedUpdate()
    {
        if(!hasProjectile && !shootingProjectile && Player.GetComponent<DemonModeScript>().DemonModeActive)
        { 
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(Player.transform.position, 2);

            foreach (Collider2D coll in hitColliders)
            {
                if (coll.gameObject.tag == "Grabbable" && coll.gameObject.GetComponent<OrbitorObjectScript>() && coll.gameObject.GetComponent<OrbitorObjectScript>().isOrbiting == false)
                {
                    Projectile = coll.gameObject;
                    Projectile.transform.parent = transform;
                    //hasProjectile = true;
                    //Debug.Log(coll.gameObject.name);
                }
            }
        }
    }
}
