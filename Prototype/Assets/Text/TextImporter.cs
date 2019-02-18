using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextImporter : MonoBehaviour
{
    public GameObject TextBox;
    public Text Text;
    public TextAsset TextFile;
    private string[] Textlines;
    public int currentline;
    public int endAtline;
    private bool isTyping = false;
    private bool cancelTyping = false;
    public float typeSpeed;
    
    public bool isActive;

    // Start is called before the first frame update
    void Start()
    {
        if(TextFile != null)
        {
            Textlines = (TextFile.text.Split('\n'));

        }

        if(endAtline == 0)
        {
            endAtline = Textlines.Length - 1;
        }

        if (isActive)
            EnableTextBox();
        else
            DisableTextBox();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive)
        {
            return;
        }
        //Text.text = Textlines[currentline];
        if(Input.GetKeyDown(KeyCode.Return))
        {

            if(!isTyping)
            {

                currentline += 1;
                if (currentline > endAtline)
                {
                    DisableTextBox();
                }
                else
                {
                    Debug.Log("here");
                    StartCoroutine(TextScroll(Textlines[currentline]));
                }
               
            }
            else if (isTyping && !cancelTyping)
            {
                cancelTyping = true;
            }
        }


    }

    private IEnumerator TextScroll(string lineofText)
    {
        int letter = 0;
        Text.text = "";
        isTyping = true;
        cancelTyping = false;
        while(isTyping && !cancelTyping &&(letter < lineofText.Length - 1))
        {
            Text.text += lineofText[letter];
            letter += 1;
            yield return new WaitForSeconds(typeSpeed);
        }
        Text.text = lineofText;
        isTyping = false;
        cancelTyping = false;
    }

    public void EnableTextBox()
    {
        TextBox.SetActive(true);
        StartCoroutine(TextScroll(Textlines[currentline]));

    }

    public void DisableTextBox()
    {
        TextBox.SetActive(false);
    }
} 
