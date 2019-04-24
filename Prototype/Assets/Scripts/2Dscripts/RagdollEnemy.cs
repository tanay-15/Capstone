using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollEnemy : BasicEnemy {

    Collider2D[] playerColliders;
    Collider2D[] myColliders;
    SpriteRenderer[] mySpriteRenderers;

    Component[] bones;

    public bool fadeAfterDeath = true;
    

    public bool isDead = false;

   

    void Start ()
    {

        bones = gameObject.transform.GetComponentsInChildren<Rigidbody2D>();
        mySpriteRenderers = GetComponentsInChildren<SpriteRenderer>();

        SetChildrenKinematic(true);
    }
	

	void Update () {

	}

    public void SetChildrenKinematic(bool state)
    {
        foreach (Rigidbody2D rb2d in bones)
        {
            if(rb2d.name != gameObject.name)
            {
                rb2d.isKinematic = state;
                rb2d.GetComponent<Collider2D>().enabled = !state;
                rb2d.AddForce(Vector2.up * 5,ForceMode2D.Impulse);
                rb2d.AddTorque(-15);
                //Debug.Log(rb2d.name);
            }
                
        }
    }

    public void TurnOffCollisions(bool isTrue)
    {
        //playerColliders = Movement2D.sharedInstance.GetComponentsInChildren<Collider2D>();
        playerColliders = FindObjectOfType<PlayerStates>().GetComponentsInChildren<Collider2D>();
        myColliders = GetComponentsInChildren<Collider2D>();

        foreach (Collider2D playerCol in playerColliders)
        {
            foreach (Collider2D myCol in myColliders)
            {
                Physics2D.IgnoreCollision(playerCol, myCol, isTrue);
            }
        }
    }

    IEnumerator DieRoutine()
    {
        TurnOffCollisions(true);

        yield return new WaitForSeconds(2f);

        float speed = 2.5f;
        Color newColor = Color.white;
        for (float i = 0f; i < 1f; i += Time.deltaTime * speed)
        {
            foreach (SpriteRenderer spr in mySpriteRenderers)
            {
                newColor.a = 1f - i;
                spr.color = newColor;
            }
            yield return 0;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "projectile" || collision.gameObject.tag == "Grabbable" || collision.gameObject.name == "AttackTrigger" || collision.gameObject.name == "StompTrigger")
        {
            if (fadeAfterDeath)
                StartCoroutine(DieRoutine());
            else
                TurnOffCollisions(true);
            events.OnDeath.Invoke();
            isDead = true;
            //Debug.Log("hit");
            SetChildrenKinematic(false);
            GetComponents<BoxCollider2D>()[0].enabled = false;
            GetComponents<BoxCollider2D>()[1].enabled = false;

            if (GetComponent<AudioSource>())
                GetComponent<AudioSource>().Play();
        }

    }
}
