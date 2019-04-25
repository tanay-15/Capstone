using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lifebar : MonoBehaviour {

    public GameObject sprites;
    public GameObject redScalingSprite;
    public GameObject yellowScalingSprite;
    BasicEnemy myEnemy;

    float yellowScale;
    float targetScale;
    float drainSpeed = 0.5f;
    float yOffset;

    Vector3 normalScale;
    Vector3 flipScale;
    Vector3 offset;

    void Start()
    {
        normalScale = transform.localScale;
        flipScale = normalScale;
        flipScale.x *= -1f;
        yOffset = transform.localPosition.y;
        offset = new Vector3(0f, yOffset, -5f);
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
        if (targetScale == Mathf.Infinity || targetScale.Equals(null))
        {
            targetScale = 0.0f;
        }
            redScalingSprite.transform.localScale = new Vector3(targetScale, 1f, 1f);
        
    }

    void LateUpdate()
    {
        sprites.SetActive(myEnemy.enableLifebar);
        if (yellowScale > targetScale)
            yellowScale -= Time.deltaTime * drainSpeed;
        yellowScalingSprite.transform.localScale = new Vector3(yellowScale, 1f, 1f);

        transform.localScale = (myEnemy.gameObject.transform.localScale.x < 0) ? flipScale : normalScale;

        //Keep lifebars upright on the enemy
        transform.rotation = Quaternion.identity;
        transform.position = transform.parent.position + transform.parent.localScale.y * offset;
    }
}
