using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_InputField))]
public class RangedInputField : MonoBehaviour
{
    [SerializeField] int minValue, maxValue;
    private TMP_InputField _inputfield;
    private void Start()
    {
        _inputfield = GetComponent<TMP_InputField>();
        _inputfield.onEndEdit.AddListener(OnValueChanged);
    }
    public void OnValueChanged(string value)
    {
        if (value.Length > 6)
            value = value.Substring(0, 6);

        if (int.TryParse(value, out int v))
            _inputfield.text = Mathf.Clamp(v, minValue, maxValue).ToString();
        Debug.Log(v);
    }
}
