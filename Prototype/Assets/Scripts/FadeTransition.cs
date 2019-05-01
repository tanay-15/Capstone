using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    //TODO: Replace loading text with more professional-looking loading screen
    public GameObject loading;
    public Image black;
    public string newScene;
    public float fadeSpeed = 1.2f;
    Color invisible = new Color(0f, 0f, 0f, 0f);
    IEnumerator routine;
    void Start()
    {
        black.color = invisible;
        routine = null;
        if (loading != null)
            loading.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && routine == null)
        {
            if (routine == null)
            {
                col.gameObject.SendMessage("SetFixedRun");
                routine = FadeOut();
                StartCoroutine(routine);
            }
        }
    }

    //Load the scene in the background, then load them after the scene fades out
    IEnumerator FadeOut()
    {
        if (loading != null)
            loading.SetActive(true);
        AsyncOperation op = SceneManager.LoadSceneAsync(newScene);
        op.allowSceneActivation = false;
        for (float i = 0; i < 1f; i += fadeSpeed * Time.deltaTime)
        {
            float alpha = i;
            Color newColor = Color.black;
            newColor.a = alpha;
            black.color = newColor;
            if (op.progress == 0.5f && loading != null)
                loading.SetActive(false);
            yield return 0;
        }
        black.color = Color.black;
        op.allowSceneActivation = true;
    }

    public void StartSequence()
    {
        if (routine == null)
        {
            routine = FadeOut();
            StartCoroutine(routine);
        }
    }

    void Update()
    {
        
    }
}
