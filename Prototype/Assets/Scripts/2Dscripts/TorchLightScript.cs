using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchLightScript : MonoBehaviour {

    GameObject player;
    Color color;

    float lerpTimer = 0;
    float prevRandom = 0;
    float nextRandom = 0;

	// Use this for initialization
	void Start () {

        player = GameObject.Find("Character");
        color = GetComponent<SpriteRenderer>().color;

        prevRandom = Random.Range(0, 0.25f);

    }
	
	// Update is called once per frame
	void Update () {

        if (player.GetComponent<DemonModeScript>().transitioning)
        {
            if (player.GetComponent<DemonModeScript>().DemonModeActive)
            {
                GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b,
                    GetComponent<SpriteRenderer>().color.a - 0.0025f);
                color.a -= 0.0025f;
            }
            else
            {
                GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 
                    GetComponent<SpriteRenderer>().color.a + 0.0025f);
                color.a += 0.0025f;
            }
        }


// lerping brightness
        lerpTimer += Time.deltaTime;

        float offset = Mathf.Lerp(prevRandom, nextRandom, lerpTimer * 4);
        GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b,
                color.a + offset);

        if (lerpTimer > 0.25f)
        {
            prevRandom = nextRandom;
            nextRandom = Random.Range(0, 0.25f);            
            lerpTimer = 0;
        }

    }
}
