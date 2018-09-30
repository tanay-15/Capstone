using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour {

    public static Tutorial sharedInstance;
    public AnimationCurve enterCurve;
    public GameObject arrowIcon;
    public Text text;
    public string[] messages;
    int phase = -1;

    float transitionTextSpeed = 3f;
    float iconEnterSpeed = 1.5f;

    static Tutorial()
    {
        sharedInstance = null;
    }

	void Start () {
        arrowIcon.transform.localScale = Vector3.zero;
        if (sharedInstance != null)
            Destroy(sharedInstance);
        sharedInstance = this;
        SetPhase(0);
	}

    IEnumerator MoveInIcon(GameObject icon, Vector3 maxScale)
    {
        for (float i = 0f; i < 1f; i += Time.deltaTime * iconEnterSpeed)
        {
            icon.transform.localScale = maxScale * enterCurve.Evaluate(i);
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
        StartCoroutine(TransitionText(messages[phase].Replace("\\n", "\n")));

        //Enable or disable GameObjects...
        switch (phase)
        {
            case 0:
                break;

            case 1:
                break;

            case 2:
                StartCoroutine(MoveInIcon(arrowIcon, Vector3.one * 0.5f));
                break;
        }
    }

	void Update () {
		
	}
}
