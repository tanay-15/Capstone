using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    float count;
    public GameObject yinYang;
    public Text loadingText;

    void Start()
    {
        count = 0f;
    }

    void Update()
    {
        if (gameObject.activeSelf)
        {
            count += Time.deltaTime;
            yinYang.transform.rotation = Quaternion.Euler(0f, 0f, count * -50);

            if (count % 2 < 0.5f)
            {
                loadingText.text = "Now Loading";
            }
            else if (count % 2 < 1f)
            {
                loadingText.text = "Now Loading.";
            }
            else if (count % 2 < 1.5f)
            {
                loadingText.text = "Now Loading..";
            }
            else if (count % 2 < 2f)
            {
                loadingText.text = "Now Loading...";
            }
        }
    }
}
