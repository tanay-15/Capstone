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
    float yOffset;

    Vector3 normalScale;
    Vector3 flipScale;

    void Start()
    {
        normalScale = transform.localScale;
        flipScale = normalScale;
        flipScale.x *= -1f;
        yOffset = transform.localPosition.y;
        Debug.Log(yOffset);
    }

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

    void LateUpdate()
    {
        if (yellowScale > targetScale)
            yellowScale -= Time.deltaTime * drainSpeed;
        yellowScalingSprite.transform.localScale = new Vector3(yellowScale, 1f, 1f);

        transform.localScale = (myEnemy.gameObject.transform.localScale.x < 0) ? flipScale : normalScale;

        //Keep lifebars upright on the enemy
        transform.rotation = Quaternion.identity;
        transform.position = transform.parent.position + Vector3.up * transform.parent.localScale.y * yOffset;
    }
}
