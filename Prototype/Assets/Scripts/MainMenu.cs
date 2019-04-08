using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Start at -3 so that Main = 0
enum MainMenuState
{
    None = -3,
    PressAnyButton,
    Transitioning,

    Main,
    FileSelect,
    Options
}

[System.Flags]
enum MainMenuButtons
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
    public AnimationCurve[] curves;
    MainMenuState state = MainMenuState.None;
    MainMenuButtons buttonsPressed = MainMenuButtons.None;
    string[][] menuStrings;
    int listCount;
    int[] selectIndices;
    float[] transitionSpeeds = { 4.0f, 1.3f };
    float[] delayBetweenLinesTransitioning = { 0.05f, 0.1f };
    float baseTextScale;
    float count;

    int axisDirectionPressed;
    int axisDirection;
    float minAxis = 0.5f;

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
        baseTextScale = menuText[0].gameObject.transform.localScale.x;
        Initialize();
        ChangeState(MainMenuState.PressAnyButton);
    }

    //Initialize arrays, and make all menu option text invisible
    void Initialize()
    {
        count = 0f;
        //Initialize menu strings
        menuStrings = new string[][] {
            //Main
            new string[] { "New Game", "Continue", "Options", "Quit" },

            //FileSelect
            new string[] { "File 1", "File 2", "File 3" },

            //Options
            new string[] {"Resolution:", "Joystick sensitivity:", "Back"}
        };
        selectIndices = new int[] { 0, 0, 0 };
        foreach (Text t in menuText)
        {
            t.gameObject.transform.localScale = Vector3.zero;
        }
    }

    void Update()
    {
        CheckAxis();
        CheckForDirectionsPressed();
        CheckForConfirmAndBackButtons();

        switch (state)
        {
            case MainMenuState.PressAnyButton:
                OnPressAnyButton();
                break;

            case MainMenuState.Main:
                OnMainMenu();
                break;

            case MainMenuState.FileSelect:
                break;

            case MainMenuState.Options:
                break;
        }

        ResetButtonCheck();

        //string debug = "";
        //foreach (int index in selectIndices)
        //{
        //    debug += index.ToString();
        //}
        //Debug.Log(debug);
    }

    void ChangeState(MainMenuState newState)
    {
        if (newState == state) return;

        if (newState == MainMenuState.PressAnyButton)
        {
            pressAnyButtonText.gameObject.SetActive(true);
            state = newState;
        }
        else if (state == MainMenuState.PressAnyButton && newState == MainMenuState.Main)
        {
            StartCoroutine(FadeOutText(pressAnyButtonText, Transition(newState, curves[1], transitionSpeeds[1], delayBetweenLinesTransitioning[1], true)));
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

    //Called every frame if on the main menu
    void OnMainMenu()
    {
        HandleMenuNavigation();

        if ((buttonsPressed & MainMenuButtons.Confirm) == MainMenuButtons.Confirm)
        {
            //Highlighted over "New Game"
            if (CurrentSelectIndex == 0)
                SceneManager.LoadScene(newGameLevel);

            //Options
            else if (CurrentSelectIndex == 2)
                ChangeState(MainMenuState.Options);

            //Quit
            else if (CurrentSelectIndex == 3)
                Application.Quit();
        }
    }
    #endregion

    void SetText(int index, int listSize)
    {
        for (int i = 0; i < listSize; i++)
        {
            menuText[i].text = menuStrings[index][i];
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
            yield return 0;
        }
        menuText[index].gameObject.transform.localScale = inverse ? Vector3.zero : Vector3.one * baseTextScale;
    }

    IEnumerator Transition(MainMenuState newState, AnimationCurve curve, float transitionSpeed, float delayBetweenLinesTransitioning, bool skipTransitionFromScreen)
    {
        //Fade out current screen to go to the new screen
        //Skip transition from the current screen
        MainMenuState oldState = state;
        state = MainMenuState.Transitioning;

        if (skipTransitionFromScreen)
        {
            foreach (Text t in menuText)
            {
                t.gameObject.transform.localScale = Vector3.zero;
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
        listCount = menuStrings[(int)newState].Length;
        SetText((int)newState, listCount);

        //Activate or deactivate text
        if ((int)oldState >= 0)
            menuText[selectIndices[(int)oldState]].color = Color.white;
        for (int i = 0; i < menuText.Length; i++)
        {
            menuText[i].gameObject.SetActive(i < listCount);
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
            buttonsPressed |= MainMenuButtons.Left;
        }
        //Up
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || axisDirectionPressed == 1)
        {
            buttonsPressed |= MainMenuButtons.Up;
        }
        //Right
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            buttonsPressed |= MainMenuButtons.Right;
        }
        //Down
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || axisDirectionPressed == -1)
        {
            buttonsPressed |= MainMenuButtons.Down;
        }
    }

    void CheckForConfirmAndBackButtons()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("PS4Jump"))
        {
            //Confirm pressed
            buttonsPressed |= MainMenuButtons.Confirm;
        }
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("PS4CIRCLE"))
        {
            //Back pressed
            buttonsPressed |= MainMenuButtons.Back;
        }
    }

    void ResetButtonCheck()
    {
        axisDirectionPressed = 0;
        buttonsPressed = MainMenuButtons.None;
    }

    //For moving up and down menus
    void HandleMenuNavigation()
    {
        //Check for input
        if ((buttonsPressed & MainMenuButtons.Left) == MainMenuButtons.Left ||
            (buttonsPressed & MainMenuButtons.Up) == MainMenuButtons.Up)
        {
            CurrentSelectIndex--;
        }

        if ((buttonsPressed & MainMenuButtons.Right) == MainMenuButtons.Right ||
                    (buttonsPressed & MainMenuButtons.Down) == MainMenuButtons.Down)
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
                menuText[i].color = Color.yellow;
            else
                menuText[i].color = Color.white;
        }
    }
    #endregion

}
