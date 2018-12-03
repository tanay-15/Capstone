using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform1 : MonoBehaviour {

    public Vector3 target;

    public float timeToTravel;

    public float delay;

    float startTime;
    Vector3 start;
    Vector3 end;
    bool wait;

	// Use this for initialization
	void Start () {
        startTime = Time.time;
        start = transform.position;
        end = transform.position + target;
        wait = false;

        /*LineRenderer lR = gameObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        lR.endWidth = 0.05f;
        lR.startWidth = 0.05f;
        lR.SetPosition(0, new Vector3(start.x, start.y,-1.0f));
        lR.SetPosition(1, new Vector3(end.x, end.y, -1.0f));*/
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        //Debug.DrawLine(start, end, Color.black);
        if (!wait)
        {
            this.transform.position = Vector2.MoveTowards(this.transform.position, end, (Mathf.Abs(Vector3.Distance(start, end)) / timeToTravel) * Time.deltaTime);

            //float t = Time.time - startTime;
            //transform.position = Vector3.Lerp(start, end, t / timeToTravel);
                
            /*if (t > timeToTravel){
                wait = true;
                Vector3 temp = start;
                start = end;
                end = temp;
                target = -target;
                StartCoroutine(Wait());
            }*/

            if(this.transform.position == end)
            {
                wait = true;
                Vector3 temp = start;
                start = end;
                end = temp;
                StartCoroutine(Wait());
            }
        }
	}

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(delay);
        startTime = Time.time;
        wait = false;
        yield return null;
    }

    /*private void OnDrawGizmos()
    {
        Vector3 s = start;
        Vector3 e = end;
        Gizmos.color = Color.black;
        Gizmos.DrawLine(s, e);
    }*/
}
