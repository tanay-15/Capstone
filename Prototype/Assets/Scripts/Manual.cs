using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manual : MonoBehaviour
{
    public Text[] textObjects;
    public GameObject[] popups;
    public Image black;
    public GameObject cursor;
    Color nonSelectedColor = Color.yellow;
    Color selectedColor = Color.red;
    int axisDirectionPressed;
    int axisDirection;
    int listCount;
    int CurrentSelectIndex;
    float minAxis = 0.5f;
    MenuButtons buttonsPressed = MenuButtons.None;

    float cursorRotation;
    float cursorRotationSpeed = -40f;

    static int savedIndex;

    static Manual()
    {
        savedIndex = 0;
    }

    void Start()
    {
        CurrentSelectIndex = savedIndex;
        StartCoroutine(Transition(true));
        InitializeText();
        InitializePopups();
        cursorRotation = 0f;
        listCount = textObjects.Length;
    }

    IEnumerator Transition(bool fadeIn)
    {
        Color col = Color.black;
        for (float i = 0; i < 1f; i += 0.05f)
        {
            col.a = (fadeIn) ? (1 - i) : i;
            black.color = col;
            yield return 0;
        }
        if (!fadeIn)
            ReturnToPauseMenu();
        else
        {
            col.a = 0f;
            black.color = col;
        }
    }

    void ReturnToPauseMenu()
    {
        PauseMenu.sharedInstance.ReturnToPauseMenu("Manual");
    }

    void InitializeText()
    {
        float startPos = 20 * (textObjects.Length - 1) / 2;
        for (int i = 0; i < textObjects.Length; i++)
        {
            textObjects[i].rectTransform.anchoredPosition = new Vector2(0f, startPos - 20 * i);
        }
    }

    void InitializePopups()
    {
        for (int i = 0; i < popups.Length; i++)
        {
            popups[i].SetActive(i == CurrentSelectIndex);
        }
    }

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
        int oldSelectIndex = CurrentSelectIndex;
        //Check for up and down input
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

        //Set the current popup active
        popups[oldSelectIndex].SetActive(false);
        popups[CurrentSelectIndex].SetActive(true);

        //Highlight currently selected text
        for (int i = 0; i < listCount; i++)
        {
            if (i == CurrentSelectIndex)
            {
                textObjects[i].color = selectedColor;
                //menuText2[i].color = selectedColor;
            }
            else
            {
                textObjects[i].color = nonSelectedColor;
                //menuText2[i].color = menuTextColors[i];
            }
        }

        //Check for back button pressed
        if ((buttonsPressed & MenuButtons.Back) == MenuButtons.Back)
        {
            StartCoroutine(Transition(false));
        }
    }
    #endregion


    void Update()
    {
        CheckAxis();
        CheckForDirectionsPressed();
        CheckForConfirmAndBackButtons();

        HandleMenuNavigation();

        ResetButtonCheck();
        MoveCursor();
    }

    void MoveCursor()
    {
            cursorRotation -= Time.deltaTime * cursorRotationSpeed;
            if (cursorRotation < 0)
                cursorRotation += 360;
            Vector3 position = textObjects[CurrentSelectIndex].rectTransform.position;
            //position.x -= ((menuText[CurrentSelectIndex].preferredWidth / 2f) * menuText[CurrentSelectIndex].rectTransform.lossyScale.x) + 40f;
            position.x -= 40f;
            cursor.transform.rotation = Quaternion.Euler(0f, 0f, cursorRotation);
            cursor.GetComponent<RectTransform>().position = position;
    }

    void OnDestroy()
    {
        savedIndex = CurrentSelectIndex;
    }
}
