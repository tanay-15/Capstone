using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPoint : MonoBehaviour
{
    public GameObject particlesPrefab;
    public SpriteRenderer white;
    public Light myLight;
    float count;
    float amplitude = 0.15f;
    float haloBaseScale = 1.3f;
    float haloAmplitude = 0.4f;
    float countScale = 3f;
    Color newColor;
    void Start()
    {
        newColor = Color.white;
    }

    void Update()
    {
        count += Time.deltaTime;
        newColor.a = amplitude + amplitude * -Mathf.Cos(count * countScale);
        myLight.range = haloBaseScale + (haloAmplitude * 0.5f) + (haloAmplitude * 0.5f) * -Mathf.Cos(count * countScale);  //1.3 to 1.7
        white.color = newColor;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SkillTree.info.skillPoints++;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        GameObject particles = Instantiate(particlesPrefab, transform.position, transform.rotation);
        Destroy(particles, 2f);
    }
}
