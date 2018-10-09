using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Manager : MonoBehaviour {

    // Use this for initialization

    public GameObject crate_prefab;
    public GameObject[] current_crates;

    public GameObject door;
    public GameObject boss;
    private int boss_health;

	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        current_crates = GameObject.FindGameObjectsWithTag("Grabbable");

        if(current_crates.Length == 0)
        {
            Instantiate(crate_prefab, this.transform.position, this.transform.rotation);
        }
        
        if(boss !=null)
        boss_health = (int)boss.GetComponent<Boss1>().GetHealth();

        if(boss_health <= 0)
        {
            door.SetActive(true);
        }

        
	}
}
