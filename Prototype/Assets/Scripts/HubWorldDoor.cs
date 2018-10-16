﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HubWorldDoor : MonoBehaviour {

    public TextMesh levelDisplayText;
    public string levelName;
    public string levelDisplayName;
    public bool locked;
    bool player;

    private void Start()
    {
        player = false;
        levelDisplayText.text = (locked) ? "Locked" : levelDisplayName;
        levelDisplayText.color = (locked) ? Color.gray : Color.white;
        levelDisplayText.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl) && player && !locked)
        {
            SceneManager.LoadScene(levelName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!locked)
            {
                player = true;
                collision.gameObject.SendMessage("Show", true);
            }
            levelDisplayText.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!locked)
            {
                player = false;
                collision.gameObject.SendMessage("Show", false);
            }
            levelDisplayText.gameObject.SetActive(false);
        }
    }
}