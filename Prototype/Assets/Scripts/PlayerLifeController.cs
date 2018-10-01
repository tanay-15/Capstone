using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLifeController : MonoBehaviour {

	void Start () {
		
	}
	
	void Update () {

        if (PlayerLife.sharedInstance.currentLife <= 0)
        {
            gameObject.SetActive(false);
            Invoke("ResetLevel", 1.2f);
        }

    }

    void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            PlayerLife.sharedInstance.AddLife(-10);
            
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.gameObject.tag == "Instant Death")
        {
            ResetLevel();
        }
    }
}
