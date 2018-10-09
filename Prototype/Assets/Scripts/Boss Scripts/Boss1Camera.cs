using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Camera : MonoBehaviour {

    // Use this for initialization

    public bool Shake;
    private GameObject player;
    private Vector3 originalpos;

    private float shakedur = 0.8f;


   	void Start () {

        player = GameObject.FindGameObjectWithTag("Player");
        originalpos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {


        if (Shake)
        {
          Vector2 CamShake = Random.insideUnitCircle * 0.2f;

            this.transform.position = new Vector3(this.transform.position.x + CamShake.x, player.transform.position.y   + CamShake.y, 0f);

          
                shakedur = shakedur - Time.deltaTime;

                if(shakedur <= 0)
            {
                Shake = false;
                shakedur = 0.8f;
            }
            
        }

        if (!Shake)
        {
            this.transform.position = originalpos;
        }

       
	}

   public void SmashCamRotate()
    {
        Shake = true;
    }
}
