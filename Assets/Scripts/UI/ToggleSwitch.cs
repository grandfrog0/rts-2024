using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ToggleSwitch : MonoBehaviour
{
    [SerializeField] Text _text;
    [SerializeField] Sprite _spriteOn, _spriteOff;
    [SerializeField] RectTransform _checkmark;
    private Image _checkmarkImage;
    private Toggle _toggle;

    private bool _isOn;
    public bool IsOn
    {
        get => _isOn;
        set
        {
            _isOn = value;
            _text.text = _isOn ? "ON" : "OFF";
            _checkmarkImage.sprite = _isOn ? _spriteOn : _spriteOff;
            StartCoroutine(AnimationRoutine(_isOn));
        }
    }

    private IEnumerator AnimationRoutine(bool value)
    {
        Vector3 targetPosition = new Vector3(value ? 22 : -22, _checkmark.anchoredPosition.y);

        for (float t = 0f; t <= 1; t += Time.deltaTime * 5)
        {
            _checkmark.anchoredPosition = Vector3.Lerp(_checkmark.anchoredPosition, targetPosition, t);
            yield return null;
        }

        _checkmark.anchoredPosition = targetPosition;
    }

    private void Start()
    {
        _checkmarkImage = _checkmark.GetComponent<Image>();

        _toggle = GetComponent<Toggle>();
        IsOn = _toggle.isOn;
        _toggle.onValueChanged.AddListener(x => IsOn = x);
    }
}
