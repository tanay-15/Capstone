﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BloodWellScript : MonoBehaviour {

    bool contactPlayer;
    rageBar rb;
    Color normalColor;
    Color fillingColor;

	// Use this for initialization
	void Start () {
        contactPlayer = false;
        rb = FindObjectOfType<rageBar>();
        normalColor = rb.GetFiller().color;
        fillingColor = normalColor;
        fillingColor.g = 0.5f;
        fillingColor.b = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
        if (contactPlayer)
        {
            if (rb.RBar.fillAmount <= 1)
            {
                rb.RBar.fillAmount += 0.2f * Time.deltaTime;
                SetFillerColor(fillingColor);
            }
            else
            {
                SetFillerColor(normalColor);
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
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            contactPlayer = false;
            SetFillerColor(normalColor);
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