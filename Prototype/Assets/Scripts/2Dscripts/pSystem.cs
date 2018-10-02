using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pSystem : MonoBehaviour
{
    public ParticleSystem particleLeft;
    public ParticleSystem particleRight;
    // Use this for initialization
    void Start()
    {
        particleLeft = GetComponent<ParticleSystem>();
        particleRight = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag != null)
        {
            particleLeft.Play();
            particleRight.Play();
        }
    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.tag != null)
        {
            particleLeft.Stop();
            particleRight.Stop();
        }
    }
}