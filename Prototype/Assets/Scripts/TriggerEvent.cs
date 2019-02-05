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
    public UnityEventCollider2D TriggerStay;
    public UnityEventCollider2D TriggerExit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerEnter.Invoke(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TriggerStay.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TriggerExit.Invoke(collision);
    }
}
