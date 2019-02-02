using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SkillTreeNodeState
{
    disabled = 0,
    inactive,
    active
}

public class SkillTreeNode : MonoBehaviour
{
    Image img;
    public SkillNodes node;
    public Color activeColor;
    public Color inactiveColor;
    public Color disabledColor;
    [System.NonSerialized]
    public string description;
    [System.NonSerialized]
    public SkillTreeNodeState state;

    void Start()
    {
        img = GetComponent<Image>();
        SetState(SkillTreeNodeState.disabled);
    }

    //public void SetActive(bool setActive){
    //    img.color = (setActive) ? activeColor : inactiveColor;
    //    active = setActive;
    //}
    public void SetState(SkillTreeNodeState newState)
    {
        state = newState;
        switch (newState)
        {
            case SkillTreeNodeState.disabled:
                img.color = disabledColor;
                break;

            case SkillTreeNodeState.inactive:
                img.color = inactiveColor;
                break;

            case SkillTreeNodeState.active:
                img.color = activeColor;
                break;
        }
    }
}
