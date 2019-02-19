using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulleyTargets : MonoBehaviour
{
    // 0 is horizontal, 1 is vertical
    public bool vert;
    
    // how fast the object moves
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTarget(float val)
    {
        if (vert)
        {
            this.transform.position += new Vector3(0.0f, val * speed, 0.0f);
        }
        else
        {
            this.transform.position += new Vector3(val * speed, 0.0f, 0.0f);
        }
    }
}
