using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hive : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject spawnLocation;
    public GameObject bee_prefab;

    private Animator anim;

    public float spawn_counter = 0f;
    private bool shouldspawn = false;

    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        spawn_counter = spawn_counter - Time.deltaTime;

        if(spawn_counter <= 0)
        {
            shouldspawn = true;
            spawn_counter = 3f;
        }
    }

    public void SpawnBee()
    {
        anim.SetTrigger("HiveSpawn");
        Instantiate(bee_prefab, spawnLocation.transform.position, spawnLocation.transform.rotation);
        shouldspawn = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (shouldspawn)
            {
               
                SpawnBee();
            }
        }
    }
}
