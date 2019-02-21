using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLifeController : MonoBehaviour {

    SpriteRenderer[] renderers;

    [System.NonSerialized]
    public Vector3 startPosition;
    [System.NonSerialized]
    public Vector3 respawnPosition;

    Rigidbody2D rb;
    int respawnBlinkLoop = 5;
    bool invincible;

    IEnumerator respawnBlink = null;
    Color transparentColor;

	void Start () {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        startPosition = transform.position;
        respawnPosition = startPosition;

        invincible = false;
        transparentColor = new Color(1f, 1f, 1f, 0.5f);
	}
	
	void Update () {

        if (PlayerLife.sharedInstance.currentLife <= 0)
        {
            gameObject.SetActive(false);
            Invoke("Respawn", 1.2f);
        }

        //Thread testing
        //if (Input.GetKeyDown(KeyCode.T))
        //{
        //    WaitCallback callback = new WaitCallback(Count);
        //    ThreadPool.QueueUserWorkItem(callback);
        //}
    }

    //void Count(object obj)
    //{
    //    Debug.Log("Starting");
    //    for (int i = -500000000; i < 500000000; i++)
    //    {

    //    }
    //    Debug.Log("Done");
    //}

    void StartInvincibleBlink(int loopCount)
    {
        if (respawnBlink != null)
            StopCoroutine(respawnBlink);
        respawnBlink = RespawnBlink(loopCount);
        StartCoroutine(respawnBlink);
    }

    IEnumerator RespawnBlink(int loopCount)
    {
        invincible = true;
        for (int i = 0; i < loopCount; i++)
        {
            yield return new WaitForSeconds(0.2f);
            SetRendererColors(transparentColor);
            yield return new WaitForSeconds(0.2f);
            SetRendererColors(Color.white);
        }
        invincible = false;
    }

    public void SetRespawnPosition(Vector3 pos)
    {
        respawnPosition = pos;
    }

    public void Respawn()
    {
        PlayerLife.sharedInstance.ResetLife();
        gameObject.SetActive(true);
       
        rb.velocity = Vector3.zero;
        transform.position = respawnPosition;
        StartInvincibleBlink(respawnBlinkLoop);

        if (GetComponent<PlayerStates>() != null)
        {
            GetComponent<PlayerStates>().RestartInAir();
        }

        //if (GetComponent<DemonModeScript>().DemonModeActive)
        //{ GetComponent<DemonModeScript>().Transformation(); }

        GetComponent<PlayerStates>().status = PlayerStates.State.Default;
        GetComponent<PlayerStates>().onStateStart = true;
        GetComponent<PlayerStates>().movable = true;
    }

    //void ResetLevel()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    //}

    public void GetHit(int addLife)
    {
        if (invincible || transform.GetComponent<PlayerStates>().invulnerable) return;
        StartInvincibleBlink(2);
        PlayerLife.sharedInstance.AddLife(addLife);
        FindObjectOfType<CameraFollow>().ShakeCamera();
        BlinkRed(true);
        Invoke("Unblink", 0.1f);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
     //   if (col.gameObject.tag == "Enemy")
       // {
            //PlayerLife.sharedInstance.AddLife(-10);
            //FindObjectOfType<CameraFollow>().ShakeCamera();
            //BlinkRed(true);
            //Invoke("Unblink", 0.1f);
         //   GetHit(-10);
            
       // }

        
    }

    //Eventually make these two into one function or coroutine
    void Unblink()
    {
        BlinkRed(false);
    }

    void SetRendererColors(Color col)
    {
        foreach (SpriteRenderer r in renderers)
        {
            r.color = col;
        }
    }

    void BlinkRed(bool blink)
    {
        SetRendererColors((blink) ? Color.red : Color.white);
        //foreach (SpriteRenderer r in renderers)
        //{
        //    r.color = (blink) ? Color.red : Color.white;
        //}
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        
        if (col.gameObject.tag == "Instant Death")
        {
            Respawn();
        }
    }
}
