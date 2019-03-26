using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonTransformScript : MonoBehaviour
{

    public bool DemonModeActive = false;
    public float HumanSpeed = 4f;
    public float DemonSpeed = 6f;
    bool transitioning;

    public GameObject bat;
    GameObject Human;
    GameObject Demon;

    float rageDrainRateNormal = 0.04f;
    float rageDrainRateSkill = 0.032f;

    Animator PlayerAnimator;

    private float DrainRate
    {
        get
        {
            return ((SkillTree.info.nodesActivated & SkillNodes.D_1) == SkillNodes.D_1) ? rageDrainRateSkill : rageDrainRateNormal;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = transform.GetComponent<PlayerStates>().PlayerAnimator;
        Human = transform.GetComponent<PlayerStates>().Human;
        Demon = transform.GetComponent<PlayerStates>().Demon;
        transitioning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!transitioning && Time.timeScale > 0f && (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("LeftTrigger2")))
        {
            if (!DemonModeActive && rageBar.sharedInstance.fillAmount >= 1f)
            {
                StartCoroutine(DelayedTransform(true));
            }
            else if (DemonModeActive && rageBar.sharedInstance.fillAmount >= 0f)
            {
                StartCoroutine(DelayedTransform(false));
            }
        }
        if (DemonModeActive)
        {
            if (rageBar.sharedInstance.fillAmount > 0f)
            {
                rageBar.sharedInstance.AddRage(-Time.deltaTime * DrainRate);
            }
            if (rageBar.sharedInstance.fillAmount <= 0f && !transitioning)
            {
                StartCoroutine(DelayedTransform(false));
            }
        }
    }

    void GenerateBats()
    {
        for (int i = 0; i < 50; i++)
        {
            var Bat = Instantiate(bat, transform.position + new Vector3(0, 100, 0), Quaternion.identity);
            Bat.GetComponent<BatScript>().player = gameObject;
        }
    }

    public IEnumerator DelayedTransform(bool toDemon)
    {
        GenerateBats();
        transitioning = true;
        yield return new WaitForSeconds(1.3f);  //2.0

        DemonSwitch[] DemonSwitches = FindObjectsOfType<DemonSwitch>();
        foreach (DemonSwitch s in DemonSwitches)
        {
            s.Switch(toDemon);
        }
        Levitation.sharedInstance.gameObject.SetActive(!toDemon);

        yield return new WaitForSeconds(0.7f);

        DemonModeActive = toDemon;
        Demon.gameObject.SetActive(toDemon);
        Human.gameObject.SetActive(!toDemon);

        GetComponent<PlayerStates>().PlayerAnimator = toDemon ? Demon.GetComponent<Animator>() : Human.GetComponent<Animator>();
        GetComponent<PlayerStates>().speed = toDemon ? DemonSpeed : HumanSpeed;
        //Levitation.sharedInstance.SetActive(!toDemon);

        yield return new WaitForSeconds(0.5f);

        //0 - Levitation icon
        //2 - Orbit icon
        UIIcons.sharedInstance.SwitchIcons(0, 2, !toDemon);
        PlayerLife.sharedInstance.SetDemonIcon(toDemon);
        transitioning = false;

    }
}
