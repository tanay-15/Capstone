using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionIndicator : MonoBehaviour {

    public AnimationCurve showCurve;
    public AnimationCurve hideCurve;
    float speed = 5f;

    public GameObject indicator;
    IEnumerator routine;

    private void Start()
    {
        indicator.transform.localScale = Vector3.zero;
        indicator.SetActive(true);
        UIIcons.sharedInstance.SetIconActive(UIIcon.Interact, false);
    }

    IEnumerator Scale(bool show)
    {
        for (float i = 0f; i < 1f; i += Time.deltaTime * speed)
        {
            AnimationCurve curve = (show) ? showCurve : hideCurve;
            float scale = curve.Evaluate(i);
            indicator.transform.localScale = Vector3.one * scale;
            yield return 0;
        }
        indicator.transform.localScale = (show) ? Vector3.one : Vector3.zero;
    }

    public void Show(bool show)
    {
        UIIcons.sharedInstance.SetIconActive(UIIcon.Interact, show);

        if (routine != null)
            StopCoroutine(routine);
        routine = Scale(show);
        StartCoroutine(routine);
    }
}
