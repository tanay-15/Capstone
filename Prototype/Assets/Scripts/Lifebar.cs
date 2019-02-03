using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifebar : MonoBehaviour {

    public GameObject redScalingSprite;
    public GameObject yellowScalingSprite;
    BasicEnemy myEnemy;

    float yellowScale;
    float targetScale;
    float drainSpeed = 0.5f;

	public void AssignEnemy(BasicEnemy enemy)
    {
        myEnemy = enemy;
        myEnemy.events.OnDeath.AddListener(OnEnemyDeath);
        myEnemy.events.OnTakeDamage.AddListener(OnEnemyTakeDamage);
        targetScale = 1f;
        yellowScale = targetScale;
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
        targetScale = newHealth;
        redScalingSprite.transform.localScale = new Vector3(targetScale, 1f, 1f);
    }

    void Update()
    {
        if (yellowScale > targetScale)
            yellowScale -= Time.deltaTime * drainSpeed;
        yellowScalingSprite.transform.localScale = new Vector3(yellowScale, 1f, 1f);
    }
}
