using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Ending : MonoBehaviour
{
    public Text[] messageText;
    public GameObject continueButton;
    bool canContinue;
    string[] text;
    float fadeSpeed = 0.6f;
    Color textColor;
    IEnumerator routine;

    bool MoreHumanSkills()
    {
        int humanSkills = 0, demonSkills = 0;

        if ((SkillTree.info.nodesActivated & SkillNodes.H_1) == SkillNodes.H_1) humanSkills++;
        if ((SkillTree.info.nodesActivated & SkillNodes.H_2) == SkillNodes.H_2) humanSkills++;
        if ((SkillTree.info.nodesActivated & SkillNodes.H_3) == SkillNodes.H_3) humanSkills++;
        if ((SkillTree.info.nodesActivated & SkillNodes.H_4) == SkillNodes.H_4) humanSkills++;

        if ((SkillTree.info.nodesActivated & SkillNodes.D_1) == SkillNodes.D_1) demonSkills++;
        if ((SkillTree.info.nodesActivated & SkillNodes.D_2) == SkillNodes.D_2) demonSkills++;
        if ((SkillTree.info.nodesActivated & SkillNodes.D_3) == SkillNodes.D_3) demonSkills++;
        if ((SkillTree.info.nodesActivated & SkillNodes.D_4) == SkillNodes.D_4) demonSkills++;

        return (humanSkills >= demonSkills);
    }

    void Start()
    {
        canContinue = false;
        bool demonEnding = !MoreHumanSkills();

        //Human ending
        if (!demonEnding)
        {
            text = new string[]{
            "And so, our hero defeated the Lord of the Dead,",
            "and the demon within him was vanquished.",
            "Now our hero can rest easy,",
            "knowing that demons and monsters will no longer plague the land...",
            "Human Ending"
            };
            textColor = new Color(1f, 1f, 1f, 1f);
        }

        //Demon ending
        if (demonEnding)
        {
            //Human ending
            text = new string[]{
            "And so, the warrior proclaimed himself as the new Lord of the Dead.",
            "Not a fragment of humanity remained inside of him.",
            "And with the Land of the Dead now under his control,",
            "he set his sights on the rest of the world, with his vile power...",
            "Demon Ending"
            };
            textColor = new Color(1f, 0.5f, 0.5f, 1f);
        }

        routine = Routine();
        StartCoroutine(routine);
    }

    IEnumerator Routine()
    {
        continueButton.SetActive(false);
        Color newColor = textColor;
        newColor.a = 0f;
        foreach (Text t in messageText)
        {
            t.color = newColor;
        }

        for (int i = 0; i < text.Length; i++)
        {
            messageText[i].text = text[i];
            for (float j = 0f; j < 1f; j += Time.deltaTime * fadeSpeed)
            {
                newColor.a = j;
                messageText[i].color = newColor;
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("PS4Jump"))
                {
                    SetAllText();
                    StopCoroutine(routine);
                }
                yield return 0;
            }
            messageText[i].color = newColor;
            for (float j = 0f; j < 0.5f; j += Time.deltaTime)
            {
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("PS4Jump"))
                {
                    SetAllText();
                    StopCoroutine(routine);
                }
                yield return 0;
            }
        }
        continueButton.SetActive(true);
        canContinue = true;
    }

    void SetAllText()
    {
        for (int i = 0; i < text.Length; i++)
        {
            messageText[i].text = text[i];
            messageText[i].color = textColor;
        }
        continueButton.SetActive(true);
        canContinue = true;
    }

    void Update()
    {

        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("PS4Jump")) && canContinue)
        {
            SceneManager.LoadScene("MainMenuBasic");
        }
    }
}