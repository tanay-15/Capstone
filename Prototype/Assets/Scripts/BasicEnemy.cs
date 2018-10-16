using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class EnemyEvents
{
    //[System.NonSerialized]
    public UnityEvent OnDeath;

    //[System.NonSerialized]
    public UnityEvent OnTakeDamage;
}

public class BasicEnemy : MonoBehaviour {
    public EnemyEvents events;
}
