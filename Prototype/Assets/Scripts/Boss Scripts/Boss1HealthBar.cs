using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boss1HealthBar : MonoBehaviour {

    // Use this for initialization

    public GameObject boss;
    public int boss_health;

    public Text hptext;
	void Start () {
		

	}
	
	// Update is called once per frame
	void Update () {

        boss_health = (int)boss.GetComponent<Boss1>().GetHealth();

        hptext.text = " Health: " + boss_health;
	}
}
