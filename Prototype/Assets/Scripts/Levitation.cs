using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Levitation : MonoBehaviour {

    public static Levitation sharedInstance;
    public Color nonHoverColor;
    public Color hoverColor;
    public ParticleSystem particles;
    [System.NonSerialized]
    public GameObject heldObject;
    public float mouseZPosition = 0f;

    float grabRadius = 0.5f;
    Vector3 mousePosition;
    IEnumerable<Collider> collidingObjects;

    static Levitation()
    {
        sharedInstance = null;
    }

	void Start () {
        if (sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        heldObject = null;
	}

    void CalculatePosition()
    {
        //TODO: Make this easier
        mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.gameObject.transform.position.z + mouseZPosition);
        particles.gameObject.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
    }

    void UpdateColor()
    {
        collidingObjects = from col in Physics.OverlapSphere(Camera.main.ScreenToWorldPoint(mousePosition), grabRadius).ToList()
                               where col.gameObject.tag == "Grabbable" || col.gameObject.tag == "Player Weapon"
                               select col;

        ParticleSystem.MainModule main = particles.main;
        if (collidingObjects.Count() > 0)
        {
            main.startColor = hoverColor;
        }
        else
        {
            main.startColor = nonHoverColor;
        }
    }

    void PickUpObject(GameObject obj)
    {
        heldObject = obj;
        heldObject.GetComponent<Rigidbody>().useGravity = false;
        heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    void ReleaseObject()
    {
        heldObject.GetComponent<Rigidbody>().useGravity = true;
        heldObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
        heldObject = null;
    }

    void CheckForButtonPress()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                Collider hoveringObject = collidingObjects.SingleOrDefault();
                if (hoveringObject != null)
                    PickUpObject(hoveringObject.gameObject);
            }
            else
            {
                ReleaseObject();
            }
        }
    }

    void MoveHeldObject()
    {
        if (heldObject != null)
        {
            heldObject.transform.position = Camera.main.ScreenToWorldPoint(mousePosition);
        }
    }
	
	void Update () {
        CalculatePosition();
        UpdateColor();
        CheckForButtonPress();
        MoveHeldObject();
	}
}
