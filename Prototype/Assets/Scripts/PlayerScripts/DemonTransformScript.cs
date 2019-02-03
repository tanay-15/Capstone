using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonTransformScript : MonoBehaviour
{

    public bool DemonModeActive;
    public GameObject bat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("RightTrigger2"))
        {
            for (int i = 0; i < 50; i++)
            {
                var Bat = Instantiate(bat, transform.position + new Vector3(0, 100, 0), Quaternion.identity);
                Bat.GetComponent<Bat_Script>().player = gameObject;
            }
        }
    }
}
