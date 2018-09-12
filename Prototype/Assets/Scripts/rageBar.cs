using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rageBar : MonoBehaviour {

    [SerializeField]
    private float fillAmount;
    private bool rageBarActive;
    private rageBar RBar;
    private float newValue;
    private float updatedValue;
    public float speedtoDecrease;
    public float speedtoIncrease;
    [SerializeField]
    private Image filler;

   
                                       // Use this for initialization
    void Start () {
        rageBarActive = false;
        RBar = GetComponent<rageBar>();
	}
	
	// Update is called once per frame
	void Update () {
        handleBar();

        if(Input.GetKeyDown(KeyCode.Z))
        {
            if (rageBarActive)
            {
                rageBarActive = false;
            }
            else
            {
                rageBarActive = true;
            }      
        }

        if(rageBarActive)
        {
            Invoke("rageBarDeplete", 3f);
        }
        else if (!rageBarActive)
        {
            Invoke("rageBarRegen", 5f);
        }

        
	}
    private void handleBar()
    {
        if(RBar.fillAmount != filler.fillAmount)
        {
            filler.fillAmount = RBar.fillAmount;
        }
        
    }

    private float Mapping(float value, float inMin, float inMax,float outMin,float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    private void rageBarDeplete()
    {
        newValue = Mapping(RBar.fillAmount, 0,100,0,1);
        updatedValue = Mathf.Lerp(RBar.fillAmount, newValue, Time.deltaTime * speedtoDecrease);
        RBar.fillAmount = updatedValue;
    }

    private void  rageBarRegen()
    {
        newValue = Mapping(100, 0, 100, 0, 1);
        RBar.fillAmount = Mathf.Lerp(RBar.fillAmount, newValue, Time.deltaTime * speedtoIncrease);  
    }
}
