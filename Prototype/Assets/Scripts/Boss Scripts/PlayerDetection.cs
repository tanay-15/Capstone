using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject bosshealthBar;
    public GameObject boss;

    void Start()
    {
        bosshealthBar.SetActive(false);
        boss.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            bosshealthBar.SetActive(true);
            boss.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
