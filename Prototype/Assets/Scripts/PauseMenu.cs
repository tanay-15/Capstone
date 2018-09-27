using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class PauseMenu : MonoBehaviour {

    public static PauseMenu sharedInstance;
    public GameObject pauseMenu;
    [System.NonSerialized]
    public bool GamePaused;

    Grayscale grayscale;

    static PauseMenu()
    {
        sharedInstance = null;
    }
	void Start () {
        pauseMenu.SetActive(false);
        grayscale = FindObjectOfType<Grayscale>();
        grayscale.enabled = false;
        GamePaused = false;
        if (sharedInstance != null)
            Destroy(sharedInstance);
        sharedInstance = this;
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            GamePaused = !GamePaused;
            pauseMenu.SetActive(GamePaused);
            grayscale.enabled = GamePaused;
            Time.timeScale = (GamePaused) ? 0f : 1f;
        }
	}
}
