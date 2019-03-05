using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleButton_Target : MonoBehaviour
{
    public Vector2 targetPosition;

    [HideInInspector]
    public bool moving;

    // speed at which object moves
    public float speed;

    Vector3 beginPos;
    Vector3 endPos;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        beginPos = this.transform.position;
        endPos = this.transform.position + new Vector3(targetPosition.x, targetPosition.y, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, endPos, speed * Time.deltaTime);

            if(this.transform.position == endPos)
            {
                moving = false;
                Vector3 temp = beginPos;
                beginPos = endPos;
                endPos = temp;
            }
        }
    }

    public void StartMove()
    {
        moving = true;
    }
}
