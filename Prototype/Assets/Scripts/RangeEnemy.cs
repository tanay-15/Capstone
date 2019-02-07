using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeEnemy : Enemy {


    [Header("Range details")]
    public GameObject shurikenprefab;
    public GameObject shuriloct;
    private GameObject go;
    private bool IsPunched;
    private bool Thrown = false;

    

    public override Vector3 lifebarOffset
    {
        get
        {
            return Vector3.up * 2f;
        }
    }

	// Use this for initialization
	void Start () {

        maxHealth = health;
        anim = this.GetComponent<Animator>();
        rigi = this.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {

        if (IsAlive)
        {
            if (target != null)
            {
                rateofattack = rateofattack - Time.deltaTime;
                CheckForPlayerPosition();
                if (rateofattack <= 1f)
                {
                    Thrown = false;
                }
                if (!Thrown && rateofattack <= 0)
                {

                    Debug.Log("Found Target now attack him");
                    ThrowAxe();
                }

            }
        }
        if(health <=0)
        {
            Death();
        }
    }

    public void CheckForPlayerPosition()
    {
        if(target.transform.position.x < this.transform.position.x)
        {
            //check where it is looking
            //look left
            if (LookingLeft)
            {
                //ignore
            }
            if (!LookingLeft)
            {
                flip();
            }

        }

        if(target.transform.position.x > this.transform.position.x)
        {
            //check where it is looking
            //Look right

            if (LookingLeft)
            {
                flip();
            }

            if (!LookingLeft)
            {
                //ignore
            }
        }
    }

    public override void applyDamage(int damage)
    {

        this.health = this.health - damage;

        events.OnTakeDamage.Invoke((float)this.health / (float)maxHealth);



    }

    public void ThrowAxe()
    {
        Instantiate(shurikenprefab, shuriloct.transform.position, shurikenprefab.transform.rotation);
        rateofattack = 3f;
        Thrown = true;
    }


  
    public override void Pursuit()
    {

        if (target && !AttackReady)
        {
            currentstate = States.Pursuit;
            anim.SetBool("ShouldPursuit", true);
            this.transform.position = Vector2.MoveTowards(this.transform.position, targetpos, movspeed * Time.deltaTime);


            if(Vector2.Distance(this.transform.position,target.transform.position) <= 3f)
            {
                targetpos = target.transform.position;
                AttackReady = true;
            }
        }

    }

   public  void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "projectile" || collision.gameObject.tag == "Grabbable") && Vector3.Distance(collision.transform.position, transform.position) < 2f)
        {

            if(collision.GetType() == typeof(BoxCollider2D))
            {
                applyDamage(5);
                if (collision.GetComponent<OrbitorObjectScript>())
                    collision.GetComponent<OrbitorObjectScript>().hit = true;
                else if (collision.GetComponent<playerKnife2D>())
                    collision.GetComponent<playerKnife2D>().hasHit = true;

                if (health <= 0)
                    IsAlive = false;

                var impact = Instantiate(ImpactAnim, collision.transform.position, Quaternion.identity);
                impact.gameObject.SetActive(true);
                Destroy(collision.gameObject);
            }


        }

        if (collision.gameObject.tag == "Player")
        {
            target = collision.gameObject;
            targetpos = collision.gameObject.transform.position;
            anim.SetBool("Attack", true);
            currentstate = States.Attack;
            AttackReady = true;
            Debug.Log("Enemy attack ready");
        }
    }

    public override void TriggerEnter2D(Collider2D collision)
    {
        if ((/*collision.gameObject.tag == "projectile" || */collision.gameObject.tag == "Grabbable") && Vector3.Distance(collision.transform.position, transform.position) < 1.5f)
        {
            if (collision.GetType() == typeof(BoxCollider2D))
            {

                applyDamage(Random.Range(5, 8));
                if (collision.GetComponent<OrbitorObjectScript>())
                    collision.GetComponent<OrbitorObjectScript>().hit = true;
                else if (collision.GetComponent<playerKnife2D>())
                    collision.GetComponent<playerKnife2D>().hasHit = true;
                //


                if (health > 0.0f)
                {
                    //Destroy(collision.gameObject);
                }

                var impact = Instantiate(ImpactAnim, collision.transform.position, Quaternion.identity);
                impact.gameObject.SetActive(true);

            }
        }

        if (collision.gameObject.name == "AttackTrigger" && !IsPunched && Vector3.Distance(collision.transform.position, transform.position) < 1.5f)
        {
            applyDamage(5);

            IsPunched = true;
            StartCoroutine(IsPunchedReset());

            var impact = Instantiate(ImpactAnim, new Vector2(transform.position.x, transform.position.y + 0.8f), Quaternion.identity);
            impact.gameObject.SetActive(true);
        }

        if (collision.gameObject.tag == "Player")
        {

            
            target = collision.gameObject;


        }
    }


    public override void OnTriggerExit2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            target = null;
            anim.SetBool("Attack", false);
            AttackReady = false;
            currentstate = States.Idle;
        }
    }

    public Vector2 GetTargetpos()
    {
        return targetpos;
    }

    public GameObject GetTarget()
    {
        return target;
    }

    IEnumerator IsPunchedReset()
    {
        yield return new WaitForSeconds(0.25f);
        IsPunched = false;
    }

}
