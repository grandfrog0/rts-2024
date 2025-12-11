
using System;
using TMPro;
using UnityEngine;

[Serializable]
public class InvItem
{
    public string Name;
    [SerializeField] int _count;
    public int Count
    {
        get => _count;
        set
        {
            _count = value;
            _text.text = _count.ToString();
        }
    }
    public bool IsResource;
    [SerializeField] TMP_Text _text;
}