using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class VisibleEvent : UnityEvent<VisibleTrigger, bool, int> { }

public class VisibleTrigger : MonoBehaviour
{
    public VisibleEvent BecameVisible;
    public int priority;
    //For if an arrow is pointing to the object
    public Vector3 arrowOffset = Vector3.zero;
    bool isVisible;

    public void OnDisable()
    {
        if (isVisible)
        {
            BecameVisible.Invoke(this, false, priority);
            isVisible = false;
        }
    }

    private void Start()
    {
        isVisible = false;
    }

    private void OnBecameVisible()
    {
        if (enabled)
        {
            BecameVisible.Invoke(this, true, priority);
            isVisible = true;
        }
    }

    private void OnBecameInvisible()
    {
        if (enabled)
        {
            BecameVisible.Invoke(this, false, priority);
            isVisible = false;
        }
    }
}
