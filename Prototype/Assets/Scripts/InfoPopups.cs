using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class InfoPopups : MonoBehaviour
{
    public GameObject[] popups;
    Grayscale grayscale;
    int imageIndex;
    
    void Start()
    {
        grayscale = FindObjectOfType<Grayscale>();
        imageIndex = -1;
    }

    IEnumerator ShowImages_(params int[] indexes)
    {
        PauseMenu.sharedInstance.PauseMenuDisabled = true;
        Time.timeScale = 0f;
        grayscale.enabled = true;
        //showingImage = true;
        foreach (int index in indexes)
        {
            imageIndex = index;
            popups[index].SetActive(true);
            yield return 0;
            while (!(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetButtonDown("PS4Jump")))
            {
                yield return 0;
            }
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
}
