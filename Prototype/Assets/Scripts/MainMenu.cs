﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Start at -3 so that Main = 0
enum MainMenuState
{
    None = -4,
    PressAnyButton,
    Transitioning,
    IntroSequence,

    Main,
    Options,
    Credits,
}

[System.Flags]
public enum MenuButtons
{
    None    = 0,
    Left    = 1,
    Up      = 1 << 1,
    Right   = 1 << 2,
    Down    = 1 << 3,
    Confirm = 1 << 4,
    Back    = 1 << 5
}

public class MainMenu : MonoBehaviour
{
    public string newGameLevel;
    public Text pressAnyButtonText;
    public Text[] menuText;
    public Text[] menuText2;
    public RectTransform credits;
    public GameObject cursor;
    public Transform[] cameraFocus; //0 start position, 1 options menu, 2 player
    Color nonSelectedColor = Color.yellow;// new Color(0.25f, 0.25f, 0.25f, 1f);
    Color selectedColor = Color.red;
    public Color[] menuTextColors;
    public AnimationCurve[] curves;
    public GameObject levitationTesting;
    MainMenuState state = MainMenuState.None;
    MenuButtons buttonsPressed = MenuButtons.None;
    string[][] menuStrings;
    bool[] alignLeft;
    int listCount;
    int[] selectIndices;
    float[] transitionSpeeds = { 4.0f, 1.3f };
    float[] delayBetweenLinesTransitioning = { 0.05f, 0.1f };
    float baseTextScale;
    float count;

    int resolutionIndex;
    float cursorRotation;
    float cursorRotationSpeed = -40f;

    int axisDirectionPressed;
    int axisDirection;
    float minAxis = 0.5f;

    CameraFollow cameraFollow;
    Resolution[] resolutions;
    bool confirmEnabled = true;

    bool SameResolution(Resolution a, Resolution b)
    {
        return (a.width == b.width && a.height == b.height && a.refreshRate == b.refreshRate);
    }

    bool SameResolution(Resolution r, int width, int height, int refreshRate)
    {
        return (r.width == width && r.height == height && r.refreshRate == refreshRate);
    }

    int CurrentSelectIndex
    {
        get
        {
            return selectIndices[(int)state];
        }
        set
        {
            selectIndices[(int)state] = value;
        }
    }

    void Start()
    {
        //Not sure if setting targetFrameRate actually works if vSyncCount is 1
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 60;

        ResetStaticFields();
        GetResolutions();
        credits.localScale = Vector3.zero;
        baseTextScale = menuText[0].gameObject.transform.localScale.x;
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        cursorRotation = 0f;
        Initialize();
        ChangeState(MainMenuState.PressAnyButton);
    }

    void GetResolutions()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
        resolutions = Screen.resolutions;
        for(int i = 0; i < resolutions.Length; i++)
        {
            if (SameResolution(resolutions[i], Screen.width, Screen.height, Application.targetFrameRate))
            {
                resolutionIndex = i;
                break;
            }
        }
    }

    void ResetStaticFields()
    {
        SkillTree.info.skillPoints = 0;
        SkillTree.info.nodesActivated = SkillNodes.None;
    }

    //Initialize arrays, and make all menu option text invisible
    void Initialize()
    {
        count = 0f;
        //Initialize menu strings
        menuStrings = new string[][] {
            //Main
            new string[] {"Play", "Options", "Credits", "Quit"},
            //new string[] { "New Game", "Continue", "Options", "Quit" },

            //FileSelect
            //new string[] { "File 1", "File 2", "File 3", "Back" },

            //Options
            new string[] {"Resolution:", "Full Screen:", "VSync:", "Joystick sensitivity for Levitation:", "Back" },

            //Credits
            new string[] {"Back"}
        };
        alignLeft = new bool[] { false, false, true };
        menuTextColors = new Color[] { nonSelectedColor, nonSelectedColor, nonSelectedColor, nonSelectedColor, nonSelectedColor };
        selectIndices = new int[] { 0, 0, 0 };
        for (int i = 0; i < menuText.Length; i++)
        {
            menuText[i].gameObject.transform.localScale = Vector3.zero;
            menuText2[i].gameObject.transform.localScale = Vector3.zero;
        }
    }

    void Update()
    {
        CheckAxis();
        CheckForDirectionsPressed();
        CheckForConfirmAndBackButtons();

        switch (state)
        {
            case MainMenuState.IntroSequence:
                OnIntroSequence();
                break;

            case MainMenuState.PressAnyButton:
                OnPressAnyButton();
                break;

            case MainMenuState.Main:
                OnMainMenu();
                break;

            //case MainMenuState.FileSelect:
            //    OnFileSelect();
            //    break;

            case MainMenuState.Options:
                OnOptionsMenu();
                break;

            case MainMenuState.Credits:
                OnCreditsMenu();
                break;
        }

        ResetButtonCheck();
        //MoveCursor();
        //RotateCursor();
    }

    void RotateCursor()
    {
        cursorRotation -= Time.deltaTime * cursorRotationSpeed;
        if (cursorRotation < 0)
            cursorRotation += 360;
        cursor.transform.rotation = Quaternion.Euler(0f, 0f, cursorRotation);
    }

    void MoveCursor()
    {
        if (state != MainMenuState.PressAnyButton && state != MainMenuState.Transitioning)
        {

            Vector3 position = menuText[CurrentSelectIndex].rectTransform.position;
            //position.x -= ((menuText[CurrentSelectIndex].preferredWidth / 2f) * menuText[CurrentSelectIndex].rectTransform.lossyScale.x) + 40f;
            position.x -= 40f;
            cursor.GetComponent<RectTransform>().position = position;
        }
    }

    void ChangeState(MainMenuState newState)
    {
        if (newState == state) return;

        if (newState == MainMenuState.PressAnyButton)
        {
            levitationTesting.SetActive(false);
            cursor.SetActive(false);
            pressAnyButtonText.gameObject.SetActive(true);
            state = newState;
        }
        else if (state == MainMenuState.PressAnyButton && newState == MainMenuState.Main)
        {
            StartCoroutine(FadeOutText(pressAnyButtonText, Transition(newState, curves[1], transitionSpeeds[1], delayBetweenLinesTransitioning[1], true)));
        }
        else if (newState == MainMenuState.IntroSequence)
        {
            state = MainMenuState.IntroSequence;
        }
        else
        {
            StartCoroutine(Transition(newState, curves[0], transitionSpeeds[0], delayBetweenLinesTransitioning[0], false));
        }
    }

    #region Menu states
    void OnPressAnyButton()
    {
        count += Time.deltaTime;
        float alpha = 0.75f + 0.25f * Mathf.Cos(count * 4f);
        pressAnyButtonText.color = new Color(1f, 1f, 1f, alpha);

        //Check for button input
        if (Input.anyKeyDown)
        {
            ChangeState(MainMenuState.Main);
        }
    }

    //Called every of the Main Menu
    void OnMainMenu()
    {
        HandleMenuNavigation();

        if ((buttonsPressed & MenuButtons.Confirm) == MenuButtons.Confirm)
        {
            //Highlighted over "New Game"
            if (CurrentSelectIndex == 0)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    SkillTree.info.nodesActivated = SkillNodes.HUMAN;
                    SceneManager.LoadScene("Ending");
                }
                else if (Input.GetKey(KeyCode.RightShift))
                {
                    SkillTree.info.nodesActivated = SkillNodes.DEMON;
                    SceneManager.LoadScene("Ending");
                }
                else
                {
                    //SceneManager.LoadScene(newGameLevel);
                    confirmEnabled = false;
                    FindObjectOfType<PlayerStates>().WakeUp();
                    ChangeState(MainMenuState.IntroSequence);
                    //cameraFollow.target = cameraFocus[2];
                    //Destroy(gameObject);
                }
                return;
            }

            //Continue
            //else if (CurrentSelectIndex == 1)
            //    ChangeState(MainMenuState.FileSelect);

            //Options
            else if (CurrentSelectIndex == 1)
                ChangeState(MainMenuState.Options);

            //Credits
            else if (CurrentSelectIndex == 2)
                ChangeState(MainMenuState.Credits);

            //Quit
            else if (CurrentSelectIndex == 3)
                Application.Quit();
        }

        //Go to Quit when pressing Escape
        if ((buttonsPressed & MenuButtons.Back) == MenuButtons.Back){
            CurrentSelectIndex = 3;
        }
        RotateCursor();
        MoveCursor();
    }

    //Called every frame of the File Select Menu
    void OnFileSelect()
    {
        HandleMenuNavigation();

        if ((buttonsPressed & MenuButtons.Confirm) == MenuButtons.Confirm)
        {
            //Back
            if (CurrentSelectIndex == 3)
                ChangeState(MainMenuState.Main);
        }

        if ((buttonsPressed & MenuButtons.Back) == MenuButtons.Back)
        {
            ChangeState(MainMenuState.Main);
        }
    }

    //Called every frame of the Options Menu
    void OnOptionsMenu()
    {
        HandleMenuNavigation();

        if ((buttonsPressed & MenuButtons.Left) == MenuButtons.Left)
        {   //Resolution
            if (CurrentSelectIndex == 0)
            {
                resolutionIndex--;
                if (resolutionIndex < 0)
                    resolutionIndex += resolutions.Length;

                //Set resolution
                Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen, resolutions[resolutionIndex].refreshRate);
                Application.targetFrameRate = resolutions[resolutionIndex].refreshRate;
                //Resolution text
                menuText2[0].text = resolutions[resolutionIndex].ToString();
            }

            //Full Screen
            else if (CurrentSelectIndex == 1)
            {
                menuText2[1].text = Screen.fullScreen ? "Off" : "On";
                Screen.fullScreen = !Screen.fullScreen;
            }

            //Vsync
            else if (CurrentSelectIndex == 2)
            {
                QualitySettings.vSyncCount = (QualitySettings.vSyncCount == 1) ? 0 : 1;
                menuText2[2].text = (QualitySettings.vSyncCount == 1) ? "On" : "Off";
            }

            //Joystick sensitivity
            else if (CurrentSelectIndex == 3)
            {
                Settings.sharedInstance.levitationJoystickSensitivity = Mathf.Max(Settings.minJoystick, Settings.sharedInstance.levitationJoystickSensitivity - 1);
                menuText2[3].text = Settings.sharedInstance.levitationJoystickSensitivity.ToString();
            }
        }

        if ((buttonsPressed & MenuButtons.Right) == MenuButtons.Right)
        {
            //Resolution
            if (CurrentSelectIndex == 0)
            {
                resolutionIndex++;
                if (resolutionIndex >= resolutions.Length)
                    resolutionIndex -= resolutions.Length;

                //Set resolution
                Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen, resolutions[resolutionIndex].refreshRate);
                Application.targetFrameRate = resolutions[resolutionIndex].refreshRate;
                //Resolution text
                menuText2[0].text = resolutions[resolutionIndex].ToString();
            }

            //Full Screen
            else if (CurrentSelectIndex == 1)
            {
                menuText2[1].text = Screen.fullScreen ? "Off" : "On";
                Screen.fullScreen = !Screen.fullScreen;
            }

            //Vsync
            else if (CurrentSelectIndex == 2)
            {
                QualitySettings.vSyncCount = (QualitySettings.vSyncCount == 1) ? 0 : 1;
                menuText2[2].text = (QualitySettings.vSyncCount == 1) ? "On" : "Off";
            }

            //Joystick sensitivity
            else if (CurrentSelectIndex == 3)
            {
                Settings.sharedInstance.levitationJoystickSensitivity = Mathf.Min(Settings.maxJoystick, Settings.sharedInstance.levitationJoystickSensitivity + 1);
                menuText2[3].text = Settings.sharedInstance.levitationJoystickSensitivity.ToString();
            }
        }

        if ((buttonsPressed & MenuButtons.Confirm) == MenuButtons.Confirm)
        {
            //Back
            if (CurrentSelectIndex == 4)
                ChangeState(MainMenuState.Main);
        }

        if ((buttonsPressed & MenuButtons.Back) == MenuButtons.Back)
        {
            ChangeState(MainMenuState.Main);
        }
        RotateCursor();
        MoveCursor();
    }

    void OnCreditsMenu()
    {
        HandleMenuNavigation();

        if ((buttonsPressed & MenuButtons.Confirm) == MenuButtons.Confirm)
        {
            //Back
            ChangeState(MainMenuState.Main);
        }

        if ((buttonsPressed & MenuButtons.Back) == MenuButtons.Back)
        {
            ChangeState(MainMenuState.Main);
        }
        RotateCursor();
        MoveCursor();
    }

    void OnIntroSequence()
    {
        //The confirmEnabled check may not be necessary if the IntroSequence case is before OnMainMenu in Update
        RotateCursor();
        if ((buttonsPressed & MenuButtons.Confirm) == MenuButtons.Confirm && confirmEnabled)
        {
            FindObjectOfType<FadeTransition>().StartSequence();
        }
        confirmEnabled = true;
    }
    #endregion

    IEnumerator ScaleCredits(bool show)
    {
        if (!show && credits.localScale == Vector3.zero)
        {
            yield return 0;
        }
        else
        {
            float scale = 0f;
            for (float i = 0; i < 1f; i += Time.deltaTime * 6f)
            {
                scale = (show) ? i : 1f - i;
                credits.localScale = new Vector3(1f, scale, 1f);
                yield return 0;
            }
            credits.localScale = (show) ? Vector3.one : Vector3.zero;
        }
    }
    
    //Called in the middle of transitioning from menus
    void SetText(int index, int listSize)
    {
        //Enable or disable levitation testing for Options menu
        if ((MainMenuState)index == MainMenuState.Options)
        {
            levitationTesting.SetActive(true);
            cameraFollow.target = cameraFocus[1];
        }
        else
        {
            levitationTesting.SetActive(false);
            cameraFollow.target = cameraFocus[0];
        }

        //if ((MainMenuState)index == MainMenuState.Credits)
        //{
        //    StartCoroutine(ScaleCredits(true));
        //}
        StartCoroutine(ScaleCredits((MainMenuState)index == MainMenuState.Credits));

        for (int i = 0; i < listSize; i++)
        {
            //Vector3 pos = menuText[i].rectTransform.anchoredPosition;
            //Vector3 pos2 = menuText2[i].rectTransform.anchoredPosition;
            //Set the string
            menuText[i].text = menuStrings[index][i];
            //Adjust if on file select menu
            //(May be more practical to calculate text in the ChangeState option, then pass it as parameters in the Transition coroutines rather than set here)
            //if ((MainMenuState)index == MainMenuState.FileSelect)
            //{
            //    //TODO: Handle multiple files
            //    if (i < 3)
            //    {
            //        menuText[i].text += " -Empty-";
            //        //menuText[i].color = Color.gray;
            //        menuTextColors[i] = Color.gray;
            //    }
            //    else
            //        menuTextColors[i] = nonSelectedColor;
            //}
            if ((MainMenuState)index == MainMenuState.Options)
            {
                //pos.x = -150;
                //pos2.x = 150;
                //menuText[i].rectTransform.anchoredPosition = pos;
                //menuText2[i].rectTransform.anchoredPosition = pos2;

                string text = "";
                switch (i)
                {
                    //Screen resolution
                    case 0:
                        text = resolutions[resolutionIndex].ToString();
                        break;

                    //Full Screen
                    case 1:
                        text = Screen.fullScreen ? "On" : "Off";
                        break;

                    //Vsync
                    case 2:
                        text = (QualitySettings.vSyncCount == 1) ? "On" : "Off";
                        break;

                    //Joystick Sensitivity
                    case 3:
                        text = Settings.sharedInstance.levitationJoystickSensitivity.ToString();
                        break;
                }
                menuText2[i].text = text;
            }
            else
            {
                //pos.x = 0;
                //pos2.x = 0;
                menuText2[i].text = "";
                menuTextColors[i] = nonSelectedColor;
            }
            //Align the text
            //menuText[i].rectTransform.anchoredPosition = pos;
            menuText[i].color = menuTextColors[i];
            //menuText[i].alignment = alignLeft[index] ? TextAnchor.MiddleLeft : TextAnchor.MiddleCenter;
            //menuText2[i].rectTransform.anchoredPosition = pos2;
            menuText2[i].color = menuTextColors[i];
            //menuText2[i].alignment = alignLeft[index] ? TextAnchor.MiddleRight : TextAnchor.MiddleCenter;
            //Might not need alignment and anchored position variables anymore
        }
    }

    #region Transition coroutines
    IEnumerator FadeOutText(Text text, IEnumerator next)
    {
        state = MainMenuState.Transitioning;
        float baseScale = text.gameObject.transform.localScale.x;
        float dScale = 2f;
        float speed = 3f;
        float newScale;
        float alpha;
        for (float i = 0f; i < 1f; i += Time.deltaTime * speed)
        {
            newScale = (1f + i * dScale) * baseScale;
            alpha = Mathf.Lerp(1f, 0f, i);
            text.gameObject.transform.localScale = new Vector3(newScale, newScale, 1f);
            text.color = new Color(1f, 1f, 1f, alpha);
            yield return 0;
        }
        text.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        yield return next;
    }

    IEnumerator TransitionText(int index, AnimationCurve curve, float transitionSpeed, bool inverse)
    {
        for (float i = 0f; i < 1f; i += Time.deltaTime * transitionSpeed)
        {
            float j = inverse ? (1f - i) : i;
            //float scale = moveInCurve.Evaluate(i);
            float yscale = curve.Evaluate(j);
            float xscale = 2 - yscale;
            Vector3 scale = new Vector3(xscale, yscale, 1f);
            menuText[index].gameObject.transform.localScale = scale * baseTextScale;
            menuText2[index].gameObject.transform.localScale = scale * baseTextScale;
            yield return 0;
        }
        menuText[index].gameObject.transform.localScale = inverse ? Vector3.zero : Vector3.one * baseTextScale;
        menuText2[index].gameObject.transform.localScale = inverse ? Vector3.zero : Vector3.one * baseTextScale;
    }

    IEnumerator Transition(MainMenuState newState, AnimationCurve curve, float transitionSpeed, float delayBetweenLinesTransitioning, bool skipTransitionFromScreen)
    {
        cursor.SetActive(false);
        //Fade out current screen to go to the new screen
        //Skip transition from the current screen
        MainMenuState oldState = state;
        state = MainMenuState.Transitioning;

        if (skipTransitionFromScreen)
        {
            for(int i = 0; i < menuText.Length; i++)
            {
                menuText[i].gameObject.transform.localScale = Vector3.zero;
                menuText2[i].gameObject.transform.localScale = Vector3.zero;
            }
        }
        else
        {
            //Transition from old screen
            for (int i = 0; i < listCount; i++)
            {
                if (i < listCount - 1)
                {
                    StartCoroutine(TransitionText(i, curve, transitionSpeed, true));
                    yield return new WaitForSeconds(delayBetweenLinesTransitioning);
                }
                else
                {
                    yield return TransitionText(i, curve, transitionSpeed, true);
                }
            }
        }

        //Set values for new menu
        if ((int)oldState >= 0)
        {
            menuText[selectIndices[(int)oldState]].color = nonSelectedColor;
            menuText2[selectIndices[(int)oldState]].color = nonSelectedColor;
        }
        listCount = menuStrings[(int)newState].Length;
        SetText((int)newState, listCount);

        //Activate or deactivate text
        for (int i = 0; i < menuText.Length; i++)
        {
            menuText[i].gameObject.SetActive(i < listCount);
            menuText2[i].gameObject.SetActive(i < listCount);
        }

        //Transition to new screen
        for (int i = 0; i < listCount; i++)
        {
            if (i < listCount - 1)
            {
                StartCoroutine(TransitionText(i, curve, transitionSpeed, false));
                yield return new WaitForSeconds(delayBetweenLinesTransitioning);
            }
            else
            {
                yield return TransitionText(i, curve, transitionSpeed, false);
            }
        }
        state = newState;
        cursor.SetActive(true);
        MoveCursor();
        RotateCursor();
    }
    #endregion

    #region Input checkers
    void CheckAxis()
    {
        //For some reason a bug happens in this script if GetAxis is used, but not any other script
        if (Input.GetAxisRaw("Vertical") >= minAxis && axisDirection != 1)
        {
            axisDirection = 1;
            axisDirectionPressed = 1;
        }
        else if (Input.GetAxisRaw("Vertical") <= -minAxis && axisDirection != -1)
        {
            axisDirection = -1;
            axisDirectionPressed = -1;
        }
        else if (Input.GetAxisRaw("Vertical") == 0 && axisDirection != 0)
        {
            axisDirection = 0;
            axisDirectionPressed = 0;
        }
    }

    void CheckForDirectionsPressed()
    {
        //Left
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            buttonsPressed |= MenuButtons.Left;
        }
        //Up
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || axisDirectionPressed == 1)
        {
            buttonsPressed |= MenuButtons.Up;
        }
        //Right
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            buttonsPressed |= MenuButtons.Right;
        }
        //Down
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || axisDirectionPressed == -1)
        {
            buttonsPressed |= MenuButtons.Down;
        }
    }

    void CheckForConfirmAndBackButtons()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("PS4Jump"))
        {
            //Confirm pressed
            buttonsPressed |= MenuButtons.Confirm;
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("PS4CIRCLE"))
        {
            //Back pressed
            buttonsPressed |= MenuButtons.Back;
        }
    }

    void ResetButtonCheck()
    {
        axisDirectionPressed = 0;
        buttonsPressed = MenuButtons.None;
    }

    //For moving up and down menus
    void HandleMenuNavigation()
    {
        //Check for input
        if ((buttonsPressed & MenuButtons.Up) == MenuButtons.Up)
        {
            CurrentSelectIndex--;
        }

        if ((buttonsPressed & MenuButtons.Down) == MenuButtons.Down)
        {
            CurrentSelectIndex++;
        }

        //Wrap around
        if (CurrentSelectIndex >= listCount)
            CurrentSelectIndex -= listCount;
        else if (CurrentSelectIndex < 0)
            CurrentSelectIndex += listCount;

        //Highlight currently selected text
        for (int i = 0; i < listCount; i++)
        {
            if (i == CurrentSelectIndex)
            {
                menuText[i].color = selectedColor;
                menuText2[i].color = selectedColor;
            }
            else
            {
                menuText[i].color = menuTextColors[i];
                menuText2[i].color = menuTextColors[i];
            }
        }
    }
    #endregion

}
