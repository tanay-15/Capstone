using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class InfoPopups : MonoBehaviour
{
    public GameObject[] popups;
    public GameObject okButton;
    public int tutorialsViewed;
    Grayscale grayscale;
    int imageIndex;
    
    void Start()
    {
        grayscale = FindObjectOfType<Grayscale>();
        okButton.SetActive(false);
        imageIndex = -1;
        Manual.tutorialsViewed = tutorialsViewed;
    }

    IEnumerator ShowImages_(params int[] indexes)
    {
        PauseMenu.sharedInstance.PauseMenuDisabled = true;
        Time.timeScale = 0f;
        grayscale.enabled = true;
        //showingImage = true;
        foreach (int index in indexes)
        {
            //Manual.tutorialsViewed = tutorialsViewed + 1 + index;
            imageIndex = index;
            popups[index].SetActive(true);
            for (int i = 0; i < 30; i++) { yield return 0; }
            okButton.SetActive(true);
            while (!(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("PS4Jump")))
            //while (!(Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("PS4TRIANGLE")))
            {
                yield return 0;
            }
            okButton.SetActive(false);
            popups[imageIndex].SetActive(false);
        }
        Time.timeScale = 1f;
        grayscale.enabled = false;
        //showingImage = false;
        PauseMenu.sharedInstance.PauseMenuDisabled = false;
    }

    public void ShowImages(params int[] indexes)
    {
        StartCoroutine(ShowImages_(indexes));
    }

    public void ShowImage(int index)
    {
        StartCoroutine(ShowImages_(index));
    }
    public void SetTutorialViewed(int index)
    {
        Manual.tutorialsViewed = Mathf.Max(Manual.tutorialsViewed, index);
    }
}
