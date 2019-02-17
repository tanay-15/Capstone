using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class rageBar : MonoBehaviour {

    [SerializeField]
    public float fillAmount;
    private bool rageBarActive;
    [System.NonSerialized]
    public rageBar RBar;
    private float newValue;
    private float updatedValue;
    public float speedtoDecrease;
    public float speedtoIncrease;
    [SerializeField]
    private Image filler;
    bool rageBarFilled;
    Color normalColor;
    Color fillingColor;
    private bool barColorFill;
    GradientScroll gradientScroll;
    DemonModeScript demonModeScript;

    //public bool BarColorFill
    //{
    //    get { return barColorFill; }
    //    set
    //    {
    //        //Ignore setting the filler color if the bar is already full or if rage mode is active
    //        if (fillAmount < 1 && !rageBarActive)
    //            filler.color = (value) ? fillingColor : normalColor;
    //        barColorFill = value;
    //    }
    //}

   
                                       // Use this for initialization
    void Start () {
        rageBarActive = false;
        RBar = GetComponent<rageBar>();
        rageBarFilled = false;
        gradientScroll = FindObjectOfType<GradientScroll>();
        demonModeScript = GetComponent<DemonModeScript>();

        normalColor = RBar.filler.color;
        fillingColor = normalColor;
        fillingColor.g = 0.5f;
        fillingColor.b = 0.5f;
    }

    public void AddRage(float amount)
    {
        RBar.fillAmount = Mathf.Clamp(RBar.fillAmount + amount, 0f, 1f);
    }
	
	// Update is called once per frame
	void Update () {
        handleBar();

        //if(Input.GetKeyDown(KeyCode.Q))
        {
            if (demonModeScript != null)
                rageBarActive = demonModeScript.DemonModeActive && !GetComponent<DemonModeScript>().transitioning;
        }

        if (rageBarActive)
        {
            //Drain the rage bar while demon mode is active
            if (RBar.fillAmount >= 0)
                RBar.fillAmount -= 0.04f * Time.deltaTime;//0.07f * Time.deltaTime;
            else
            {
                RBar.fillAmount = 0;
                GetComponent<DemonModeScript>().Transformation();
                rageBarActive = false;

                //BarColorFill = false;
            }
        }


        //Slowly fill the rage bar when in normal mode
        //if (!rageBarActive && RBar.fillAmount <= 1)
        //{
        //    RBar.fillAmount += 0.02f * Time.deltaTime;
        //}

        //Call one time once the bar fills
        if (RBar.fillAmount >= 1f && !rageBarFilled && !rageBarActive)
        {
            rageBarFilled = true;
            UIIcons.sharedInstance.SetQButton(true);
        }

        //Probably not necessary unless the rage bar drains without entering demon mode
        else if (RBar.fillAmount < 1f && rageBarFilled && !rageBarActive)
        {
            rageBarFilled = false;
            UIIcons.sharedInstance.SetQButton(false);
        }


    }
    private void handleBar()
    {
        if (filler == null) return;
        if(RBar.fillAmount != filler.fillAmount)
        {
            filler.fillAmount = RBar.fillAmount;
        }
        //Debug.Log(RBar.fillAmount);
        if (RBar.fillAmount >= 1f)
        {
            //filler.color = fillingColor;
            gradientScroll.EnableGradient(true);
            filler.color = Color.red;
        }
        else if (RBar.fillAmount < 1f)
        {
            gradientScroll.EnableGradient(false);
            filler.color = Color.yellow;
        }
        
    }

    public Image GetFiller()
    {
        return filler;
    }

    private float Mapping(float value, float inMin, float inMax,float outMin,float outMax)
    {
        return (value - inMin) * (outMax - outMin) / (inMax - inMin) + outMin;
    }

    private void rageBarDeplete()
    {
        //newValue = Mapping(RBar.fillAmount, 0,100,0,1);
        //updatedValue = Mathf.Lerp(RBar.fillAmount, newValue, Time.deltaTime * speedtoDecrease);
        //RBar.fillAmount = updatedValue;

        
            RBar.fillAmount -= 0.03f* Time.deltaTime;
     }

    private void  rageBarRegen()
    {
        //newValue = Mapping(100, 0, 100, 0, 1);
        //RBar.fillAmount = Mathf.Lerp(RBar.fillAmount, newValue, Time.deltaTime * speedtoIncrease);  

       
            RBar.fillAmount += 0.06f * Time.deltaTime;
    }
}
