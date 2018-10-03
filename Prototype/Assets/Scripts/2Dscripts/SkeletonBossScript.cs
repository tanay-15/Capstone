using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBossScript : MonoBehaviour {

    public GameObject Boss;
    public GameObject Eyes;
    public GameObject GruntProjectile;
    public GameObject BossProjectile;
    public GameObject Player;
    public GameObject MainCamera;

   Transform[] BossBones;
   Transform[] Grunts;

    float fracLerp = -2;
    float attackTimer = 0;

    int Health = 50;
    int activeHand = 1;

    bool isAssembling = false;


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
        public Transform InitialPosition;
        public Transform FinalPosition;
        public bool isTaken;
        public float fracLerp;

        public BoneSystem(Transform B, Transform I, Transform F, bool iT)
        {
            Bone = B; InitialPosition = I;
            FinalPosition = F; isTaken = iT;
            fracLerp = 0;
        }
    }

    BoneSystem[] BonesManager = 
        new BoneSystem[] {
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
            new BoneSystem(null,null,null,false),
        };
 

    void Assemble()
    {
        int i = 0;



        foreach (Transform t in Grunts)
        {
            if (t.name == "Torso" || t.name == "biceps" || t.name == "forearm" || t.name == "shin" || t.name == "thigh" || t.name == "Head")
            {               
                BonesManager[i] = new BoneSystem(t, null,null, false);

                var init = new GameObject().transform;

                BonesManager[i].InitialPosition = init;
                BonesManager[i].InitialPosition.position = t.position;
                BonesManager[i].InitialPosition.rotation = t.rotation;
                BonesManager[i].InitialPosition.localScale = t.localScale;

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
                            BonesManager[i].InitialPosition.localScale = BonesManager[i].Bone.localScale;
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
                        BonesManager[i].InitialPosition.localScale = BonesManager[i].Bone.localScale;
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
                        BonesManager[i].InitialPosition.localScale = BonesManager[i].Bone.localScale;
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
                        BonesManager[i].InitialPosition.localScale = BonesManager[i].Bone.localScale;
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
                else
                { 
                    attackTimer += Time.deltaTime;
                    if (attackTimer > 3  &&  Vector3.Distance(Player.transform.position,transform.position) < 7.5f)
                    {
                            if (!Grunts[0].GetChild(0).GetComponent<RagdollEnemy>().isDead)
                            {
                                var proj = Instantiate(GruntProjectile, Grunts[0].GetChild(0).position, Quaternion.identity);
                                proj.SetActive(true);
                                proj.GetComponent<Rigidbody2D>().velocity = 3f * Vector3.Normalize(Player.transform.position - Grunts[0].GetChild(0).position);
                            }

                            if (!Grunts[0].GetChild(1).GetComponent<RagdollEnemy>().isDead)
                            {
                                var proj = Instantiate(GruntProjectile, Grunts[0].GetChild(1).position, Quaternion.identity);
                                proj.SetActive(true);
                                proj.GetComponent<Rigidbody2D>().velocity = 3f * Vector3.Normalize(Player.transform.position - Grunts[0].GetChild(1).position);
                            }

                            if (!Grunts[0].GetChild(2).GetComponent<RagdollEnemy>().isDead)
                            {
                                var proj = Instantiate(GruntProjectile, Grunts[0].GetChild(2).position, Quaternion.identity);
                                proj.SetActive(true);
                                proj.GetComponent<Rigidbody2D>().velocity = 3f * Vector3.Normalize(Player.transform.position - Grunts[0].GetChild(2).position);
                            }



                            attackTimer = 0;
                    }
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
                        BonesManager[i].Bone.position = Vector3.Lerp(BonesManager[i].InitialPosition.position, BonesManager[i].FinalPosition.position, BonesManager[i].fracLerp);
                        BonesManager[i].Bone.rotation = Quaternion.Lerp(BonesManager[i].InitialPosition.rotation, BonesManager[i].FinalPosition.rotation, BonesManager[i].fracLerp);

                        if (BonesManager[i].fracLerp > 0)
                        {
                            float temp = Mathf.Lerp(1, 1.25f, BonesManager[i].fracLerp);
                            BonesManager[i].Bone.localScale = new Vector3(temp * BonesManager[i].InitialPosition.localScale.x, temp * BonesManager[i].InitialPosition.localScale.y, 1);
                        }
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
                        MainCamera.GetComponent<CameraFollow>().CameraPan(5f);
                        Status = State.Dead;
                    }

                    attackTimer += Time.deltaTime;
                    if (attackTimer > 2 && Vector3.Distance(Player.transform.position, transform.position) < 10)
                    {
                        activeHand *= -1;
                        var proj = Instantiate(BossProjectile, transform.position + new Vector3(activeHand,1,-1), Quaternion.identity);
                        proj.SetActive(true);
                        proj.GetComponent<Rigidbody2D>().velocity = 4f * Vector3.Normalize(Player.transform.position -  (transform.position + new Vector3(activeHand, 1, -1)));
                        attackTimer = 0;
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

        MainCamera.GetComponent<CameraFollow>().CameraPan(6.5f);

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
