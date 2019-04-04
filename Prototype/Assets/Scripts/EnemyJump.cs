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

    public int JumpCount = 0;
    public bool JumpComplete = false;

    public bool LookingLeft = true;
    void Start()
    {
        JumpComplete = false;
        FinalJumpPosition = AttackPoint.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        AttackBehaviour();
     
    }


    void AttackBehaviour()
    {



        if (!JumpComplete)
        {
            JumpAttack();
            JumpComplete = true;
        }


        if(Vector3.Distance(this.transform.position,FinalJumpPosition) < 2f)
        {
            Debug.Log("Made my jump!");
            JumpCount = JumpCount + 1;
            if (JumpCount >= 3)
            {
                flip();
                JumpCount = 0;
            }
            FinalJumpPosition = AttackPoint.transform.position;
            Invoke("JumpAgain", 2f);
           
        }

       

        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Attack(FinalJumpPosition);
        //}

    }

    void JumpAgain()
    {
   
        JumpComplete = false;
    }

    void JumpAttack()
    {
       
        Attack(FinalJumpPosition);
    }

    Vector3 CalculateLaunchVelocity(Vector3 _FinalJumpPosition)
    {
        float displacementY = _FinalJumpPosition.y - this.transform.position.y;

        Vector3 displacementXZ = new Vector3(_FinalJumpPosition.x - this.transform.position.x, 0, _FinalJumpPosition.z - this.transform.position.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * h * gravity);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * h / gravity) + 0.885f /*1.7785f Mathf.Sqrt(2*(displacementY-h))/gravity*/);



        return velocityXZ + velocityY;
    }

    void Attack(Vector3 _JumpPosition)
    {
        Physics.gravity = Vector3.up * gravity;
        this.GetComponent<Rigidbody2D>().velocity = CalculateLaunchVelocity(_JumpPosition);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Destroy(this.gameObject);
        }
    }

    public void flip()
    {

        LookingLeft = !LookingLeft;

        this.transform.localScale = new Vector3(this.transform.localScale.x * -1, this.transform.localScale.y, this.transform.localScale.z);

    }
}
