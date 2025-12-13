using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance { get; private set; }

    [SerializeField] List<InvItem> invItems = new List<InvItem>();
    private Dictionary<string, InvItem> _items;
    
    public void Init(string name, int count)
    {
        instance = this;

        _items = new();
        foreach(InvItem item in invItems)
            _items.Add(item.Name, item);

        SetCount(name, count);
    }
    public int GetCount(string name)
    {
        return _items[name].Count;
    }
    public void AddCount(string name, int value)
    {
        _items[name].Count += value;
    }
    public void SetCount(string name, int value)
    {
        _items[name].Count = value;
    }
}
