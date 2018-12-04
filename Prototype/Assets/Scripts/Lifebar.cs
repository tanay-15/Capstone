﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifebar : MonoBehaviour {

    public GameObject scaledLifebar;
    BasicEnemy myEnemy;

	public void AssignEnemy(BasicEnemy enemy)
    {
        Debug.Log(enemy == null);
        myEnemy = enemy;
        myEnemy.events.OnDeath.AddListener(OnEnemyDeath);
        myEnemy.events.OnTakeDamage.AddListener(OnEnemyTakeDamage);
    }

    void OnEnemyDeath()
    {
        myEnemy.events.OnDeath.RemoveListener(OnEnemyDeath);
        myEnemy.events.OnTakeDamage.RemoveListener(OnEnemyTakeDamage);
        Destroy(gameObject);
        EnemySoulGenerator.sharedInstance.GenerateSoul(myEnemy.gameObject.transform.position + Vector3.up);
    }

    void OnEnemyTakeDamage(float newHealth)
    {
        scaledLifebar.transform.localScale = new Vector3(newHealth, 1f, 1f);
    }
}
