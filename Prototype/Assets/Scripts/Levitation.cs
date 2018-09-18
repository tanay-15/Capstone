using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Levitation : MonoBehaviour {

    public static Levitation sharedInstance;
    public float followSpeed = 6f;
    public Color nonHoverColor;
    public Color hoverColor;
    public ParticleSystem particles;
    [System.NonSerialized]
    public GameObject heldObject;
    public float mouseZPosition = 0f;

    bool heldObjectWasDiscrete;
    float grabRadius = 0.5f;
    float maxGrabDistance = 4f;
    Vector3 grabPosition;
    IEnumerable<Collider2D> collidingObjects;

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
        heldObjectWasDiscrete = true;
	}

    void CalculatePosition()
    {
        Vector3 playerPos = Movement2D.sharedInstance.gameObject.transform.position;
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
        collidingObjects = from col in Physics2D.OverlapCircleAll(grabPosition, grabRadius).ToList()
                               where col.gameObject.tag == "Grabbable" || col.gameObject.tag == "Player Weapon"
                               select col;

        ParticleSystem.MainModule main = particles.main;
        if (collidingObjects.Count() > 0 || heldObject != null)
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
        Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;

        //So you cant move the object through walls
        heldObjectWasDiscrete = rb.collisionDetectionMode == CollisionDetectionMode2D.Discrete;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    void ReleaseObject()
    {
        Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 1;
        //rb.velocity = Vector3.zero;
        rb.collisionDetectionMode = (heldObjectWasDiscrete) ? CollisionDetectionMode2D.Discrete : CollisionDetectionMode2D.Continuous;
        heldObject = null;
    }

    void CheckForButtonPress()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObject == null)
            {
                Collider2D hoveringObject = collidingObjects.SingleOrDefault();
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
        if (heldObject == null) return;

        Vector3 distance = grabPosition - heldObject.transform.position;
        Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
        rb.velocity = distance * followSpeed;
    }
	
	void Update () {
        CalculatePosition();
        UpdateColor();
        CheckForButtonPress();
	}

    void FixedUpdate()
    {
        MoveHeldObject();
    }
}
