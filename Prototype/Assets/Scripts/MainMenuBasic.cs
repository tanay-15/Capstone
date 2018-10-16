﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuBasic : MonoBehaviour {

    public string[] menuStrings;
    public string newGameLevel;

    public Text[] menuText;
    int selectIndex;
    int prevIndex;

    Color transparentColor;

	void Start () {
        transparentColor = new Color(1f, 1f, 1f, 0.5f);
        selectIndex = 0;
        prevIndex = selectIndex;
        InitializeText();
	}
	
	void Update () {
        CheckForArrowKeys();
        CheckForConfirmButton();
	}

    void UpdateMenu(int newIndex, int oldIndex)
    {
        menuText[oldIndex].color = transparentColor;
        menuText[newIndex].color = Color.white;
    }

    void InitializeText()
    {
        for (int i = 0; i < menuStrings.Length; i++)
        {
            menuText[i].text = menuStrings[i];
            if (i != selectIndex)
                menuText[i].color = transparentColor;
        }
    }

    void CheckForArrowKeys()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectIndex--;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectIndex++;
        }
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

    void CheckForConfirmButton()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            switch (selectIndex)
            {
                //New Game
                case 0:
                    SceneManager.LoadScene(newGameLevel);
                    break;

                //Continue
                case 1:
                    break;

                //Options
                case 2:
                    break;

                //Quit
                case 3:
                    Application.Quit();
                    break;
            }
        }
    }
}