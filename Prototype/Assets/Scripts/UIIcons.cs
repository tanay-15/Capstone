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

    static UIIcons()
    {
        sharedInstance = null;
    }

	// Use this for initialization
	void Start () {
        if (sharedInstance != null)
            Destroy(sharedInstance);
        sharedInstance = this;
	}

    public void SetIconActive(UIIcon icon, bool active)
    {
        icons[(int)icon].color = (active) ? activeColor : inactiveColor;
        icons[(int)icon].gameObject.transform.GetChild(0).gameObject.SetActive(active);
    }
}
