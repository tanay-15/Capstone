using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

enum PauseMenuOption
{
    Resume = 0,
    SkillTree,
    Options,
    ReturnHubWorld,
    ReturnMainMenu
}

enum PauseMenuState
{
    None = 0,
    Main,
    Options,
    SkillTree
}

public class PauseMenu : MonoBehaviour {

    public GameObject pauseMenu;

    public Text[] menuText;
    public Image black;
    string[] menuStrings;
    PauseMenuOption selectIndex;
    PauseMenuOption prevIndex;
    PauseMenuState menuState;
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

        selectIndex = PauseMenuOption.Resume;
        prevIndex = selectIndex;
        menuState = PauseMenuState.Main;
        menuStrings = new string[] { "Resume", "Skill Tree", "Options", "Return to Hub World", "Return to Main Menu" };
        transparentColor = new Color(1f, 1f, 1f, 0.5f);
        disabledColor = new Color(1f, 1f, 1f, 0.2f);
        //InitializeText();
    }
	
	void Update () {
        Debug.Log(menuState);
		if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("PS4Options")) && (menuState == PauseMenuState.Main || menuState == PauseMenuState.Options))
        {
            PauseUnpause();
        }
        if (GamePaused)
        {
            CheckAxis();
            if (menuState == PauseMenuState.Main || menuState == PauseMenuState.Options)
            {
                CheckForArrowKeys();
                CheckForConfirmButton();
            }
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
            selectIndex = PauseMenuOption.Resume;
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
            if (selectIndex == PauseMenuOption.ReturnHubWorld)
            {
                selectIndex += (prevIndex < selectIndex) ? 1 : -1;
            }
        }

        //Wrap around
        if ((int)selectIndex > menuStrings.Length - 1)
            selectIndex -= menuStrings.Length;
        else if (selectIndex < 0)
            selectIndex += menuStrings.Length;

        if (selectIndex != prevIndex)
        {
            UpdateMenu(selectIndex, prevIndex);
            prevIndex = selectIndex;
        }
    }

    void UpdateMenu(PauseMenuOption newIndex, PauseMenuOption oldIndex)
    {
        menuText[(int)oldIndex].color = transparentColor;
        menuText[(int)newIndex].color = Color.white;
    }

    //Return from skill tree back to pause menu
    //Function called from skill tree
    public void ReturnToPauseMenu()
    {
        StartCoroutine(TransitionToOrFromSkillTree(false));
        menuState = PauseMenuState.None;
    }

    //Because Time.scale is 0, Coroutines will not be framerate-independent
    IEnumerator TransitionToOrFromSkillTree(bool toTree)
    {
        if (!toTree)
            SceneManager.UnloadSceneAsync("SkillTree");
        Color col = Color.black;
        for (float i = 0; i < 1f; i += 0.05f)
        {
            col.a = (toTree) ? i : (1 - i);
            black.color = col;
            yield return 0;
        }
        if (toTree)
            SceneManager.LoadScene("SkillTree", LoadSceneMode.Additive);
        menuState = (toTree) ? PauseMenuState.SkillTree : PauseMenuState.Main;
    }

    void CheckForConfirmButton()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("PS4Jump"))
        {
            switch (selectIndex)
            {
                //Resume
                case PauseMenuOption.Resume:
                    PauseUnpause();
                    break;

                //Skill Tree
                case PauseMenuOption.SkillTree:
                    StartCoroutine(TransitionToOrFromSkillTree(true));
                    menuState = PauseMenuState.None;
                    break;

                //Options
                case PauseMenuOption.Options:
                    //menuState = MenuState.Options;
                    break;

                //Return to Hub World
                case PauseMenuOption.ReturnHubWorld:
                    if (!disableReturnToHubWorld)
                    {
                        Time.timeScale = 1f;
                        SceneManager.LoadScene("HubWorld");
                    }
                    break;

                //Return to Main Menu
                case PauseMenuOption.ReturnMainMenu:
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

            if (i != (int)selectIndex)
                menuText[i].color = (i == (int)PauseMenuOption.ReturnHubWorld && disableReturnToHubWorld) ? disabledColor : transparentColor;
            else
                menuText[i].color = Color.white;
        }
    }
}
