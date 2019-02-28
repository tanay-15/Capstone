using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyTargets : MonoBehaviour
{
    // 0 is horizontal, 1 is vertical
    public bool vert;

    public Vector2 maxDist;
    float minPos;
    float maxPos;

    // how fast the object moves
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        if (vert)
        {
            minPos = this.transform.position.y + maxDist.x;
            maxPos = this.transform.position.y + maxDist.y;
        }
        else
        {
            minPos = this.transform.position.x + maxDist.x;
            maxPos = this.transform.position.x + maxDist.y;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTarget(float val)
    {
        Vector3 target = this.transform.position;

        if (vert)
        {
            if (val > 0)
            {
                target.y = maxPos;
            }
            else
            {
                target.y = minPos;
            }
        }
        else
        {
            if (val > 0)
            {
                target.x = maxPos;
            }
            else
            {
                target.x = minPos;
            }
        }

        this.transform.position = Vector3.MoveTowards(this.transform.position, target, speed * Time.deltaTime);
    }
}
