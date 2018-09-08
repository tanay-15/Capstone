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
    float maxGrabDistance = 4f;
    Vector3 grabPosition;
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
        Vector3 playerPos = Movement.sharedInstance.gameObject.transform.position;
        //TODO: Make this easier
        grabPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.gameObject.transform.position.z + mouseZPosition);
        //This line may not be necessary
        grabPosition = Camera.main.ScreenToWorldPoint(grabPosition);

        //Tether the grab area to the player
        Vector3 distance = grabPosition - playerPos;
        if (distance.magnitude > maxGrabDistance)
        {
            Ray r = new Ray(playerPos, distance);
            grabPosition = r.GetPoint(maxGrabDistance);
        }
        particles.gameObject.transform.position = grabPosition;
    }

    void UpdateColor()
    {
        collidingObjects = from col in Physics.OverlapSphere(grabPosition, grabRadius).ToList()
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
            heldObject.transform.position = grabPosition;
        }
    }
	
	void Update () {
        CalculatePosition();
        UpdateColor();
        CheckForButtonPress();
        MoveHeldObject();
	}
}
