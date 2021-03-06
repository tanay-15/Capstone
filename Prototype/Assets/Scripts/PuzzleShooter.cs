﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleShooter : MonoBehaviour
{
    public GameObject projectile;

    // speed at which projectile fires
    public float speed;

    // rate of fire
    public float fireRate;

    public float fireDistance;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0.0f;

        projectile.GetComponent<PuzzleProjectile>().speed = speed;
        if(fireDistance == 0f)
        {
            fireDistance = 10.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer > fireRate)
        {
            timer = 0.0f;
            projectile.GetComponent<PuzzleProjectile>().distance = fireDistance;
            Instantiate(projectile,this.transform);
            
        }
    }
}
