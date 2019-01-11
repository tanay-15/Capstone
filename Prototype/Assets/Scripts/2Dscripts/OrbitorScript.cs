using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitorScript : MonoBehaviour {

    GameObject Player;
    GameObject Trail;
    public GameObject Projectile;
    //public Camera MainCam;

    public float speed = 1.0f;
    public float radius = 2.0f;
    public float theta = 0;
    int rotDirection = 1;

    public Material OriginalTrailColor;
    public Material HighlightTrailColor;

    //public bool hasProjectile = false;
    //public bool shootingProjectile = false;

    //private float startTime = 0;
    //private float journeyLength = 0;
    public float fracJourney = 0;
    bool facingRight = true;


    public enum State
    {
        NoProjectile ,
        HasProjectile ,
        ShootingProjectile 
    };

    public State Status;

    void Start () {

        Player = transform.parent.parent.gameObject;
        Trail = transform.GetChild(0).gameObject;

        Status = State.NoProjectile;

        Trail.GetComponent<TrailRenderer>().material = OriginalTrailColor;
    }


    void Update()
    {

        if (Player.GetComponent<DemonModeScript>().DemonModeActive)
        {

            // Orbiting path

            theta += 2 * Time.deltaTime;
            transform.localPosition = new Vector3(radius * Mathf.Sin(theta) * rotDirection, radius * Mathf.Cos(theta), -1);
           

            

            switch (Status)
            {
                case State.NoProjectile:
                    {
                        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(Player.transform.position, 3);

                        foreach (Collider2D coll in hitColliders)
                        {
                            if (coll.gameObject.tag == "Grabbable" && coll.gameObject.GetComponent<OrbitorObjectScript>() 
                                && coll.gameObject.GetComponent<OrbitorObjectScript>().hit == true
                                && coll.gameObject.GetComponent<OrbitorObjectScript>().isOrbiting == false 
                                && transform.childCount<2)
                            {
                                Projectile = coll.gameObject;
                                Projectile.transform.parent = transform;

                                Projectile.GetComponent<OrbitorObjectScript>().isOrbiting = true;
                                Projectile.GetComponent<OrbitorObjectScript>().hit = false;
                                Projectile.GetComponent<Collider2D>().enabled = false;
                                Projectile.GetComponent<Rigidbody2D>().gravityScale = 0;

                                if (transform.childCount > 1)
                                {
                                    Status = State.HasProjectile;
                                }
                            }
                        }
                        break;
                    }





                case State.HasProjectile:
                    {
                        Trail.SetActive(true);
                        Trail.transform.position = Projectile.transform.position;
                        Trail.GetComponent<TrailRenderer>().material = OriginalTrailColor;

                        // Lerping 
                        if (Projectile.GetComponent<OrbitorObjectScript>().isOrbiting)
                        {
                            fracJourney += Time.deltaTime;

                            Projectile.transform.position = Vector3.Lerp(Projectile.transform.position, Projectile.transform.parent.position, fracJourney);


                        }


                        

                        break;
                    }



                case State.ShootingProjectile:
                    {
                        Trail.transform.position = Projectile.transform.position- new Vector3(0,0,0.1f);

                        if (Projectile.GetComponent<OrbitorObjectScript>().hit)
                        {
                            Projectile.GetComponent<OrbitorObjectScript>().isOrbiting = false;
                            Status = State.NoProjectile;
                            Trail.SetActive(false);
                        }

                        break;
                    }
            }


        }


        else
        {
            if (Status == State.HasProjectile && Projectile.GetComponent<OrbitorObjectScript>().hit == false)
            {
                Projectile.transform.position = Player.transform.position + new Vector3(0, 1, 0);
                Projectile.transform.parent = null;
                Projectile.GetComponent<OrbitorObjectScript>().hit = true;
                Projectile.GetComponent<OrbitorObjectScript>().isOrbiting = false;
                Projectile.GetComponent<Collider2D>().enabled = true;
                Projectile.GetComponent<Rigidbody2D>().gravityScale = 0.4f;              
                Status = State.NoProjectile;
            }
            else if (Status == State.ShootingProjectile)
            {
                Status = State.NoProjectile;
            }
            Trail.SetActive(false);
        }



    }



}
