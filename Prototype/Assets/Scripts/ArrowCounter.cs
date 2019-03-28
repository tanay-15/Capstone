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
    public const int MaxArrows = 5;
    public float regenTimer;
    float regenTime = 5f;

    float shakeTime = 0.6f;
    float startAmpl = 0.3f;
    float freq = 50f;

    IEnumerator routine;

    public bool HasMaxArrows {
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

    void UpdateText()
    {
        text.text = ArrowCount.ToString();
        switch (ArrowCount)
        {
            case 0:
                text.color = Color.red;
                break;

            case MaxArrows:
                text.color = Color.yellow;
                break;

            default:
                text.color = Color.white;
                break;

        }
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
        if (regenTimer < regenTime && ArrowCount < 1)
        {
            regenTimer += Time.deltaTime;
            mask.fillAmount = (regenTimer / regenTime);
            darkMask.fillAmount = 1f - mask.fillAmount;
        }
        if (regenTimer >= regenTime && ArrowCount < 1)
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
