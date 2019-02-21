using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// lutem's orb puzzle component, starting position

public class OrbPuzzle_Start : MonoBehaviour
{

    public GameObject orbPrefab;
    public float timeAlive;

    bool orbPresent;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        orbPrefab.GetComponent<CircleCollider2D>().isTrigger = true;

        Instantiate(orbPrefab, this.transform);
        orbPresent = true;
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!orbPresent)
        {

            timer += Time.deltaTime;
            if (timeAlive < timer)
            {
                if (this.transform.childCount > 0)
                {
                    Destroy(this.transform.GetChild(0).gameObject);
                }
                Instantiate(orbPrefab, this.transform);
                orbPresent = true;
                timer = 0f;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("Orb"))
        {
            orbPresent = false;
            if (this.transform.childCount > 0)
            {
                this.transform.GetChild(0).GetComponent<CircleCollider2D>().isTrigger = false;
            }
        }
    }
}
