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
    public AnimationCurve moveCurve;
    public Text text;
    public GameObject jumpText;
    public string[] messages;
    public int startPhase = 0;
    int phase = -1;
    int textPhase = -1;
    float transitionTextSpeed = 3f;
    float defaultIconEnterSpeed = 1.5f;
    
    public GameObject UIArrowIcon;
    public GameObject UILevitationIcon;
    //public GameObject levitationSystem;
    public Levitation levitationSystem;
    public GameObject rageBar;

    public Transform canvasCenter;
    SortedList<int, VisibleTrigger> pointedObjects;


    GameObject pointedObject;
    bool objectVisible;
    Vector3 arrowOffset;

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
            levitationSystem.SetLevitationActive(false);
        }
        if (phase < 4)
        {
            rageBar.transform.localScale = Vector3.zero;
        }
    }

	void Start () {
        objectVisible = false;
        if (sharedInstance != null)
            Destroy(sharedInstance);
        sharedInstance = this;
        SetPhase(startPhase);
        DisableObjects();
        StartCoroutine(MoveArrow());
        pointedObjects = new SortedList<int, VisibleTrigger>();

        //DisableObjects();
	}

    IEnumerator MoveArrow()
    {
        Vector3 basePosition = arrow.transform.position;
        Vector3 newPos = basePosition;
        Vector3 doorPosition = Vector3.zero;
        float x;
        float y;
        float i = 0f;
        while (true)
        {
            i += Time.deltaTime * 5f;
            x = -Mathf.Cos(i) * 50f;
            y = 200f - Mathf.Cos(i) * 50f;
            doorPosition = objectVisible ? Camera.main.WorldToScreenPoint(pointedObject.transform.position + arrowOffset) : Vector3.zero;
            newPos.x = (objectVisible) ? doorPosition.x : basePosition.x + x;
            newPos.y = (objectVisible) ? doorPosition.y + y : basePosition.y;
            arrow.transform.position = newPos;
            yield return 0;
        }
    }

    IEnumerator MoveInIcon(GameObject icon, Vector3 maxScale, AnimationCurve curve, float moveSpeed, bool startInCenter = false)
    {
        Vector3 startPos = icon.transform.position;
        if (startInCenter)
        {
            icon.transform.position = canvasCenter.position;
        }
        for (float i = 0f; i < 1f; i += Time.deltaTime * defaultIconEnterSpeed)
        {
            icon.transform.localScale = maxScale * curve.Evaluate(i);
            yield return 0;
        }
        icon.transform.localScale = maxScale;
        if (startInCenter)
        {
            yield return new WaitForSeconds(0.6f);
            for(float i = 0f; i < 1f; i += Time.deltaTime)
            {
                icon.transform.position = Vector3.Lerp(canvasCenter.position, startPos, moveCurve.Evaluate(i));
                yield return 0;
            }
            icon.transform.position = startPos;
        }
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
                StartCoroutine(MoveInIcon(UIArrowIcon, Vector3.one, enterCurve1, defaultIconEnterSpeed, true));
                break;

            case 3:
                levitationSystem.SetLevitationActive(true);
                StartCoroutine(MoveInIcon(UILevitationIcon, Vector3.one, enterCurve1, defaultIconEnterSpeed, true));
                break;

            case 4:
                StartCoroutine(MoveInIcon(rageBar, Vector3.one, enterCurve2, defaultIconEnterSpeed * 0.5f, true));
                break;

            case 5:
                if (jumpText != null)
                    jumpText.SetActive(false);
                break;
        }
    }

	void Update () {
		
	}

    public void OnObjectBecameVisible(VisibleTrigger obj, bool visible, int priority)
    {
        if (visible)
        {
            pointedObjects.Add(-obj.priority, obj);
        }
        else
        {
            pointedObjects.Remove(-obj.priority);
        }
        objectVisible = (pointedObjects.Count > 0);
        pointedObject = objectVisible ? pointedObjects.Values[0].gameObject : null;
        arrowOffset = objectVisible ? pointedObjects.Values[0].arrowOffset : Vector3.zero;
        arrow.transform.Rotate(0f, 0f, (objectVisible) ? 270f : -270f);
        arrow.transform.localRotation = Quaternion.Euler(0f, 0f, (objectVisible) ? 270f : 0f);
    }
}
