using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArrowCounter : MonoBehaviour
{
    public static ArrowCounter sharedInstance;
    public Text text;
    public int ArrowCount { get; private set; }
    public const int MaxArrows = 3;
    public bool HasMaxArrows {
        get
        {
            return ArrowCount >= MaxArrows;
        }
    }

    void Start()
    {
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
    
    void Update()
    {
        
    }
}
