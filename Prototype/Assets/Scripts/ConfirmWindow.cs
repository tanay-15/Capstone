using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmWindow : MonoBehaviour
{
    public GameObject cursor;
    public Transform yes;
    public Transform no;
    public Text text;
    [System.NonSerialized]
    public bool selectedYes;
    void Start()
    {
        selectedYes = false;
        cursor.transform.position = no.position;
    }

    private void OnEnable()
    {
        selectedYes = false;
        cursor.transform.position = no.position;
    }

    public void SetSkillPointsIndicator(int skillPoints)
    {
        text.text = string.Format("Learn this skill for {0} skill point{1}?", skillPoints, (skillPoints != 1) ? "s" : "");
    }

    public void ChangeSelection()
    {
        selectedYes = !selectedYes;
        cursor.transform.position = (selectedYes) ? yes.position : no.position;
    }
}
