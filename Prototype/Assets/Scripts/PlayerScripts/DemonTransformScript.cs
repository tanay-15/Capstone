using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonTransformScript : MonoBehaviour
{

    public bool DemonModeActive = false;
    public GameObject bat;
    GameObject Human;
    GameObject Demon;

    Animator PlayerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = transform.GetComponent<PlayerStates>().PlayerAnimator;
        Human = transform.GetComponent<PlayerStates>().Human;
        Demon = transform.GetComponent<PlayerStates>().Demon;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("LeftTrigger2"))
        {
            if (!DemonModeActive)
            {
                StartCoroutine(DelayedTransform(true));
            }
            else
            {
                StartCoroutine(DelayedTransform(false));
            }

            for (int i = 0; i < 50; i++)
            {
                var Bat = Instantiate(bat, transform.position + new Vector3(0, 100, 0), Quaternion.identity);
                Bat.GetComponent<BatScript>().player = gameObject;
            }

        }
    }

    IEnumerator DelayedTransform(bool toDemon)
    {
        yield return new WaitForSeconds(2.0f);

        DemonSwitch[] DemonSwitches = FindObjectsOfType<DemonSwitch>();
        foreach (DemonSwitch s in DemonSwitches)
        {
            s.Switch(toDemon);
        }

        yield return new WaitForSeconds(0.7f);

        DemonModeActive = toDemon;
        Demon.gameObject.SetActive(toDemon);
        Human.gameObject.SetActive(!toDemon);

        transform.GetComponent<PlayerStates>().PlayerAnimator = toDemon ? Demon.GetComponent<Animator>() : Human.GetComponent<Animator>();

        //Levitation.sharedInstance.SetActive(!toDemon);

    }
}
