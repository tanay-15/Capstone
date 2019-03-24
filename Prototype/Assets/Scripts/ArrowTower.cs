using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowTower : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject target;
    private Vector3 targetpos;

   
    public GameObject leftshootpoint;

    public float health = 20;

    public float attackCounter = 0;
    public GameObject arrowPrefab;
    private bool InRange = false;

    public GameObject SkeletonPrefab;
    public GameObject skeletonspawn;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (this.health <= 0f)
        {
            Instantiate(SkeletonPrefab, skeletonspawn.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        attackCounter = attackCounter - Time.deltaTime;

        if (attackCounter <= 0f && InRange)
        {
            Attack();
            attackCounter = 3f;
        }
    }

    public void Attack()
    {
        Instantiate(arrowPrefab, leftshootpoint.transform.position, Quaternion.identity);
    }

    public void TriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            target = collision.gameObject;
            InRange = true;

        }

        
    }

    public void TriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            target = null;
            InRange = false;
        }
    }

    public void HitTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.gameObject.tag == "projectile") { 
            this.health = this.health - 5f;
        }
    }
}
