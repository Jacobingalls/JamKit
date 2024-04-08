using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonHighlightExpand : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool isText;
    public float scaleFactor = 1.0f;
    public float duration = 1.0f;

    private TextMeshProUGUI _text;
    private float _originalFontSize;

    public float current = 0;
    public float desired = 0;

    public void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _originalFontSize = _text.fontSize;
    }

    public void Update()
    {
        if (current != desired) {
            if (current > desired) {
                current -= Time.deltaTime;
                current = Mathf.Max(current, desired);
            } else {
                current += Time.deltaTime;
                current = Mathf.Min(current, desired);
            }

            _text.fontSize = Mathf.Lerp(_originalFontSize, _originalFontSize * scaleFactor, current / duration);
        }
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        desired = duration;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        desired = 0;
    }
}
