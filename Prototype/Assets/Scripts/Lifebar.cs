using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifebar : MonoBehaviour {

    public GameObject scaledLifebar;
    BasicEnemy myEnemy;

	public void AssignEnemy(BasicEnemy enemy)
    {
        myEnemy = enemy;
        myEnemy.events.OnDeath.AddListener(OnEnemyDeath);
        myEnemy.events.OnTakeDamage.AddListener(OnEnemyTakeDamage);
    }

    void OnEnemyDeath()
    {
        myEnemy.events.OnDeath.RemoveListener(OnEnemyDeath);
        myEnemy.events.OnTakeDamage.RemoveListener(OnEnemyTakeDamage);
        Destroy(gameObject);
    }

    void OnEnemyTakeDamage()
    {

    }
}
