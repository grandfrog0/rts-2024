
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
            if (Text) Text.text = _count.ToString();
        }
    }
    public bool IsResource;
    public TMP_Text Text;
}