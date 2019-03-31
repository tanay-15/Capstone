﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DestroyEvent : MonoBehaviour
{
    public UnityEvent onDestroy;

    void OnDestroy()
    {
        onDestroy.Invoke();
    }
}