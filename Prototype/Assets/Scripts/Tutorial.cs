using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Phases
//0 - Jump
//1 - Long jump
//2 - Shoot
//3 - Levitate/Toggle switches
//4 - Demon mode

public class Tutorial : MonoBehaviour {

    public static Tutorial sharedInstance;
    public GameObject arrow;
    public AnimationCurve enterCurve1;
    public AnimationCurve enterCurve2;
    public Text text;
    public string[] messages;
    public int startPhase = 0;
    int phase = -1;
    int textPhase = -1;
    float transitionTextSpeed = 3f;
    float defaultIconEnterSpeed = 1.5f;
    
    public GameObject UIArrowIcon;
    public GameObject UILevitationIcon;
    public GameObject levitationSystem;
    public GameObject rageBar;

    static Tutorial()
    {
        sharedInstance = null;
    }

    void DisableObjects()
    {
        if (phase < 2)
            UIArrowIcon.transform.localScale = Vector3.zero;
        if (phase < 3)
        {
            UILevitationIcon.transform.localScale = Vector3.zero;
            levitationSystem.SetActive(false);
        }
        if (phase < 4)
        {
            rageBar.transform.localScale = Vector3.zero;
        }
    }

	void Start () {
        if (sharedInstance != null)
            Destroy(sharedInstance);
        sharedInstance = this;
        DisableObjects();
        SetPhase(startPhase);
        StartCoroutine(MoveArrow());

        //DisableObjects();
	}

    IEnumerator MoveArrow()
    {
        Vector3 startPosition = arrow.transform.position;
        Vector3 newPos = startPosition;
        float x;
        float i = 0f;
        while (true)
        {
            i += Time.deltaTime * 5f;
            x = -Mathf.Cos(i) * 50f;
            newPos.x = startPosition.x + x;
            arrow.transform.position = newPos;
            yield return 0;
        }
    }

    IEnumerator MoveInIcon(GameObject icon, Vector3 maxScale, AnimationCurve curve, float moveSpeed)
    {
        for (float i = 0f; i < 1f; i += Time.deltaTime * defaultIconEnterSpeed)
        {
            icon.transform.localScale = maxScale * curve.Evaluate(i);
            yield return 0;
        }
        icon.transform.localScale = maxScale;
    }

    IEnumerator TransitionText(string newText)
    {
        float y;
        Vector3 newScale;
        for (float i = 0; i < 1f; i += Time.deltaTime * transitionTextSpeed)
        {
            if (i >= 0.5f)
                text.text = newText;
            y = (i < 0.5f) ? (1f - i * 2f) : ((i - 0.5f) * 2f);
            newScale = new Vector3(1f, y, 1f);
            text.transform.localScale = newScale;
            yield return 0;
        }
        text.transform.localScale = Vector3.one;
    }

    public void SetPhase(int newPhase)
    {
        phase = newPhase;
        if (text != null)
            StartCoroutine(TransitionText(messages[++textPhase].Replace("\\n", "\n")));

        //Enable or disable GameObjects...
        switch (phase)
        {
            case 0:
                break;

            case 1:
                break;

            case 2:
                StartCoroutine(MoveInIcon(UIArrowIcon, Vector3.one * 0.5f, enterCurve1, defaultIconEnterSpeed));
                break;

            case 3:
                levitationSystem.SetActive(true);
                StartCoroutine(MoveInIcon(UILevitationIcon, Vector3.one * 0.5f, enterCurve1, defaultIconEnterSpeed));
                break;

            case 4:
                StartCoroutine(MoveInIcon(rageBar, Vector3.one, enterCurve2, defaultIconEnterSpeed * 0.5f));
                break;
        }
    }

	void Update () {
		
	}
}
