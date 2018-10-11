using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Should be ordered from left to right top to bottom
public enum UIIcon
{
    Levitation = 0,
    Arrow,
    Interact
}

public class UIIcons : MonoBehaviour {

    public static UIIcons sharedInstance;
    public Image[] icons;
    public Color activeColor;
    public Color inactiveColor;
    public Image qButton;

    IEnumerator routine;
    Color transparent;

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
            qButton.gameObject.SetActive(false);
        }
    }

    IEnumerator QButtonRoutine()
    {
        qButton.gameObject.SetActive(true);
        qButton.color = Color.white;
        while (true)
        {
            yield return new WaitForSeconds(0.7f);
            qButton.color = transparent;
            yield return new WaitForSeconds(0.7f);
            qButton.color = Color.white;
        }
    }

    public void SetIconActive(UIIcon icon, bool active)
    {
        icons[(int)icon].color = (active) ? activeColor : inactiveColor;
        icons[(int)icon].gameObject.transform.GetChild(0).gameObject.SetActive(active);
    }
}
