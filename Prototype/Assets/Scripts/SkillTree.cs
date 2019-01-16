using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillTree : MonoBehaviour
{
    public Image black;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Transition(true));
    }

    IEnumerator Transition(bool fadeIn)
    {
        Color col = Color.black;
        for (float i = 0; i < 1f; i += 0.05f)
        {
            col.a = (fadeIn) ? (1 - i) : i;
            black.color = col;
            yield return 0;
        }
        if (!fadeIn)
            ReturnToPauseMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            StartCoroutine(Transition(false));
        }
    }

    void ReturnToPauseMenu()
    {
        PauseMenu.sharedInstance.ReturnToPauseMenu();
    }
}
