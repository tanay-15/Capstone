using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossSpiderLevel : MonoBehaviour {

    // Use this for initialization
    public GameObject player;
    public GameObject door;
    public GameObject boss;

    private float playerhealth;
    public GameObject playerUI;

    public Slider bossHPbar;

    private float bosshealth;
	void Start () {

        bossHPbar.gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

        bosshealth = boss.GetComponent<BossSpider>().health;
        playerhealth = playerUI.GetComponent<PlayerLife>().currentLife;

      
     

        if (bosshealth <= 0)
        {
            door.SetActive(true);
            SceneManager.LoadScene("MainMenuBasic");

        }
        else
        {
            door.SetActive(false);
        }

        if(playerhealth <=0)
        {
            SceneManager.LoadScene("Enemy");
        }

       // BossCheck();
	}

    void BossCheck()
    {
        if(boss.activeInHierarchy == true)
        {
            bossHPbar.gameObject.SetActive(true);

        }

        else
        {
            bossHPbar.gameObject.SetActive(false);
        }
    }
}
