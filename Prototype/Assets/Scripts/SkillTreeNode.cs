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
    public GameObject lockSprite;
    public Color activeColor;
    public Color inactiveColor;
    public Color disabledColor;
    [System.NonSerialized]
    public string description;
    [System.NonSerialized]
    public SkillTreeNodeState state;

    void Awake()
    {
        img = GetComponent<Image>();
        SetState(SkillTreeNodeState.disabled);
    }
    public void SetState(SkillTreeNodeState newState)
    {
        state = newState;
        switch (newState)
        {
            case SkillTreeNodeState.disabled:
                lockSprite.SetActive(true);
                img.color = disabledColor;
                break;

            case SkillTreeNodeState.inactive:
                lockSprite.SetActive(false);
                img.color = inactiveColor;
                break;

            case SkillTreeNodeState.active:
                lockSprite.SetActive(false);
                img.color = activeColor;
                break;
        }
    }
}
