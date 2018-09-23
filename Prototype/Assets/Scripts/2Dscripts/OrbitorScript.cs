using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitorScript : MonoBehaviour {

    public GameObject Projectile;
    public Camera MainCam;

    public float speed = 1.0f;
    public float radius = 2.0f;
    public float theta = 0;

    bool hasProjectile = false;
    bool shootingProjectile = false;

    private float startTime;
    private float journeyLength;
    


    void Start () {
        startTime = Time.time;

        journeyLength = Vector3.Distance(transform.position, Projectile.transform.position);
    }


    void Update()
    {

// flags before picking up object
        if (!Projectile.GetComponent<OrbitorObjectScript>().isOrbiting && Vector3.Distance(transform.parent.position, Projectile.transform.position) < 3f && !shootingProjectile)
        {
            Projectile.GetComponent<OrbitorObjectScript>().isOrbiting = true;
            Projectile.GetComponent<OrbitorObjectScript>().hit = false;
            Projectile.GetComponent<Collider2D>().enabled = false;
            Projectile.GetComponent<Rigidbody2D>().gravityScale = 0;

            startTime = Time.time;
        }


        if (Projectile.GetComponent<OrbitorObjectScript>().hit == true)
            shootingProjectile = false;

// Lerping 
        if (!shootingProjectile && Vector3.Distance(transform.parent.position, Projectile.transform.position)<3f)
        {
            float distCovered = (Time.time - startTime) * speed;

            float fracJourney = distCovered / journeyLength;

            Projectile.transform.position = Vector3.Lerp(Projectile.transform.position, transform.position, fracJourney);
        }



// Orbiting path
        theta += 2 * Time.deltaTime;
        transform.localPosition = new Vector3(radius * Mathf.Sin(theta), radius * Mathf.Cos(theta), 0);


// Throw to mouse click position
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = MainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.point);

                if (transform.localPosition.x > 1f && !shootingProjectile && transform.localPosition.y > -0.3f)
                {
                    Vector3 direction = Vector3.Normalize(new Vector3(hit.point.x, hit.point.y, 0) - Projectile.transform.position);
                    Projectile.GetComponent<Rigidbody2D>().velocity = 10 * direction;
                    Projectile.GetComponent<Collider2D>().enabled = true;

                    shootingProjectile = true;
                    Projectile.GetComponent<OrbitorObjectScript>().isOrbiting = false;
                }
                
            }
        }


       
    }

    private void FixedUpdate()
    {

        //Collider2D[] hitColliders = Physics2D.OverlapCircle(center, radius);

    }
}
