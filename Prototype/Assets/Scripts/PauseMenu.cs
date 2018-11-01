using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenu;

    public Text[] menuText;
    string[] menuStrings;
    int selectIndex;
    int prevIndex;
    Color transparentColor;
    Color disabledColor;
    int axisDirectionPressed;
    int axisDirection;
    public bool disableReturnToHubWorld;

    public static PauseMenu sharedInstance;
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
        if (grayscale != null)  grayscale.enabled = false;
        GamePaused = false;
        if (sharedInstance != null)
            Destroy(sharedInstance);
        sharedInstance = this;

        selectIndex = 0;
        prevIndex = selectIndex;
        menuStrings = new string[] { "Resume", "Options", "Return to Hub World", "Return to Main Menu" };
        transparentColor = new Color(1f, 1f, 1f, 0.5f);
        disabledColor = new Color(1f, 1f, 1f, 0.2f);
        //InitializeText();
    }
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("PS4Options"))
        {
            PauseUnpause();
        }
        if (GamePaused)
        {
            CheckAxis();
            CheckForArrowKeys();
            CheckForConfirmButton();
            ResetCheckAxis();
        }
    }

    void PauseUnpause()
    {
        GamePaused = !GamePaused;
        pauseMenu.SetActive(GamePaused);
        if (grayscale != null) grayscale.enabled = GamePaused;
        Time.timeScale = (GamePaused) ? 0f : 1f;

        if (GamePaused)
        {
            selectIndex = 0;
            prevIndex = selectIndex;
            InitializeText();
        }
    }

    void CheckAxis()
    {
        if (Input.GetAxis("Vertical") == 1 && axisDirection != 1)
        {
            axisDirection = 1;
            axisDirectionPressed = 1;
        }
        else if (Input.GetAxis("Vertical") == -1 && axisDirection != -1)
        {
            axisDirection = -1;
            axisDirectionPressed = -1;
        }
        else if (Input.GetAxis("Vertical") == 0 && axisDirection != 0)
        {
            axisDirection = 0;
            axisDirectionPressed = 0;
        }
    }

    void ResetCheckAxis()
    {
        axisDirectionPressed = 0;
    }

    void CheckForArrowKeys()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || axisDirectionPressed == 1)
        {
            selectIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || axisDirectionPressed == -1)
        {
            selectIndex++;
        }

        //Skip return to Hub World if on a tutorial level
        if (disableReturnToHubWorld)
        {
            if (selectIndex == 2)
            {
                selectIndex += (prevIndex < selectIndex) ? 1 : -1;
            }
        }

        //Wrap around
        if (selectIndex > menuStrings.Length - 1)
            selectIndex -= menuStrings.Length;
        else if (selectIndex < 0)
            selectIndex += menuStrings.Length;

        if (selectIndex != prevIndex)
        {
            UpdateMenu(selectIndex, prevIndex);
            prevIndex = selectIndex;
        }
    }

    void UpdateMenu(int newIndex, int oldIndex)
    {
        menuText[oldIndex].color = transparentColor;
        menuText[newIndex].color = Color.white;
    }

    void CheckForConfirmButton()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("PS4Jump"))
        {
            switch (selectIndex)
            {
                //Resume
                case 0:
                    PauseUnpause();
                    break;

                //Options
                case 1:
                    break;

                //Return to Hub World
                case 2:
                    if (!disableReturnToHubWorld)
                    {
                        Time.timeScale = 1f;
                        SceneManager.LoadScene("HubWorld");
                    }
                    break;

                //Return to Main Menu
                case 3:
                    Time.timeScale = 1f;
                    SceneManager.LoadScene("MainMenuBasic");
                    break;
            }
        }
    }

    void InitializeText()
    {
        for (int i = 0; i < menuStrings.Length; i++)
        {
            menuText[i].text = menuStrings[i];

            if (i != selectIndex)
                menuText[i].color = (i == 2 && disableReturnToHubWorld) ? disabledColor : transparentColor;
            else
                menuText[i].color = Color.white;
        }
    }
}
