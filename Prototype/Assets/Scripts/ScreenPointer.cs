using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenPointer : MonoBehaviour
{
    public static ScreenPointer sharedInstance;
    Transform target;
    Vector2 targetPos;
    public Image img;
    float edgeOffset = 75f;
    float yOffset = 100f;
    float rotation;
    // Start is called before the first frame update
    void Start()
    {
        if (sharedInstance != null)
        {
            Destroy(sharedInstance.gameObject);
        }
        sharedInstance = this;
        img.gameObject.SetActive(false);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        img.gameObject.SetActive(true);
    }

    public void RemoveTarget()
    {
        target = null;
        img.gameObject.SetActive(false);
    }

    bool isOutside(Vector2 vector)
    {
        return (vector.x > Screen.width || vector.x < 0f || vector.y > Screen.height || vector.y < 0f);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            targetPos = target.transform.position;

            targetPos = Camera.main.WorldToScreenPoint(targetPos);
            if (isOutside(targetPos))
            {
                targetPos.x -= Screen.width / 2;
                targetPos.y -= Screen.height / 2;

                if (targetPos.x > Screen.width / 2f || targetPos.x < -Screen.width / 2f)
                {
                    float ratio = Mathf.Abs((Screen.width - edgeOffset) * 0.5f / targetPos.x);
                    targetPos.x *= ratio;
                    targetPos.y *= ratio;
                }
                if (targetPos.y > Screen.height / 2f || targetPos.y < -Screen.height / 2f)
                {
                    float ratio = Mathf.Abs((Screen.height - edgeOffset) * 0.5f / targetPos.y);
                    targetPos.x *= ratio;
                    targetPos.y *= ratio;
                }
                rotation = Mathf.Atan2(targetPos.y, targetPos.x) * 180f / Mathf.PI;
                targetPos.x += Screen.width / 2;
                targetPos.y += Screen.height / 2;
                img.transform.rotation = Quaternion.Euler(0f, 0f, rotation);
            }
            else
            {
                img.transform.rotation = Quaternion.Euler(0f, 0f, 270f);
                targetPos.y += yOffset;
            }
            img.transform.position = targetPos;
        }
    }
}
