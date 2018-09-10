using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponPickup : MonoBehaviour {

    public GameObject[] weapons;
    public GameObject weaponHere;

    // Use this for initialization
    void Start()
    {
        weaponHere = weapons[Random.Range(0, weapons.Length)];
        GetComponent<SpriteRenderer>().sprite = weaponHere.GetComponent<SpriteRenderer>().sprite;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "player")
        {
            other.transform.Find("handEnd").GetComponent<weaponManager>().updateWeapon(weaponHere);
        }
    }
}
