using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodWellScript : MonoBehaviour {

    bool contactPlayer;
    rageBar rb;
    float damageTimer = 0.0f;
    public int scale = 0;
    const float lifeDrainRate = 0.04f;

	// Use this for initialization
	void Start () {
        contactPlayer = false;
        rb = FindObjectOfType<rageBar>();
	}
	
	// Update is called once per frame
	void Update () {
        if (contactPlayer)
        {
            if (rb.RBar.fillAmount <= 1)
            {
                rb.RBar.fillAmount += 0.2f * Time.deltaTime;
                PlayerLife.sharedInstance.AddLife(-lifeDrainRate, false);
            }
        }
    }

    void SetFillerColor(Color col)
    {
        rb.GetFiller().color = col;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            contactPlayer = true;
            //rb.BarColorFill = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && damageTimer > 1.0f)
        {
            collision.gameObject.SendMessage("GetHit", -5 * scale);
            damageTimer = 0.0f;

        }
        else if(damageTimer < 1.0f)
        {
            damageTimer += Time.deltaTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            contactPlayer = false;
            //rb.BarColorFill = false;
        }
    }

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        if (collision.gameObject.GetComponent<rageBar>().RBar.fillAmount <= 1)
    //        {collision.gameObject.GetComponent<rageBar>().
    //            RBar.fillAmount += 0.2f * Time.deltaTime;
    //        }
            
    //    }
    //}
}
