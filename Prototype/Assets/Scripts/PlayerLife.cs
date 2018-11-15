﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour {

    public static PlayerLife sharedInstance;
    public float maxLife = 100f;
    [System.NonSerialized]
    public float currentLife;
    public Image barGraphic;
    float time = 0f;

    Color goodColor;
    Color badColor;
    Color cosColor;

    float lifebarRatio;

    static PlayerLife()
    {
        sharedInstance = null;
    }

	void Start () {
        lifebarRatio = 1f;
        cosColor = Color.black;
        if (sharedInstance != null)
        {
            Destroy(sharedInstance);
        }
        sharedInstance = this;
        currentLife = maxLife;

        goodColor = barGraphic.color;
        badColor = Color.red;
	}
	
	void Update () {
        time += Time.deltaTime;
        cosColor.r = -0.2f - Mathf.Cos(time * 15f) * 0.2f;
        cosColor.g = -0.2f - Mathf.Cos(time * 15f) * 0.2f;
        cosColor.b = -0.2f - Mathf.Cos(time * 15f) * 0.2f;
        if (lifebarRatio <= 0.25f)
        {
            barGraphic.color = badColor + cosColor;
        }
        else
        {
            barGraphic.color = goodColor;
        }
	}

    void UpdateWidth()
    {
        //barGraphic.transform.localScale = new Vector3(currentLife / maxLife, 1f, 1f);
        lifebarRatio = currentLife / maxLife;
        barGraphic.fillAmount = lifebarRatio;

        //barGraphic.color = (ratio <= 0.25f) ? badColor : goodColor;
    }

    public void ResetLife()
    {
        currentLife = maxLife;
        UpdateWidth();
    }

    public void SetLife(int life)
    {
        currentLife = Mathf.Clamp(life, 0f, 100f);
        UpdateWidth();
    }

    public void AddLife(int life)
    {
        currentLife += life;
        currentLife = Mathf.Clamp(currentLife, 0f, 100f);
        UpdateWidth();
    }
}
