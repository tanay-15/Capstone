using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifebarPlacer : MonoBehaviour {
    public GameObject lifebarPrefab;
    Vector3 offset;
    //Enemy types
    //RagdollEnemy
    IEnumerable<BasicEnemy> enemies;
	void Start () {
        offset = new Vector3(0f, 1f, 0f);
        enemies = FindObjectsOfType<BasicEnemy>();
        foreach(BasicEnemy enemy in enemies)
        {
            Lifebar lifebar = (Instantiate(lifebarPrefab, enemy.gameObject.transform, true) as GameObject).GetComponent<Lifebar>();
            lifebar.gameObject.transform.position = enemy.gameObject.transform.position + offset;
            lifebar.AssignEnemy(enemy);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
