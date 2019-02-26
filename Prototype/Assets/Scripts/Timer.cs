using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer sharedInstance;
    public Text text;
    public float time;

    int seconds;
    int cseconds;

    string sseconds;
    string scseconds;

    public bool Running
    {
        get
        {
            return time > 0f;
        }
    }

    static Timer()
    {

    }

    void Start()
    {
        if (sharedInstance != null)
        {
            Destroy(sharedInstance.gameObject);
        }
        sharedInstance = this;
        text.gameObject.SetActive(false);
    }

    public void Set(float seconds)
    {
        time = seconds;
        text.gameObject.SetActive(true);
    }

    public void Stop()
    {
        time = 0f;
        text.gameObject.SetActive(false);
    }

    void Update()
    {
        time -= (time > 0) ? Time.deltaTime : 0;
        if (time > 0)
        {
            seconds = (int)time;
            cseconds = (int)(time * 100) % 100;
            sseconds = ((seconds < 10) ? "0" : "") + seconds.ToString();
            scseconds = ((cseconds < 10) ? "0" : "") + cseconds.ToString();

            text.text = string.Format("{0}:{1}", sseconds, scseconds);
        }
        else
        {
            text.gameObject.SetActive(false);
        }
    }
}
