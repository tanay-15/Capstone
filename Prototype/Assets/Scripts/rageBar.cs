using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rageBar : MonoBehaviour {

    [SerializeField]
    private float fillAmount;


    [SerializeField]
    private Image filler;

    public int MaxValue { get; set; }

    public float Value
    {
        set
        {
            fillAmount = Mapping(value, 0, MaxValue, 0, 1);
        }
    }
                                       // Use this for initialization
    void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        handleBar();
	}
    private void handleBar()
    {
        if(fillAmount != filler.fillAmount)
        {
            filler.fillAmount = fillAmount;
        }
        
    }

    private float Mapping(float value, float inMin, float inMax,float outMin,float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }
}
