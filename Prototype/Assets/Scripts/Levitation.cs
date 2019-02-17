using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Levitation : MonoBehaviour {

    public static Levitation sharedInstance;
    public float moveSpeed = 6f;
    public Color nonHoverColor;
    public Color hoverColor;
    Color nonHoverBaseColor;
    Color hoverBaseColor;
    public ParticleSystem particles;
    [System.NonSerialized]
    public GameObject heldObject;
    public float mouseZPosition = 0f;
    float minimumMagnitude = 0.15f;
    float joystickSpeed = 6f;
    bool rightTriggerDown;
    bool rightTriggerPressed;
    bool rightTriggerReleased;
    ActionIndicator action;
    PlayerStates player;
    [System.NonSerialized]
    public bool Active;

    Vector3 positionInCamera;
    Vector3 prevPositionInCamera;
    float movementThreshold = 0.07f;

    public bool HoldingObject
    {
        get
        {
            return (heldObject != null);
        }
    }
    
    bool active;
    Vector3 PlayerPos
    {
        get
        {
            if (Movement2D.sharedInstance != null)
                return Movement2D.sharedInstance.gameObject.transform.position;
            else
                return player.gameObject.transform.position;
        }
    }

    Vector2 baseJoystickPosition;
    Vector2 dJoystick;
    float grabRadius = 0.1f;
    float maxGrabDistance = 4f;
    [System.NonSerialized]
    public Vector3 grabPosition;
    IEnumerable<Collider2D> collidingObjects;

    bool useJoystick;

    GameObject Player
    {
        get
        {
            if (Movement2D.sharedInstance != null)
                return Movement2D.sharedInstance.gameObject;
            else
                return player.gameObject;
        }
    }

    static Levitation()
    {
        sharedInstance = null;
    }

    //Used to make the levitation system work or not but keep the GameObject and position active
    //Because the Arrow shooting system uses the Levitation's position
    void SetActive(bool active)
    {
        particles.gameObject.SetActive(active);
    }

	void Start () {
        Active = true;
        player = FindObjectOfType<PlayerStates>();
        rightTriggerDown = false;
        rightTriggerPressed = false;
        rightTriggerReleased = false;
        if (sharedInstance != null)
        {
            Destroy(sharedInstance);
        }
        sharedInstance = this;
        active = true;

        heldObject = null;
        baseJoystickPosition = Vector2.zero;
        useJoystick = false;
        action = GetComponent<ActionIndicator>();

        nonHoverBaseColor = nonHoverColor;
        hoverBaseColor = hoverColor;

        positionInCamera = particles.gameObject.transform.position - Camera.main.gameObject.transform.position;
        prevPositionInCamera = positionInCamera;
	}

    public void SetLevitationActive(bool active)
    {
        this.active = active;
        particles.gameObject.SetActive(active);
        if (!active && heldObject != null)
        {
            ReleaseObject();
        }
    }

    void CalculatePositionMouse()
    {
        //TODO: Make this easier
        grabPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.gameObject.transform.position.z + mouseZPosition);
        //This line may not be necessary
        grabPosition = Camera.main.ScreenToWorldPoint(grabPosition);

        //Tether the grab area to the player
        Vector3 distance = grabPosition - PlayerPos;
        if (distance.magnitude > maxGrabDistance)
        {
            Ray r = new Ray(PlayerPos, distance);
            grabPosition = r.GetPoint(maxGrabDistance);
        }
        particles.gameObject.transform.position = grabPosition;
    }

    void CalculatePositionJoystick()
    {
        if (dJoystick.magnitude > minimumMagnitude)
            baseJoystickPosition += dJoystick * Time.deltaTime * joystickSpeed;
        if (baseJoystickPosition.magnitude > maxGrabDistance)
            baseJoystickPosition = baseJoystickPosition.normalized * maxGrabDistance;

        grabPosition = PlayerPos + (Vector3)baseJoystickPosition;
        
        particles.gameObject.transform.position = grabPosition;
    }
    
    void UpdateColorAndIcon()
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

        action.Show(collidingObjects.Count() > 0 && heldObject == null);
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
        action.Show(false);
    }

    public void ReleaseObject()
    {
        Rigidbody2D rb = heldObject.GetComponent<Rigidbody2D>();
        IgnoreCollisions(heldObject, false);
        rb.gravityScale = 1;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Discrete;
        heldObject = null;
    }

    void CheckRightTrigger()
    {
        if (Input.GetAxis("RightTrigger1") != 0 && !rightTriggerDown)
        {
            rightTriggerDown = true;
            rightTriggerPressed = true;
        }
        else if (Input.GetAxis("RightTrigger1") == 0 && rightTriggerDown)
        {
            rightTriggerDown = false;
            rightTriggerReleased = true;
        }
    }

    void ResetTriggerCheck()
    {
        rightTriggerPressed = false;
        rightTriggerReleased = false;
    }

    //In the future, detect input from any button/axis on keyboard or controller to switch modes
    void CheckForButtonPress()
    {
        if ((Input.GetMouseButtonDown(1) || rightTriggerPressed) && active)
        {
            if (heldObject == null)
            {
                Collider2D hoveringObject = collidingObjects.FirstOrDefault();
                if (hoveringObject != null)
                    PickUpObject(hoveringObject.gameObject);
            }
        }
        else if ((Input.GetMouseButtonUp(1) || rightTriggerReleased) && active)
        {
            ReleaseObject();
        }
        //if ((Input.GetMouseButtonDown(0) || Input.GetAxis("RightTrigger2") != 0) && active)
        //{
        //    if (heldObject == null)
        //    {
        //        Collider2D hoveringObject = collidingObjects.FirstOrDefault();
        //        if (hoveringObject != null)
        //            PickUpObject(hoveringObject.gameObject);
        //    }
        //    else
        //    {
        //        ReleaseObject();
        //    }
        //}
    }

    void CheckJoystickAndMouse()
    {
        dJoystick = new Vector2(Input.GetAxis("RHorizontal"), Input.GetAxis("RVertical")) ;
        if (dJoystick.magnitude > minimumMagnitude)
        {
            baseJoystickPosition = grabPosition - PlayerPos;
            useJoystick = true;
        }
        if (Input.GetMouseButtonDown(1))
        {
            useJoystick = false;
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

    void DullParticles()
    {
        positionInCamera = particles.gameObject.transform.position - Camera.main.gameObject.transform.position;
        if (Vector3.Distance(positionInCamera,prevPositionInCamera) <= movementThreshold)
        {
            nonHoverColor = nonHoverBaseColor * 0.25f;
            hoverColor = hoverBaseColor * 0.25f;
        }
        else
        {
            nonHoverColor = nonHoverBaseColor;
            hoverColor = hoverBaseColor;
        }
        prevPositionInCamera = positionInCamera;
    }
	
	void Update () {

        CheckJoystickAndMouse();
        if (useJoystick)
            CalculatePositionJoystick();
        else
            CalculatePositionMouse();
        DullParticles();
        if (Active)
        {
            UpdateColorAndIcon();
            CheckRightTrigger();
            CheckForButtonPress();
            ResetTriggerCheck();
            MoveHeldObject();
        }
	}
}
