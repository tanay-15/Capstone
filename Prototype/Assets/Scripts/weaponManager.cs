using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponManager : MonoBehaviour {

    public GameObject activeWeapon;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void updateWeapon(GameObject newWeapon)
    {
        activeWeapon = newWeapon;
        GetComponent<SpriteRenderer>().sprite = activeWeapon.GetComponent<SpriteRenderer>().sprite;
    }
}
