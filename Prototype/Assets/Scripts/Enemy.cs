using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    // Use this for initialization

    //components used
    private Animator anim;

    [Header("Attributes")]
    public int health;
    public float rangeofattack;
    public float range;
    public float damage;
    public float movspeed;
    public enum World
    {
        Hero, Demon
    }
    public World InWorld;
    public float losrange;

    [Header("Brain")]
    public GameObject target;
    private Vector3 targetpos;
    
    public enum States
    {
        Idle,
        Patrol,
        Alert,
        Pursuit,
        Attack,
        GetHit,
        Dead
    }
    [Header("States")]
    public States currentstate;

    [Header("Eyesight")]
    public GameObject vision;
    public RaycastHit eyehit;
    private RaycastHit vishit;


    void Start () {
		
        anim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        DetectingPlayer();
        Pursuit();
	}

    public void DetectingPlayer()
    {
        
        if(Physics.Raycast(vision.transform.position,transform.right,out eyehit, range))
        {
            Debug.Log("Player in range");
        }

        if(Physics.Raycast(vision.transform.position,transform.right,out vishit, losrange))
        {
            if(vishit.collider.gameObject.tag == "Player")
            {
                currentstate = States.Alert;
                anim.SetTrigger("Alert");
                target = vishit.collider.gameObject;
                targetpos = target.transform.position;
                Debug.Log("Player spotted");
            }

           
           
        }
        else
        {
            target = null;
            currentstate = States.Idle;
        }

    }

    public void Pursuit()
    {
        if (target)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, targetpos, movspeed * Time.deltaTime);
        }
    }

    public virtual void Attack()
    {
        //Each type of enemy will have its seperate attack
    }


    private void OnDrawGizmosSelected()
    {
        
        Gizmos.DrawWireSphere(this.transform.position,losrange);
        
    }
}
