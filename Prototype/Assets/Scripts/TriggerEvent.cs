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
    public string requiredTag;
    public UnityEventCollider2D TriggerEnter;
    public UnityEventCollider2D TriggerStay;
    public UnityEventCollider2D TriggerExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (requiredTag == "" || collision.gameObject.tag == requiredTag)
            TriggerEnter.Invoke(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (requiredTag == "" || collision.gameObject.tag == requiredTag)
            TriggerStay.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (requiredTag == "" || collision.gameObject.tag == requiredTag)
            TriggerExit.Invoke(collision);
    }
}
