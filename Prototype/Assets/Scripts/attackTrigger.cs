using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackTrigger : MonoBehaviour {
    public float damage = 10;

	void OnTriggerEnter(Collider col)
    {
        if(col.isTrigger != true && col.CompareTag("Enemy"))
        {
            col.SendMessageUpwards("applyDamage", damage);
        }
    }
}
