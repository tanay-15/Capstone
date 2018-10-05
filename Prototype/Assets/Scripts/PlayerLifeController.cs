using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLifeController : MonoBehaviour {

    SpriteRenderer[] renderers;

	void Start () {
        renderers = GetComponentsInChildren<SpriteRenderer>();
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
            FindObjectOfType<CameraFollow>().ShakeCamera();
            BlinkRed(true);
            Invoke("Unblink", 0.1f);
            
        }
    }

    //Eventually make these two into one function or coroutine
    void Unblink()
    {
        BlinkRed(false);
    }

    void BlinkRed(bool blink)
    {
        foreach (SpriteRenderer r in renderers)
        {
            r.color = (blink) ? Color.red : Color.white;
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
