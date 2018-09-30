using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBossScript : MonoBehaviour {

    public GameObject Boss;
   
    //Transform[] BossBones;
    Transform[] Grunts;

    struct Pair
    {
        public Transform Bone;
        public bool isTaken;

        public Pair(Transform B, bool iT){ Bone = B; isTaken = iT; }
    }


    Pair[] BonesManager = 
        new Pair[] {
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
            new Pair(null,false),
        };
 

    void Assemble()
    {
        int i = 0;

        //BossBones = Boss.GetComponentsInChildren<Transform>();
        Grunts = GetComponentsInChildren<Transform>();

        foreach (Transform t in Grunts)
        {
            if (t.name == "Torso" || t.name == "biceps" || t.name == "forearm" || t.name == "shin" || t.name == "thigh" || t.name == "Head")
            {
                Debug.Log(t.name);
                BonesManager[i] = new Pair(t, false);
                //BonesManager[i].Bone = t;
                //BonesManager[i].isTaken = false;
                i++;
            }
        }
    }



    // Use this for initialization
    void Start () {

        //for (int i = 0; i < 26; i++)
       // {

        //}

        Assemble();
    }

	
	// Update is called once per frame
	void Update () {

        

    }
}
