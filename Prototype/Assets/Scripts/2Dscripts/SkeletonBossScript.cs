using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBossScript : MonoBehaviour {

    public GameObject Boss;
    public GameObject Eyes;

    Transform[] BossBones;
    Transform[] Grunts;

    float fracLerp = -2;

    int Health = 50;

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

    struct BoneSystem
    {
        public Transform Bone;
        public Vector3 InitialPosition;
        public Transform FinalPosition;
        public bool isTaken;
        public float fracLerp;

        public BoneSystem(Transform B, Vector3 I, Transform F, bool iT)
        {
            Bone = B; InitialPosition = I;
            FinalPosition = F; isTaken = iT;
            fracLerp = 0;
        }
    }

    BoneSystem[] BonesManager = 
        new BoneSystem[] {
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
            new BoneSystem(null,new Vector3(),null,false),
        };
 

    void Assemble()
    {
        int i = 0;



        foreach (Transform t in Grunts)
        {
            if (t.name == "Torso" || t.name == "biceps" || t.name == "forearm" || t.name == "shin" || t.name == "thigh" || t.name == "Head")
            {
                
                BonesManager[i] = new BoneSystem(t, t.position,null, false);

                if (t.name == "Head")
                    BonesManager[i].fracLerp = -2;
                else
                    BonesManager[i].fracLerp = Random.value * -2;
                
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
                        Eyes.SetActive(true);
                        Status = State.Assembling;
                    }
                break;
            }



            case State.Assembling:
            {
                if (isAssembling)
                {
                    fracLerp +=  Time.deltaTime;
                    for (int i = 0; i < BonesManager.Length; i++)
                    {                        
                        BonesManager[i].fracLerp +=  Time.deltaTime;
                        BonesManager[i].Bone.position = Vector3.Lerp(BonesManager[i].InitialPosition, BonesManager[i].FinalPosition.position, BonesManager[i].fracLerp);
                        BonesManager[i].Bone.rotation = Quaternion.Lerp(BonesManager[i].Bone.rotation, BonesManager[i].FinalPosition.rotation, BonesManager[i].fracLerp);
                        
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
                    GetComponent<BoxCollider2D>().enabled = true;
                    for (int i = 0; i < BonesManager.Length; i++)
                    {
                        BonesManager[i].Bone.position = BonesManager[i].FinalPosition.position;
                        BonesManager[i].Bone.rotation = BonesManager[i].FinalPosition.rotation;
                    }

                    if (Health <= 0)
                    {
                        Status = State.Dead;
                    }
                    break;
            }

            case State.Dead:
            {
                    GetComponent<BoxCollider2D>().enabled = false;
                    Eyes.SetActive(false);

                    foreach (Rigidbody2D rb2d in GetComponentsInChildren<Rigidbody2D>())
                    {
                        rb2d.isKinematic = false;
                        rb2d.GetComponent<Collider2D>().enabled = true;
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


    public void applyDamage(int damage)
    {
        Health -= damage;
    }
}
