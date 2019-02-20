using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleShooter : MonoBehaviour
{
    public GameObject projectile;

    // speed at which projectile fires
    public float speed;

    // rate of fire
    public float fireRate;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;

        projectile.GetComponent<PuzzleProjectile>().speed = speed;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > fireRate)
        {
            timer = 0.0f;
            Instantiate(projectile,this.transform);
            
        }
    }
}
