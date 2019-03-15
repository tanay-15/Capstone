using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

/*
//Triggers
1 - Melee
2 - Roll
3 - Melee
4 - Levitation
5 - Stomp
6 - Arrow
7 - Button


*/
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
    //public GameObject jumpText;
    public string[] messages;
    public GameObject[] images;
    public int startPhase = 0;
    int phase = -1;
    int textPhase = -1;
    int phaseShown = -2;
    float transitionTextSpeed = 3f;
    float defaultIconEnterSpeed = 1.5f;
    
    public GameObject UIArrowIcon;
    public GameObject UILevitationIcon;
    //public GameObject levitationSystem;
    public Levitation levitationSystem;
    public GameObject rageBar;

    public Transform canvasCenter;
    SortedList<int, VisibleTrigger> pointedObjects;
    bool showingImage;
    Grayscale grayscale;
    int imageIndex;


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
        grayscale = FindObjectOfType<Grayscale>();
        showingImage = false;
        imageIndex = -1;
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

    private void OnEnable()
    {
        foreach (GameObject img in images)
        {
            img.SetActive(false);
        }
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
        if (newPhase <= phaseShown) { return; }
        phase = newPhase;
        phaseShown = phase;
        if (text != null)
            StartCoroutine(TransitionText(messages[++textPhase].Replace("\\n", "\n")));

        //Enable or disable GameObjects...
        switch (phase)
        {
            case 0:
                break;

            case 1:
                ShowImages(2);
                break;

            case 2:
                ShowImages(1);
                break;

            case 3:
                ShowImages(3);
                levitationSystem.SetLevitationActive(true);
                StartCoroutine(MoveInIcon(UILevitationIcon, Vector3.one, enterCurve1, defaultIconEnterSpeed, true));
                break;

            case 4:
                ShowImages(4);
                //StartCoroutine(MoveInIcon(rageBar, Vector3.one, enterCurve2, defaultIconEnterSpeed * 0.5f, true));
                break;

            case 5:
                //if (jumpText != null)
                //    jumpText.SetActive(false);
                StartCoroutine(MoveInIcon(UIArrowIcon, Vector3.one, enterCurve1, defaultIconEnterSpeed, true));
                ShowImages(0);
                break;
            case 6:
                ShowImages(5);
                break;
            case 7:
                ShowImages(0);
                break;
            case 8:
                ShowImages(1);
                break;
            case 9:
                ShowImages(2);
                break;
            case 10:
                ShowImages(3);
                break;
            case 11:
                ShowImages(5);
                break;
            case 12:
                ShowImages(6);
                break;
        }
    }

    IEnumerator ShowImages_(params int[] indexes)
    {
        PauseMenu.sharedInstance.PauseMenuDisabled = true;
        Time.timeScale = 0f;
        grayscale.enabled = true;
        showingImage = true;
        foreach(int index in indexes)
        {
            imageIndex = index;
            images[index].SetActive(true);
            yield return 0;
            while (!(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("PS4Jump")))
            {
                yield return 0;
            }
            images[imageIndex].SetActive(false);
        }
        Time.timeScale = 1f;
        grayscale.enabled = false;
        showingImage = false;
        PauseMenu.sharedInstance.PauseMenuDisabled = false;
    }

    void ShowImages(params int[] indexes)
    {
        StartCoroutine(ShowImages_(indexes));
    }

    void ShowImage(int index)
    {
        images[index].SetActive(true);
        Time.timeScale = 0f;
        grayscale.enabled = true;
        showingImage = true;
        imageIndex = index;
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
