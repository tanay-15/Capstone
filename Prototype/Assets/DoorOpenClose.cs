using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DoorOpenClose : MonoBehaviour
{
    public bool open = false;
    public bool inOpen = false;
    public Animator PlayerAnimator;
    AudioManager audioManager;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();

    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!inOpen)
        {
            if (collision.gameObject.name == "Player")
            {
                open = true;
                StartCoroutine("OpenDoor");
            }
        }
    }

    IEnumerator OpenDoor()
    {
        inOpen = true;
        PlayerAnimator.Play("Open");
        audioManager.Play("Door");
        yield return new WaitForSeconds(1f);
        PlayerAnimator.Play("Up");
        yield return new WaitForSeconds(4f);

        PlayerAnimator.Play("Close");
        audioManager.Play("Door");
        yield return new WaitForSeconds(1f);
        PlayerAnimator.Play("Normal");

        inOpen = false;
    }
}
