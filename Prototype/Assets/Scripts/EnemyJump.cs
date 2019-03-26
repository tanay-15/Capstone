using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    // Start is called before the first frame update

    public float gravity = -18f;
    public float h = 10f;

    public bool ShouldJump;

    public GameObject AttackPoint;
    private Vector3 FinalJumpPosition;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AttackBehaviour();
    }


    void AttackBehaviour()
    {
        //Just kick the ball

        if (Input.GetKeyDown(KeyCode.Space))
        {
            FinalJumpPosition = AttackPoint.transform.position;
            Attack(FinalJumpPosition);
        }

    }

    Vector3 CalculateLaunchVelocity(Vector3 _FinalJumpPosition)
    {
        float displacementY = _FinalJumpPosition.y - this.transform.position.y;

        Vector3 displacementXZ = new Vector3(_FinalJumpPosition.x - this.transform.position.x, 0, _FinalJumpPosition.z - this.transform.position.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * h * gravity);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * h / gravity) + 0.885f /*1.7785f Mathf.Sqrt(2*(displacementY-h))/gravity*/);

        Debug.Log(Mathf.Sqrt(-2 * h / gravity));
        Debug.Log(Mathf.Sqrt(2 * (displacementY - h)) / gravity);
        Debug.Log(displacementY);
        Debug.Log(velocityXZ);

        return velocityXZ + velocityY;
    }

    void Attack(Vector3 _JumpPosition)
    {
        Physics.gravity = Vector3.up * gravity;
        this.GetComponent<Rigidbody2D>().velocity = CalculateLaunchVelocity(_JumpPosition);
    }
}
