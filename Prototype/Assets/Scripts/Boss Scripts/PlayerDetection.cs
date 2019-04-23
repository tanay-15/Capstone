using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetection : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject bosshealthBar;
    public GameObject boss;

    public GameObject bossHealthBarPhase1;
    public GameObject bossPhase1;

    public bool HasPhase1Begun = false;
    public bool BeginPhase2 = false;

    public GameObject jumpEnemy;
    public GameObject jumpEnemPosition;

    public float jumpTimer = 3;

    void Start()
    {
        bosshealthBar.SetActive(false);
        boss.SetActive(false);

        bossHealthBarPhase1.SetActive(false);
        bossPhase1.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (HasPhase1Begun)
        {
            bossHealthBarPhase1.GetComponent<Slider>().value = bossPhase1.GetComponent<Enemy>().GetHealth();
            jumpTimer = jumpTimer - Time.deltaTime;
            if (bossPhase1 == null || bossPhase1.GetComponent<Enemy>().GetHealth() <= 0)
            {
                BeginPhase2 = true;
                HasPhase1Begun = false;
                Destroy(bossHealthBarPhase1);
            }
        }

        if (BeginPhase2)
        {
            Invoke("NextStage", 3f);
        }


        if (jumpTimer <= 0)
        {
            Instantiate(jumpEnemy, jumpEnemPosition.transform.position, Quaternion.identity);

            jumpTimer = 10f;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((collision.gameObject.tag == "Player") && !HasPhase1Begun)
        {
            bossHealthBarPhase1.SetActive(true);
            bossPhase1.SetActive(true);
            HasPhase1Begun = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            HasPhase1Begun = true;
        }
    }

    public void NextStage()
    {
        bosshealthBar.SetActive(true);
        boss.SetActive(true);
        Destroy(this.gameObject);
    }

   
}
