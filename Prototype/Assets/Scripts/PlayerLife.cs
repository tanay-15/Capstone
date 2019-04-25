using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour {

    public static PlayerLife sharedInstance;
    public bool lifebarVisible = true;
    public float maxLife = 100f;
    [System.NonSerialized]
    public float currentLife;
    public Image playerIcon;
    public Image demonIcon;
    public Image barGraphic;
    public Image yellowBarGraphic;
    float time = 0f;

    Color goodColor;
    Color badColor;
    Color cosColor;

    float lifebarRatio;

    float lerpAmount = 0.08f;
    float lerpThreshold = 0.002f;

    Color darkGray = new Color(0.25f, 0.25f, 0.25f, 1f);

    static PlayerLife()
    {
        sharedInstance = null;
    }

    public void SetSkillMaxLife()
    {
        maxLife = 120;
        UpdateWidth();
    }

    public void SetDemonIcon(bool set)
    {
        playerIcon.color = (set) ? darkGray : Color.white;
        demonIcon.color = (set) ? Color.white : darkGray;
    }

	void Start () {
        if ((SkillTree.info.nodesActivated & SkillNodes.H_1) == SkillNodes.H_1)
            SetSkillMaxLife();
        lifebarRatio = 1f;
        cosColor = Color.black;
        if (sharedInstance != null)
        {
            Destroy(sharedInstance);
        }
        sharedInstance = this;
        currentLife = maxLife;

        if (lifebarVisible)
        {
            goodColor = barGraphic.color;
            badColor = Color.red;

            SetDemonIcon(false);
        }
	}
	
	void Update () {
        if (lifebarVisible)
        {
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

            //Testing purposes
            //if (Input.GetKeyDown(KeyCode.H))
            //{
            //    AddLife(-10);
            //}

            if (yellowBarGraphic.fillAmount - barGraphic.fillAmount <= lerpThreshold)
            {
                yellowBarGraphic.fillAmount = barGraphic.fillAmount;
            }
            else
            {
                yellowBarGraphic.fillAmount = Mathf.Lerp(yellowBarGraphic.fillAmount, barGraphic.fillAmount, lerpAmount);
            }
        }
	}

    public void UpdateWidth()
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

    public void SetLife(float life)
    {
        currentLife = Mathf.Clamp(life, 0f, 100f);
        UpdateWidth();
    }

    public void SetMinLife()
    {
        currentLife = Mathf.Max(0.01f, currentLife);
        UpdateWidth();
    }

    public void AddLife(float life, bool canKill = true)
    {
        currentLife += life;
        currentLife = Mathf.Clamp(currentLife, 0f, 100f);
        if (!canKill) SetMinLife();
        UpdateWidth();
    }
}
