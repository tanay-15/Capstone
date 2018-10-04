using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonModeScript : MonoBehaviour {

    public GameObject bat;
    public bool DemonModeActive;
    public bool transitioning;
    public Camera MainCam;
    public GameObject DemonCircle;
    bool DemonCircleSpawn = false;

     float demonCircleLerp = -0.8f;
    float demonCirclePrevSize = 0.05f;
    float demonCircleFinalSize = 0.5f;

    // Use this for initialization
    void Start () {
        DemonModeActive = false;
        transitioning = false;       
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButtonDown("Fire2") && !transitioning && GetComponent<rageBar>().fillAmount >= 1)
        {
            Transformation();
        }

        if (transitioning)
        {
            if(!DemonModeActive)
            { 
                MainCam.backgroundColor = new Color(MainCam.backgroundColor.r-0.001f, MainCam.backgroundColor.g - 0.002f, MainCam.backgroundColor.b - 0.001f, MainCam.backgroundColor.a);
                Color darkness = GameObject.Find("Darkness").GetComponent<SpriteRenderer>().color;
                GameObject.Find("Darkness").GetComponent<SpriteRenderer>().color = new Color(darkness.r, darkness.g,darkness.b, darkness.a + 0.001f);

                DemonCircleSpawn = true;

            }
            else
            {
                MainCam.backgroundColor = new Color(MainCam.backgroundColor.r + 0.001f, MainCam.backgroundColor.g + 0.002f, MainCam.backgroundColor.b + 0.001f, MainCam.backgroundColor.a);
                Color darkness = GameObject.Find("Darkness").GetComponent<SpriteRenderer>().color;
                GameObject.Find("Darkness").GetComponent<SpriteRenderer>().color = new Color(darkness.r, darkness.g, darkness.b, darkness.a - 0.001f);
            }
        }

        if(DemonModeActive)
            GetComponent<Movement2D>().speed = 7;
        else
            GetComponent<Movement2D>().speed = 5;


//Demon Circle effect
        if (DemonCircleSpawn)
        {
            if (demonCircleLerp >= 0)
                DemonCircle.SetActive(true);

            demonCircleLerp += 0.4f * Time.deltaTime;
            float temp = Mathf.Lerp(demonCirclePrevSize, demonCircleFinalSize, demonCircleLerp);
            DemonCircle.transform.localScale = new Vector3(demonCirclePrevSize, demonCirclePrevSize, 1);
            DemonCircle.transform.localScale = new Vector3(temp, temp, 1);
            Color color = DemonCircle.GetComponent<SpriteRenderer>().color;
            DemonCircle.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1 - demonCircleLerp);

            if (demonCircleLerp >= 1)
            {
                DemonCircle.transform.localScale = new Vector3(demonCirclePrevSize, demonCirclePrevSize, 1);
                DemonCircle.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1 );
                demonCircleLerp = -0.8f;
                DemonCircleSpawn = false;
                DemonCircle.SetActive(false);
            }
        }
    }

    public void Transformation()
    {
        transitioning = true;
        StartCoroutine("SpriteTransform", !DemonModeActive);
        for (int i = 0; i < 50; i++)
        {
            var Bat = Instantiate(bat, transform.position + new Vector3(0, 100, 0), Quaternion.identity);
            Bat.GetComponent<Bat_Script>().player = gameObject;
        }

        if (!DemonModeActive)
        {
            GetComponent<AudioSource>().Play();
        }
    }


    IEnumerator SpriteTransform(bool toDemon)
    {
        yield return new WaitForSeconds(2.7f);

        transform.GetComponent<Movement2D>().myAnim = (toDemon) ? transform.Find("Demon").GetComponent<Animator>() : transform.Find("Normal").GetComponent<Animator>();
        transform.Find("Normal").gameObject.SetActive(!toDemon);
        transform.Find("Demon").gameObject.SetActive(toDemon);
        transitioning = false;
        DemonModeActive = toDemon;
    }

   
}
