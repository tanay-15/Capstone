using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShield : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if(collision.gameObject.GetComponent<PlayerStates>().status == PlayerStates.State.Melee)
            {
                Debug.Log("PLayer hitrs me");
                this.transform.parent.gameObject.SendMessage("applyDamage", 10f);
            }
        }
    }
}
