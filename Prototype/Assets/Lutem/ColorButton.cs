using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColorButton : MonoBehaviour {
    public UnityEvent OnPressed;
    public bool pressed;

    List<GameObject> colliders;

    // Use this for initialization
    void Start()
    {
        colliders = new List<GameObject>();
        pressed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (colliders.Count > 0)
        {
            pressed = true;
        }
        else
        {
            pressed = false;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<SpriteRenderer>().color == this.GetComponent<SpriteRenderer>().color)
        {
            OnPressed.Invoke();
            colliders.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<SpriteRenderer>().color == this.GetComponent<SpriteRenderer>().color)
        {
            colliders.Remove(collision.gameObject);
        }
    }
}
