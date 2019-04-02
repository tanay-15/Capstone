using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIndicator : MonoBehaviour {

    public AnimationCurve showCurve;
    public AnimationCurve hideCurve;
    float speed = 5f;
    bool shown = false;
    Vector3 initialScale;

    public GameObject indicator;
    IEnumerator routine;

    private void Start()
    {
        initialScale = indicator.transform.localScale;
        indicator.transform.localScale = Vector3.zero;
        indicator.SetActive(true);
        //UIIcons.sharedInstance.SetIconActive(UIIcon.Interact, false);
    }

    private void LateUpdate()
    {
        if (indicator.transform.parent != null)
        {
            Vector3 newScale = indicator.transform.localScale;
            newScale.x = (Mathf.Sign(indicator.transform.parent.localScale.x) != Mathf.Sign(indicator.transform.localScale.x)) ? -newScale.x : newScale.x;
            indicator.transform.localScale = newScale;
        }
    }

    IEnumerator Scale(bool show)
    {
        for (float i = 0f; i < 1f; i += Time.deltaTime * speed)
        {
            AnimationCurve curve = (show) ? showCurve : hideCurve;
            float scale = curve.Evaluate(i);
            indicator.transform.localScale = initialScale * scale;
            yield return 0;
        }
        indicator.transform.localScale = (show) ? initialScale : Vector3.zero;
    }

    public void Show(bool show)
    {
        if (shown == show) { return; }
        shown = show;
        //UIIcons.sharedInstance.SetIconActive(UIIcon.Interact, show);

        if (routine != null)
            StopCoroutine(routine);
        routine = Scale(show);
        StartCoroutine(routine);
    }
}
