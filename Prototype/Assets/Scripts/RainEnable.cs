using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainEnable : MonoBehaviour
{
    bool lightning;
    float timer;
    float delay;

    // Start is called before the first frame update
    void Start()
    {
        lightning = false;
        timer = 0.0f;
        delay = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (lightning)
        {
            GameObject light = Camera.main.transform.GetChild(2).gameObject;
            timer += Time.deltaTime;
            if (light.GetComponent<SpriteRenderer>().enabled)
            {
                light.GetComponent<SpriteRenderer>().color = new Color(light.GetComponent<SpriteRenderer>().color.r, light.GetComponent<SpriteRenderer>().color.g,
                    light.GetComponent<SpriteRenderer>().color.b, light.GetComponent<SpriteRenderer>().color.a - 0.3f * Time.deltaTime);
                if(light.GetComponent<SpriteRenderer>().color.a <= 0.0f)
                {
                    light.GetComponent<SpriteRenderer>().enabled = false;
                    light.GetComponent<SpriteRenderer>().color = new Color(light.GetComponent<SpriteRenderer>().color.r, light.GetComponent<SpriteRenderer>().color.g,
                    light.GetComponent<SpriteRenderer>().color.b, 0.5f);
                }
            }
            else
            {
                if (timer > delay)
                {
                    light.GetComponent<SpriteRenderer>().enabled = true;
                    delay = Random.value * 7.0f + 3.0f;
                    timer = 0.0f;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Camera.main.transform.GetChild(1).gameObject.SetActive(true);
            lightning = true;
        }
    }
}
