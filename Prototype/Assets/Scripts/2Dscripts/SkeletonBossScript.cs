using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBossScript : MonoBehaviour {

    public GameObject Boss;
   
    Transform[] BossBones;
    Transform[] Grunts;

    float fracLerp = 0;

    bool isAssembling = false;

    Vector3 init = new Vector3();

    public enum State
    {
        Inactive,
        Assembling,
        Active,
        Dead
    }

    public State Status = State.Inactive;

    struct Pair
    {
        public Transform Bone;
        public Vector3 InitialPosition;
        public Transform FinalPosition;
        public bool isTaken;

        public Pair(Transform B, Vector3 I, Transform F, bool iT){ Bone = B; InitialPosition = I; FinalPosition = F; isTaken = iT; }
    }

    Pair[] BonesManager = 
        new Pair[] {
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
            new Pair(null,new Vector3(),null,false),
        };
 

    void Assemble()
    {
        int i = 0;



        foreach (Transform t in Grunts)
        {
            if (t.name == "Torso" || t.name == "biceps" || t.name == "forearm" || t.name == "shin" || t.name == "thigh" || t.name == "Head")
            {
                
                BonesManager[i] = new Pair(t, t.position,null, false);
                i++;
            }
        }

        //i = 0;

        foreach (Transform t in BossBones)
        {
            string s = t.name;

            for (i = 0; i < BonesManager.Length; i++)
            {
                 if (s[0] == 'a')
                    {
                        if ((BonesManager[i].Bone.name == "biceps" || BonesManager[i].Bone.name == "forearm") && BonesManager[i].isTaken == false)
                        {
                            BonesManager[i].FinalPosition = t;
                            BonesManager[i].Bone.parent = transform;
                            //BonesManager[i].Bone.position = t.position;
                            //BonesManager[i].Bone.rotation = t.rotation;
                            //BonesManager[i].Bone.localScale = t.localScale;
                            BonesManager[i].isTaken = true;
                            break;
                        }                    
                }

                else if (s[0] == 'l')
                {
                    if ((BonesManager[i].Bone.name == "thigh" || BonesManager[i].Bone.name == "shin") && BonesManager[i].isTaken == false)
                    {
                        BonesManager[i].FinalPosition = t;
                        BonesManager[i].Bone.parent = transform;
                        BonesManager[i].isTaken = true;
                        break;
                    }
                }

                else if (s[0] == 'h')
                {
                    if (BonesManager[i].Bone.name == "Head" && BonesManager[i].isTaken == false)
                    {
                        BonesManager[i].FinalPosition = t;
                        BonesManager[i].Bone.parent = transform;
                        BonesManager[i].isTaken = true;
                        break;
                    }
                }

                else if(s[0] == 't')
                {
                    if (BonesManager[i].Bone.name == "Torso" && BonesManager[i].isTaken == false)
                    {
                        BonesManager[i].FinalPosition = t;
                        BonesManager[i].Bone.parent = transform;
                        BonesManager[i].isTaken = true;
                        break;
                    }
                }

               
            }

        }
    }



    // Use this for initialization
    void Start () {

        BossBones = Boss.GetComponentsInChildren<Transform>();
        Grunts = transform.GetChild(0).GetComponentsInChildren<Transform>();

        //Assemble();
    }

	
	// Update is called once per frame
	void Update () {

        switch(Status)
        {
            case State.Inactive:
            {
                if (Grunts[0].GetChild(0).GetComponent<RagdollEnemy>().isDead
                    && Grunts[0].GetChild(1).GetComponent<RagdollEnemy>().isDead
                    && Grunts[0].GetChild(2).GetComponent<RagdollEnemy>().isDead)
                    {                       
                        StartCoroutine("DelayedAssemble");
                        Status = State.Assembling;
                    }
                break;
            }



            case State.Assembling:
            {
                if (isAssembling)
                { 
                    for (int i = 0; i < BonesManager.Length; i++)
                    {
                        fracLerp += 0.02f * Time.deltaTime;
                        BonesManager[i].Bone.position = Vector3.Lerp(BonesManager[i].InitialPosition, BonesManager[i].FinalPosition.position, fracLerp);
                        BonesManager[i].Bone.rotation = Quaternion.Lerp(BonesManager[i].Bone.rotation, BonesManager[i].FinalPosition.rotation, fracLerp);
                        
                    }

                    if (fracLerp > 1)
                    {
                        Status = State.Active;
                    }
                }

                break;
            }

            case State.Active:
            {
                    for (int i = 0; i < BonesManager.Length; i++)
                    {
                        BonesManager[i].Bone.position = BonesManager[i].FinalPosition.position;
                        BonesManager[i].Bone.rotation = BonesManager[i].FinalPosition.rotation;
                    }

                    break;
            }

        }

        /*
        for (int i = 0; i < BonesManager.Length; i++)
        {
            fracLerp += 0.001f * Time.deltaTime;
            BonesManager[i].Bone.position = Vector3.Lerp(BonesManager[i].Bone.position, BonesManager[i].FinalPosition.position, fracLerp);
            BonesManager[i].Bone.rotation = Quaternion.Lerp(BonesManager[i].Bone.rotation, BonesManager[i].FinalPosition.rotation, fracLerp);
        }
        */

        
    }

    IEnumerator DelayedAssemble()
     {
        yield return new WaitForSeconds(2f);

        Grunts[0].GetChild(0).GetComponent<RagdollEnemy>().SetChildrenKinematic(true);
        Grunts[0].GetChild(1).GetComponent<RagdollEnemy>().SetChildrenKinematic(true);
        Grunts[0].GetChild(2).GetComponent<RagdollEnemy>().SetChildrenKinematic(true);

        Assemble();

        isAssembling = true;
    }
}
