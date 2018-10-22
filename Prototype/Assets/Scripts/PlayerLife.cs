using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLife : MonoBehaviour {

    public static PlayerLife sharedInstance;
    public float maxLife = 100f;
    [System.NonSerialized]
    public float currentLife;
    public Image barGraphic;

    static PlayerLife()
    {
        sharedInstance = null;
    }

	void Start () {
        if (sharedInstance != null)
        {
            Destroy(sharedInstance);
        }
        sharedInstance = this;
        currentLife = maxLife;
	}
	
	void Update () {
		
	}

    void UpdateWidth()
    {
        //barGraphic.transform.localScale = new Vector3(currentLife / maxLife, 1f, 1f);
        barGraphic.fillAmount = currentLife / maxLife;
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
