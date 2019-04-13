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
    //public UnityEventCollider2D Spawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
        {
            Debug.Log(collision.gameObject.name);
            if (transform.parent.GetComponent<Enemy>().enabled == false)
                transform.parent.SendMessageUpwards("Spawn");

            TriggerEnter.Invoke(collision);
        }
            
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
            TriggerStay.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 14)
            TriggerExit.Invoke(collision);
    }
}
