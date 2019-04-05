using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingBackground : DemonSwitch
{
    public SpriteRenderer normalBackground;
    public SpriteRenderer demonBackground;
    float deltaTime = 0.5f;
    public AnimationCurve curve;

    Color normalStart;
    Color demonStart;
    Color transparent;

    Color normal;
    Color demon;

    void Start()
    {
        normalStart = normalBackground.color;
        demonStart = demonBackground.color;
        normal = normalStart;
        demon = demonStart;

        demon.a = 0f;
        //transparent = new Color(0f, 0f, 0f, 0f);
        demonBackground.color = demon;
    }

    public override void Switch(bool toDemon)
    {
        StartCoroutine(SwitchBackground(toDemon));
    }

    IEnumerator SwitchBackground(bool toDemon)
    {
        float normalT = 0f;
        float demonT = 0f;
        for (float i = 0f; i < 1f; i+=Time.deltaTime * deltaTime){
            normalT = (i <= 0.5f) ? curve.Evaluate(i) : 0f;
            demonT = (i >= 0.5f) ? curve.Evaluate(i) : 0f;
            
            //normal = Color.Lerp(transparent, normalStart, toDemon ? normalT : 1f - normalT);
            //demon = Color.Lerp(transparent, demonStart, toDemon ? demonT : 1f - demonT);
            normal.a = Mathf.Lerp(0f, 1f, toDemon ? normalT : demonT);
            demon.a = Mathf.Lerp(0f, 1f, toDemon ? demonT : normalT);

            normalBackground.color = normal;
            demonBackground.color = demon;
            yield return 0;
        }
        normal.a = (toDemon) ? 0f : 1f;
        demon.a = (toDemon) ? 1f : 0f;
        normalBackground.color = normal;
        demonBackground.color = demon;
    }

    //For testing purposes
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Y))
    //    {
    //        Switch(true);
    //    }
    //    if (Input.GetKeyDown(KeyCode.U))
    //    {
    //        Switch(false);
    //    }
    //}
}
