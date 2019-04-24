using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.ImageEffects;

enum PauseMenuOption
{
    SkillTree = 0,
    Resume,
    Manual,
    Options,
    RestartFromCheckpoint,
    ReturnHubWorld,
    ReturnMainMenu
}

enum PauseMenuState
{
    None = 0,
    Main,
    Manual,
    Options,
    SkillTree
}

public class PauseMenu : MonoBehaviour {
    public GameObject pauseMenu;

    public Image[] menuTextImages;
    public GameObject cursor;
    public Image black;
    PauseMenuOption selectIndex;
    PauseMenuOption prevIndex;
    PauseMenuState menuState;
    Color transparentColor;
    Color disabledColor;
    int axisDirectionPressed;
    int axisDirection;
    float minAxis = 0.5f;
    public bool disableReturnToHubWorld;
    public bool disableSkillTree;
    public bool disableOptions;
    bool disableRestartFromCheckpoint;

    public static PauseMenu sharedInstance;
    [System.NonSerialized]
    public bool GamePaused;
    [System.NonSerialized]
    public bool PauseMenuDisabled = false;

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
        transparentColor = new Color(1f, 1f, 1f, 0.5f);
        disabledColor = new Color(1f, 1f, 1f, 0.2f);
        disableRestartFromCheckpoint = (YinYangCheckpoint.checkpointIndex == -1);
        //InitializeText();
    }
	
	void Update () {
		if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P) || Input.GetButtonDown("PS4Options")) && (menuState == PauseMenuState.Main || menuState == PauseMenuState.Options) && !PauseMenuDisabled)
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
            disableRestartFromCheckpoint = (YinYangCheckpoint.checkpointIndex == -1);
            selectIndex = (disableSkillTree) ? PauseMenuOption.Resume : PauseMenuOption.SkillTree;
            prevIndex = selectIndex;
            InitializeText();
        }
    }

    void CheckAxis()
    {
        if (Input.GetAxis("Vertical") >= minAxis && axisDirection != 1)
        {
            axisDirection = 1;
            axisDirectionPressed = 1;
        }
        else if (Input.GetAxis("Vertical") <= -minAxis && axisDirection != -1)
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
        bool moveDown = false;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || axisDirectionPressed == 1)
        {
            selectIndex--;
            WrapAround();
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow) || axisDirectionPressed == -1)
        {
            selectIndex++;
            moveDown = true;
            WrapAround();
        }

        //Skip return to Hub World if on a tutorial level
        while ((disableReturnToHubWorld && selectIndex == PauseMenuOption.ReturnHubWorld) ||
            (disableSkillTree && selectIndex == PauseMenuOption.SkillTree) ||
            (disableOptions && selectIndex == PauseMenuOption.Options) ||
            (disableRestartFromCheckpoint && selectIndex == PauseMenuOption.RestartFromCheckpoint))
        {
            selectIndex += (moveDown) ? 1 : -1;
            WrapAround();
        }

        if (selectIndex != prevIndex)
        {
            UpdateMenu(selectIndex, prevIndex);
            prevIndex = selectIndex;
        }
    }

    void WrapAround()
    {
        //Wrap around
        if ((int)selectIndex > menuTextImages.Length - 1)
            selectIndex -= menuTextImages.Length;
        else if (selectIndex < 0)
            selectIndex += menuTextImages.Length;
    }

    void UpdateMenu(PauseMenuOption newIndex, PauseMenuOption oldIndex)
    {
        //menuText[(int)oldIndex].color = transparentColor;
        //menuText[(int)newIndex].color = Color.white;

        cursor.transform.position = menuTextImages[(int)newIndex].transform.position;
    }

    //Return from skill tree back to pause menu
    //Function called from skill tree
    public void ReturnToPauseMenu(string currentScene)
    {
        StartCoroutine(TransitionScene(false, currentScene));
        menuState = PauseMenuState.None;
    }

    //Because Time.scale is 0, Coroutines will not be framerate-independent
    //Use last 2 parameters only if toNewScene is true
    IEnumerator TransitionScene(bool toNewScene, string sceneName = "", PauseMenuState newState = PauseMenuState.None)
    {
        if (!toNewScene)
            SceneManager.UnloadSceneAsync(sceneName);
        Color col = Color.black;
        for (float i = 0; i < 1f; i += 0.05f)
        {
            col.a = (toNewScene) ? i : (1 - i);
            black.color = col;
            yield return 0;
        }
        if (toNewScene)
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        else
        {
            col.a = 0f;
            black.color = col;
        }
        menuState = (toNewScene) ? newState : PauseMenuState.Main;
    }

    void CheckForConfirmButton()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("PS4Jump"))
        {
            switch (selectIndex)
            {
                //Skill Tree
                case PauseMenuOption.SkillTree:
                    if (!disableSkillTree)
                    {
                        StartCoroutine(TransitionScene(true, "SkillTree", PauseMenuState.SkillTree));
                        menuState = PauseMenuState.None;
                    }
                    break;

                //Resume
                case PauseMenuOption.Resume:
                    PauseUnpause();
                    break;

                //Manual
                case PauseMenuOption.Manual:
                    StartCoroutine(TransitionScene(true, "Manual", PauseMenuState.Manual));
                    menuState = PauseMenuState.None;
                    break;

                //Options
                case PauseMenuOption.Options:
                    //menuState = MenuState.Options;
                    break;

                //Restart from Checkpoint
                case PauseMenuOption.RestartFromCheckpoint:
                    FindObjectOfType<PlayerLifeController>().Respawn();
                    PauseUnpause();
                    break;

                //Return to Hub World
                case PauseMenuOption.ReturnHubWorld:
                    if (!disableReturnToHubWorld)
                    {
                        Time.timeScale = 1f;
                        SceneManager.LoadScene("HubWorld");
                        YinYangCheckpoint.checkpointIndex = -1;
                    }
                    break;

                //Return to Main Menu
                case PauseMenuOption.ReturnMainMenu:
                    Time.timeScale = 1f;
                    SceneManager.LoadScene("MainMenuBasic");
                    ResetAllGameProgress();
                    break;
            }
        }
    }

    void ResetAllGameProgress()
    {
        YinYangCheckpoint.checkpointIndex = -1;
        SkillTree.info.nodesActivated = SkillNodes.None;
    }

    //Used by InitializeText() to check if Skill Tree and/or Hub World menu options are disabled or not
    bool IsTextDisabled(int index)
    {
        return ((index == (int)PauseMenuOption.ReturnHubWorld && disableReturnToHubWorld) ||
            (index == (int)PauseMenuOption.SkillTree && disableSkillTree) ||
            (index == (int)PauseMenuOption.Options && disableOptions) ||
            (index == (int)PauseMenuOption.RestartFromCheckpoint && disableRestartFromCheckpoint));
    }

    void InitializeText()
    {
        for (int i = 0; i < menuTextImages.Length; i++)
        {
            //menuText[i].text = menuStrings[i];

            //if (i != (int)selectIndex)
            //{
            //    menuText[i].color = IsTextDisabled(i) ? disabledColor : transparentColor;
            //}
            //else
            //{
            //    menuText[i].color = Color.white;
            //}

            menuTextImages[i].color = IsTextDisabled(i) ? disabledColor : Color.white;
        }

        cursor.transform.position = menuTextImages[(int)selectIndex].transform.position;
    }
}
