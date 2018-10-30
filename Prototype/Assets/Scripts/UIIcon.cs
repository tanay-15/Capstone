using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIIcon : MonoBehaviour {
    public Image icon;
    public Image iconBack;
    public Image mask;
    public Sprite[] fillGraphics;
    float fillAmount;
    float fillSpeed = 0.1f;
	void Start () {
        //Temporary
        fillAmount = 1f;// 0f;
	}

    public void SetEmpty()
    {
        fillAmount = 0f;
    }

    void SetSprite()
    {
        int index = (int)(fillAmount * 8f);
        mask.sprite = fillGraphics[index];
        iconBack.gameObject.SetActive(fillAmount < 1f);
    }
	
	void Update () {
		if (fillAmount < 1f)
        {
            fillAmount += Time.deltaTime * fillSpeed;
            if (fillAmount > 1f) fillAmount = 1f;
            SetSprite();
        }
	}
}
