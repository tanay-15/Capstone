using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkeletonBossScript : MonoBehaviour {

    public GameObject Player;
    public GameObject MainCamera;
    public GameObject Boss;
    public GameObject Eyes;
    public GameObject GruntProjectile;
    public GameObject BossProjectile;
    public GameObject DemonicCircle;
    public GameObject HealthBar;

   Transform[] BossBones;
   Transform[] Grunts;

    float fracLerp = -2;
    float attackTimer = 0;
    float theta = 0;

    float Health = 60;
    float MaxHealth = 60;
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
        MaxHealth = Health;
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


                    // Demon Circle #1
                    DemonicCircle.SetActive(true);
                    theta += Time.deltaTime;
                    DemonicCircle.transform.Rotate(new Vector3(0, 0, 20 * Time.deltaTime));
                    Color originalColor = DemonicCircle.GetComponent<SpriteRenderer>().color;
                    DemonicCircle.GetComponent<SpriteRenderer>().color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Cos(theta) * Mathf.Cos(theta));
                    DemonicCircle.transform.localScale = new Vector3(((Mathf.Cos(theta*2) * 0.1f)/2) + 0.2f, ((Mathf.Cos(theta*2) * 0.1f)/2) + 0.2f, 1);

                    if (fracLerp > 1)
                    {
                        Status = State.Active;
                        Eyes.SetActive(true);
                        HealthBar.SetActive(true);
                        HealthBar.transform.parent = GameObject.Find("Canvas").transform;
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


                    // Death Condition
                    if (Health <= 0)
                    {
                        MainCamera.GetComponent<CameraFollow>().CameraPan(5f,0.5f);
                        Status = State.Dead;
                    }

                    attackTimer += Time.deltaTime;
                    float attackCD = (Health > 30 ? 2 : 1);
                    if (attackTimer > attackCD && Vector3.Distance(Player.transform.position, transform.position) < 10)
                    {
                        activeHand *= -1;
                        var proj = Instantiate(BossProjectile, transform.position + new Vector3(activeHand,1,-1), Quaternion.identity);
                        proj.SetActive(true);
                        proj.GetComponent<Rigidbody2D>().velocity = 4f * Vector3.Normalize(Player.transform.position -  (transform.position + new Vector3(activeHand, 1, -1)));
                        attackTimer = 0;
                    }

                    
                    var HealthBarPos = Camera.main.WorldToScreenPoint(transform.position + new Vector3(0,-2,0));
                    HealthBar.transform.position = HealthBarPos;
                    HealthBar.transform.GetChild(0).GetChild(0).GetComponent<Image>().fillAmount = Health / MaxHealth;


                    // Demon Circle #2
                    theta += Time.deltaTime;
                    DemonicCircle.transform.Rotate(new Vector3(0,0, 20 * Time.deltaTime));
                    Color originalColor = DemonicCircle.GetComponent<SpriteRenderer>().color;
                    DemonicCircle.GetComponent<SpriteRenderer>().color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Cos(theta) * Mathf.Cos(theta));
                    DemonicCircle.transform.localScale = new Vector3(((Mathf.Cos(theta * 2) * 0.1f) / 2) + 0.2f, ((Mathf.Cos(theta * 2) * 0.1f) / 2) + 0.2f, 1);

                    break;
            }

            case State.Dead:
            {
                    
                    GetComponent<BoxCollider2D>().enabled = false;
                    Eyes.SetActive(false);
                    DemonicCircle.SetActive(false);
                    HealthBar.transform.parent = transform;
                    HealthBar.SetActive(false);

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
       
        yield return new WaitForSeconds(1f);

        MainCamera.GetComponent<CameraFollow>().CameraPan(6.5f,0.35f);

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
