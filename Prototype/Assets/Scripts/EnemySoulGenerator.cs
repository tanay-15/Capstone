using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoulGenerator : MonoBehaviour {

    public GameObject rageBar;
    public GameObject soulPrefab;
    public GameObject trailPrefab;
    public GameObject arrowPickupPrefab;
    public GameObject healthPickupPrefab;
    public GameObject canvas;

    public AnimationCurve speedTo;
    public AnimationCurve slowTo;

    public static EnemySoulGenerator sharedInstance;

    float DropChance
    {
        get
        {
            return ((SkillTree.info.nodesActivated & SkillNodes.H_3) == SkillNodes.H_3) ? 0.6f : 0.3f;
        }
    }

	void Start () {

        if (sharedInstance != null)
            Destroy(sharedInstance);
        sharedInstance = this;
        Physics2D.IgnoreLayerCollision(19,15);
	}

    public void GenerateSoul(Vector2 enemyPos)
    {
        StartCoroutine(Soul(enemyPos));

        //Drop arrows
        //TODO: Chance of arrows generating should be handled by the individual enemies
        if (Random.Range(0, 1) <= DropChance)
        {
            GameObject pickupPrefab = Random.Range(0, 2) == 0 ? arrowPickupPrefab : healthPickupPrefab;
            var pickup = Instantiate(pickupPrefab, (Vector3)enemyPos, Quaternion.identity);
            pickup.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        }

        //if (Random.Range(0, 4) == 2)
        //{
        //    var potion = Instantiate(healthPickupPrefab, (Vector3)enemyPos, Quaternion.identity);
        //    potion.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        //}

    }

    IEnumerator Soul(Vector2 enemyPos)
    {
        Vector2 startPos = Camera.main.WorldToScreenPoint(enemyPos);
        Vector3 trailPos = Vector3.zero;
        GameObject soul = Instantiate(soulPrefab, startPos, Quaternion.identity, canvas.transform);
        GameObject trail = Instantiate(trailPrefab);

        Vector2 moveTo = startPos + Vector2.up * 100f;// Random.insideUnitCircle * 200f;
        for (float i = 0f; i < 1f; i += Time.deltaTime*2f)
        {
            soul.transform.position = Vector2.Lerp(startPos, moveTo, slowTo.Evaluate(i));
            trailPos = Camera.main.ScreenToWorldPoint(soul.transform.position);
            trailPos.z = -2f;
            trail.transform.position = trailPos;
            yield return 0;
        }
        soul.transform.position = moveTo;

        startPos = soul.transform.position;
        moveTo = rageBar.transform.position;
        for (float i = 0f; i < 1f; i += Time.deltaTime*2f)
        {
            soul.transform.position = Vector2.Lerp(startPos, moveTo, speedTo.Evaluate(i));
            trailPos = Camera.main.ScreenToWorldPoint(soul.transform.position);
            trailPos.z = -2f;
            trail.transform.position = trailPos;
            yield return 0;
        }
        soul.transform.position = moveTo;
        Destroy(soul);
        Destroy(trail, 1f);

        FindObjectOfType<rageBar>().AddRage(0.2f);
    }
}
