using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpenClose : MonoBehaviour
{
    public bool open = false;
    public bool inOpen = false;
    public Animator PlayerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        PlayerAnimator = GetComponent<Animator>();
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
        yield return new WaitForSeconds(1f);
        PlayerAnimator.Play("Up");
        yield return new WaitForSeconds(4f);

        PlayerAnimator.Play("Close");
        yield return new WaitForSeconds(1f);
        PlayerAnimator.Play("Normal");

        inOpen = false;
    }
}
