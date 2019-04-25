using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowCounter : MonoBehaviour
{
    public static ArrowCounter sharedInstance;
    public Image mask;
    public Image darkMask;
    public Text text;
    public int ArrowCount { get; private set; }
    //public const int MaxArrows = 5;
    public float regenTimer;
    //float regenTime = 5f;

    float shakeTime = 0.6f;
    float startAmpl = 0.3f;
    float freq = 50f;

    IEnumerator routine;

    int MaxArrows
    {
        get
        {
            return ((SkillTree.info.nodesActivated & SkillNodes.H_4) == SkillNodes.H_4) ? 10 : 5;
        }
    }

    float RegenTime
    {
        get
        {
            return ((SkillTree.info.nodesActivated & SkillNodes.H_4) == SkillNodes.H_4) ? 3.5f : 5f;
        }
    }

    public bool HasMaxArrows
    {
        get
        {
            return ArrowCount >= MaxArrows;
        }
    }

    void Start()
    {
        regenTimer = 0f;
        ArrowCount = 0;
        UpdateText();
        if (sharedInstance != null)
            Destroy(sharedInstance.gameObject);
        sharedInstance = this;
    }

    public void AddArrowCount(int amount)
    {
        ArrowCount += amount;
        UpdateText();
    }

    public void SetMaxArrows()
    {
        ArrowCount = MaxArrows;
        UpdateText();
    }

    public void UpdateArrowCounter()
    {
        ArrowCount = Mathf.Min(ArrowCount, MaxArrows);
        UpdateText();
    }

    void UpdateText()
    {
        text.text = ArrowCount.ToString();

        if (ArrowCount == MaxArrows)
            text.color = Color.yellow;
        else if (ArrowCount == 0)
            text.color = Color.red;
        else
            text.color = Color.white;
    }

    IEnumerator Blink_()
    {
        float scale = 1f;
        float ampl = startAmpl;
        for (float i = 0f; i < shakeTime; i += Time.deltaTime)
        {
            ampl = startAmpl * (1f - (i / shakeTime));
            scale = 1f - ampl * Mathf.Cos(freq * i);
            transform.parent.localScale = Vector3.one * scale;
            yield return 0;
        }
        transform.parent.localScale = Vector3.one;
        routine = null;
    }

    //If the player tries to shoot with 0 arrows
    public void Blink()
    {
        if (routine == null)
        {
            routine = Blink_();
            StartCoroutine(routine);
        }
    }
    
    void Update()
    {
        if (regenTimer < RegenTime && ArrowCount < 1)
        {
            regenTimer += Time.deltaTime;
            mask.fillAmount = (regenTimer / RegenTime);
            darkMask.fillAmount = 1f - mask.fillAmount;
        }
        if (regenTimer >= RegenTime && ArrowCount < 1)
        {
            AddArrowCount(1);
            regenTimer = 0f;
        }
        if (ArrowCount >= 1)
        {
            mask.fillAmount = 1f;
            darkMask.fillAmount = 0f;
        }
    }
}
