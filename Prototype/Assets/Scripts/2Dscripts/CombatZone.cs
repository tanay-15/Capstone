﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatZone : MonoBehaviour
{

    public GameObject Door1;
    public GameObject Door2;
    public bool Door1Open = true;
    public bool Door2Open = false;
    public float offset = 3f;
    Vector2 Door1InitPosition;
    Vector2 Door2InitPosition;

    public GameObject[] Enemies;
    int DeadEnemies = 0;
    // Start is called before the first frame update
    void Start()
    {
        Door1InitPosition = Door1.transform.position;
        Door2InitPosition = Door2.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Door1Open)
            Door1.transform.position = Vector2.MoveTowards(Door1.transform.position, Door1InitPosition + new Vector2(0,offset), 0.2f);
        else
            Door1.transform.position = Vector2.MoveTowards(Door1.transform.position, Door1InitPosition , 0.1f);

        if (Door2Open)
            Door2.transform.position = Vector2.MoveTowards(Door2.transform.position, Door2InitPosition + new Vector2(0, offset), 0.2f);
        else
            Door2.transform.position = Vector2.MoveTowards(Door2.transform.position, Door2InitPosition, 0.1f);


        

        if (DeadEnemies != Enemies.Length)
        {
            DeadEnemies = 0;
            for (int i = 0; i < Enemies.Length; i++)
            {
                if (Enemies[i] == null)
                {
                    DeadEnemies++;
                }
            }
        }
        else
        {
            Door2Open = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            Door1Open = false;
            Camera.main.GetComponent<CameraFollow>().target = transform;
            Camera.main.GetComponent<CameraFollow>().CameraPan(7,1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player" && DeadEnemies == Enemies.Length)
        {
            Camera.main.GetComponent<CameraFollow>().target = collision.gameObject.transform;
            Camera.main.GetComponent<CameraFollow>().CameraPan(6, 1);
        }
    }
}
