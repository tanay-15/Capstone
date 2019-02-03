using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTreeCursor : MonoBehaviour
{
    float count = 0f;
    float scale;
    public Image img;
    Color newColor;
    Color startColor;
    void Awake()
    {
        img = GetComponent<Image>();
        startColor = img.color;
        count = 0f;
    }

    /// <summary>
    /// Reset the "animation"
    /// </summary>
    public void Reset()
    {
        count = 0f;
        UpdateSprite();
    }

    void UpdateSprite()
    {
        scale = 1.05f - (0.05f * Mathf.Cos(count * 3f));
        float t = 0.5f - 0.5f * Mathf.Cos(count * 3f);
        newColor = Color.Lerp(startColor, Color.white, t);

        transform.localScale = Vector3.one * scale;
        img.color = newColor;
    }


    void Update()
    {
        if (img != null)
        {
            count += 0.02f; //Time.deltaTime;
            UpdateSprite();
        }
    }
}
