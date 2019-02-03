using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeNodeEdge : MonoBehaviour
{
    [System.NonSerialized]
    public Color baseColor;
    public GameObject mask;
    public Image img;
    float count = 0f;
    Vector3 newPos;
    void Awake()
    {
        newPos = mask.transform.localPosition;
        img.color = Color.black;
    }

    public void SetActive(bool active)
    {
        img.color = (active) ? baseColor : Color.black;
    }

    // Update is called once per frame
    void Update()
    {
        count += 0.5f;
        if (count > 48)
            count -= 48;
        newPos.x = -count;
        mask.transform.localPosition = newPos;
    }
}
