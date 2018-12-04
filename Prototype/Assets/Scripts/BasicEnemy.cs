using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityFloatEvent : UnityEvent<float> { }

[System.Serializable]
public class EnemyEvents
{
    //[System.NonSerialized]
    public UnityEvent OnDeath;

    //[System.NonSerialized]
    public UnityFloatEvent OnTakeDamage;
}

public class BasicEnemy : MonoBehaviour {
    public virtual Vector3 lifebarOffset { get { return new Vector3(0f, 1f, 0f); } }
    public EnemyEvents events;
}
