using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyDataItem : MonoBehaviour
{
    public Action ColorClickAction;
    public EnemyData EnemyData { get; } = new();
    [SerializeField] Image image;
    [SerializeField] TMP_InputField inputField;

    private void OnEnable()
    {
        OnNameChanged(EnemyData.Name);
        SetColor(EnemyData.Color);
    }
    public void OnNameChanged(string value)
    {
        inputField.SetTextWithoutNotify(value);
        EnemyData.Name = value;
    }
    public void SetColor(Color color)
    {
        image.color = color;
        EnemyData.Color = color;
    }

    public void OnColorClick()
    {
        ColorClickAction?.Invoke();
        Debug.Log("click!");
    }
}
