using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeNode : MonoBehaviour
{
    Image img;
    public SkillNodes node;
    public Color activeColor;
    public Color inactiveColor;
    [System.NonSerialized]
    public string description;
    [System.NonSerialized]
    public bool active;

    void Start()
    {
        img = GetComponent<Image>();
        img.color = inactiveColor;
    }

    public void SetActive(bool setActive){
        img.color = (setActive) ? activeColor : inactiveColor;
        active = setActive;
    }
}
