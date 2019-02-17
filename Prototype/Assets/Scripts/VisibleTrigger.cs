using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class VisibleEvent : UnityEvent<GameObject, bool> { }

public class VisibleTrigger : MonoBehaviour
{
    public VisibleEvent BecameVisible;

    private void OnBecameVisible()
    {
        BecameVisible.Invoke(gameObject, true);
    }

    private void OnBecameInvisible()
    {
        BecameVisible.Invoke(gameObject, false);
    }
}
