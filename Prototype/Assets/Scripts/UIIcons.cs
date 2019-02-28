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
    //
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

        icons[2].SetActive(false);
	}

    public void SwitchIcons(int index1, int index2, bool reverse)
    {
        UIIcon icon1 = icons[index1].GetComponent<UIIcon>();
        UIIcon icon2 = icons[index2].GetComponent<UIIcon>();
        StartCoroutine(SwitchIcons_(icon1, icon2, reverse));
    }

    IEnumerator SwitchIcons_(UIIcon icon1, UIIcon icon2, bool reverse)
    {
        Color newColor;
        for (float i = 0f; i < 1f; i += Time.deltaTime * 2f)
        {
            newColor = Color.Lerp(Color.white, Color.black, i);
            if (!reverse)
                icon1.icon.color = newColor;
            else
                icon2.icon.color = newColor;
            yield return 0;
        }
        icon1.icon.color = Color.black;
        icon2.icon.color = Color.black;
        icon1.gameObject.SetActive(reverse);
        icon2.gameObject.SetActive(!reverse);
        for (float i = 0f; i < 1f; i += Time.deltaTime * 2f)
        {
            newColor = Color.Lerp(Color.black, Color.white, i);
            if (!reverse)
                icon2.icon.color = newColor;
            else
                icon1.icon.color = newColor;
            yield return 0;
        }
        icon1.icon.color = Color.white;
        icon2.icon.color = Color.white;
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
