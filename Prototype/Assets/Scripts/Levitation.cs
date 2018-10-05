using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Levitation : MonoBehaviour {

    public static Levitation sharedInstance;
    public float moveSpeed = 6f;
    public Color nonHoverColor;
    public Color hoverColor;
    public ParticleSystem particles;
    [System.NonSerialized]
    public GameObject heldObject;
    public float mouseZPosition = 0f;
    
    bool active;

    float grabRadius = 0.1f;
    float maxGrabDistance = 4f;
    Vector3 grabPosition;
    IEnumerable<Collider2D> collidingObjects;

    GameObject Player
    {
        get
        {
            return Movement2D.sharedInstance.gameObject;
        }
    }

    static Levitation()
    {
        sharedInstance = null;
    }

	void Start () {
        if (sharedInstance != null)
        {
            Destroy(sharedInstance);
        }
        sharedInstance = this;
        active = true;

        heldObject = null;
	}

    public void SetActive(bool active)
    {
        this.active = active;
        particles.gameObject.SetActive(active);
        if (!active && heldObject != null)
        {
            ReleaseObject();
        }
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
            main.startSize = 0.6f;
        }
        else
        {
            main.startColor = nonHoverColor;
            main.startSize = 0.2f;
        }
    }

    void IgnoreCollisions(GameObject obj, bool ignore)
    {
        //Ignore ALL collisions between ALL colliders
        foreach (Collider2D playerCol in Player.GetComponentsInChildren<Collider2D>())
        {
            foreach (Collider2D objCol in obj.GetComponentsInChildren<Collider2D>())
            {
                Physics2D.IgnoreCollision(playerCol, objCol, ignore);
            }
        }
    }

    void PickUpObject(GameObject obj)
    {
        heldObject = obj;
        Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.velocity = Vector3.zero;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        IgnoreCollisions(obj, true);
    }

    void ReleaseObject()
    {
        Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
        IgnoreCollisions(heldObject, false);
        rb.gravityScale = 1;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        heldObject = null;
    }

    void CheckForButtonPress()
    {
        if (Input.GetMouseButtonDown(0) && active)
        {
            if (heldObject == null)
            {
                Collider2D hoveringObject = collidingObjects.FirstOrDefault();
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
            Vector3 distance = grabPosition - heldObject.transform.position;
            //heldObject.transform.position = grabPosition;
            heldObject.GetComponent<Rigidbody2D>().velocity = distance * moveSpeed;
        }
    }
	
	void Update () {
        CalculatePosition();
        UpdateColor();
        CheckForButtonPress();
        MoveHeldObject();
	}
}
