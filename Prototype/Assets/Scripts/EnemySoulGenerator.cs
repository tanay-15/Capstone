using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoulGenerator : MonoBehaviour {

    public GameObject rageBar;
    public GameObject soulPrefab;
    public GameObject trailPrefab;
    public GameObject canvas;

    public AnimationCurve speedTo;
    public AnimationCurve slowTo;

    public static EnemySoulGenerator sharedInstance;

	void Start () {
        if (sharedInstance != null)
            Destroy(sharedInstance);
        sharedInstance = this;
	}

    public void GenerateSoul(Vector2 enemyPos)
    {
        StartCoroutine(Soul(enemyPos));
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
    }
}
