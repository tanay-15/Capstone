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
    private float crateTimer;

    public GameObject healthPotionPrefab;
    public GameObject arrowDropPrefab;

    public GameObject skullPrefab;


    public GameObject location1;
    public GameObject location2;
    public GameObject location3;
    public GameObject location4;

    public GameObject crateLocation1;
    public GameObject crateLocation2;

    public GameObject cratePrefab;

    private bool spawnCrates = false;

    private int randvalue;
    private bool pickupsSpawn = false;

    public GameObject Picks;
    public GameObject Crates1;
    public GameObject Crates2;


    void Start()
    {
        bosshealthBar.SetActive(false);
        boss.SetActive(false);

        bossHealthBarPhase1.SetActive(false);
        bossPhase1.SetActive(false);
        crateTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (HasPhase1Begun)
        {
            bossHealthBarPhase1.GetComponent<Slider>().value = bossPhase1.GetComponent<Enemy>().GetHealth();
            jumpTimer = jumpTimer - Time.deltaTime;
            crateTimer = crateTimer - Time.deltaTime;
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


        if (jumpTimer <= 0 && skullPrefab != null)
        {
            skullPrefab = Instantiate(jumpEnemy, jumpEnemPosition.transform.position, Quaternion.identity);

            jumpTimer = 10f;
        }

        if (!spawnCrates && Crates1 == null && Crates2 == null && crateTimer <=0)
        {
            SpawnCrates();
            spawnCrates = true;
            crateTimer = 10f;
        }

        if(Picks == null && !pickupsSpawn)
        {
            RandomGenerator();
            pickupsSpawn = true;
            StartCoroutine(PickDes());
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


    public void SpawnCrates()
    {
       Crates1 = Instantiate(cratePrefab, crateLocation1.transform.position, Quaternion.identity);
       Crates2 = Instantiate(cratePrefab, crateLocation2.transform.position, Quaternion.identity);
        spawnCrates = true;
    }

    public void RandomGenerator()
    {
        randvalue = (int)Random.Range(1, 2);
        SpawnerPickUp(randvalue);
    }

    public void SpawnerPickUp(int value)
    {
        int locationRandom;

        locationRandom = (int)Random.Range(1, 3);

        if(value == 1)
        {
            if(locationRandom == 1)
            {
                Picks = Instantiate(healthPotionPrefab, location1.transform.position, Quaternion.identity);
                pickupsSpawn = true;
            }
            if(locationRandom == 2)
            {
                Picks = Instantiate(healthPotionPrefab, location2.transform.position, Quaternion.identity);
                pickupsSpawn = true;
            }
            if(locationRandom == 3)
            {
               Picks =  Instantiate(healthPotionPrefab, location3.transform.position, Quaternion.identity);
                pickupsSpawn = true;
            }
            
        }

        if(value == 2)
        {
            if (locationRandom == 1)
            {
                Picks = Instantiate(arrowDropPrefab, location1.transform.position, Quaternion.identity);
                pickupsSpawn = true;
            }
            if (locationRandom == 2)
            {
               Picks = Instantiate(arrowDropPrefab, location2.transform.position, Quaternion.identity);
                pickupsSpawn = true;
            }
            if (locationRandom == 3)
            {
                Picks = Instantiate(arrowDropPrefab, location3.transform.position, Quaternion.identity);
                pickupsSpawn = true;
            }
        }
    }

    IEnumerator PickDes()
    {
        yield return new WaitForSeconds(8f);
        Destroy(Picks.gameObject);
        pickupsSpawn = false;
    }
   
}
