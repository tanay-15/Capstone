using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityEventCollider2D : UnityEvent<Collider2D>
{

}

public class TriggerEvent : MonoBehaviour
{
    public UnityEventCollider2D TriggerEnter;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerEnter.Invoke(collision);
    }
}
