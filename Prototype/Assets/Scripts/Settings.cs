using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings
{
    public static Settings sharedInstance;
    public float levitationJoystickSensitivity;
    public const float minJoystick = 5f;
    public const float maxJoystick = 20f;
    static Settings()
    {
        sharedInstance = new Settings();
    }

    public Settings()
    {
        SetDefaultValues();
    }

    void SetDefaultValues()
    {
        levitationJoystickSensitivity = 10f;
    }
}
