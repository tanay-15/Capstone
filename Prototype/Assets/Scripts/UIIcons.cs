using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Should be ordered from left to right top to bottom
public enum UIIconType
{
    Levitation = 0,
    Arrow,
    Interact
}

public class UIIcons : MonoBehaviour {

    public static UIIcons sharedInstance;
    public GameObject[] icons;
    public Color activeColor;
    public Color inactiveColor;
    //public Image qButton;
    public Image[] demonModeButtons;
    public GameObject demonModeText;

    IEnumerator routine;
    Color transparent;

    void SetDemonModeButtons(bool active)
    {
        foreach (Image img in demonModeButtons)
        {
            img.gameObject.SetActive(active);
        }
    }

    void SetDemonModeButtonColors(Color color)
    {
        foreach (Image img in demonModeButtons)
        {
            img.color = color;
        }
    }

    static UIIcons()
    {
        sharedInstance = null;
    }

	// Use this for initialization
	void Start () {
        if (sharedInstance != null)
            Destroy(sharedInstance);
        sharedInstance = this;
        transparent = new Color(1f, 1f, 1f, 0.5f);

        demonModeText.SetActive(false);
        SetDemonModeButtons(false);
	}

    public void SetQButton(bool set)
    {
        if (routine != null)
            StopCoroutine(routine);
        if (set)
        {
            routine = QButtonRoutine();
            StartCoroutine(routine);
        }
        else
        {
            //qButton.gameObject.SetActive(false);
            SetDemonModeButtons(false);
        }

        demonModeText.SetActive(set);
    }

    IEnumerator QButtonRoutine()
    {
        //qButton.gameObject.SetActive(true);
        SetDemonModeButtons(true);
        //qButton.color = Color.white;
        SetDemonModeButtonColors(Color.white);
        while (true)
        {
            yield return new WaitForSeconds(0.7f);
            SetDemonModeButtonColors(transparent);
            yield return new WaitForSeconds(0.7f);
            SetDemonModeButtonColors(Color.white);
        }
    }

    //public void SetIconActive(UIIconType icon, bool active)
    //{
    //    icons[(int)icon].color = (active) ? activeColor : inactiveColor;
    //    icons[(int)icon].gameObject.transform.GetChild(0).gameObject.SetActive(active);
    //}
}
