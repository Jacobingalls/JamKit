using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GUIFadeInOut : MonoBehaviour
{
    public CanvasGroup target;

    private float current = 0f;
    private float desired = 0f;
    private float duration = 1f;
    private bool disableGameObjectOnComplete = false;

    public void Update()
    {
        if (current != desired)
        {
            if (current > desired)
            {
                current -= Time.deltaTime;
                current = Mathf.Max(current, desired);
            }
            else
            {
                current += Time.deltaTime;
                current = Mathf.Min(current, desired);
            }

            target.alpha = Mathf.Lerp(0f, 1f, current / duration);

            if (current == desired && disableGameObjectOnComplete)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void EnableAndFadeIn(float duration)
    {
        gameObject.SetActive(true);
        desired = duration;
        this.duration = duration;
        disableGameObjectOnComplete = false;
    }

    public void FadeOutAndDisable(float duration)
    {
        desired = 0;
        this.duration = duration;
        disableGameObjectOnComplete = true;
    }
}