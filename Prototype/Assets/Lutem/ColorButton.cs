using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColorButton : MonoBehaviour {
    public UnityEvent OnPressed;
    public bool pressed;
    public bool overrideColor = false;
    public Color color;

    [System.NonSerialized]
    public Color myColor;

    List<GameObject> colliders;

    // Use this for initialization
    void Start()
    {
        colliders = new List<GameObject>();
        pressed = false;

        myColor = (overrideColor) ? color : GetComponent<SpriteRenderer>().color;
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
        if (collision.gameObject.GetComponent<SpriteRenderer>().color == myColor)
        {
            OnPressed.Invoke();
            colliders.Add(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<SpriteRenderer>().color == myColor)
        {
            colliders.Remove(collision.gameObject);
        }
    }
}
