using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;

public class UnitInventory : IEnumerable<InvItem>
{
    private List<InvItem> invItems;
    private Dictionary<string, InvItem> _items = new();
    
    public void Init(List<InvItem> invItems)
    {
        this.invItems = invItems;

        _items.Clear();
        foreach (InvItem item in invItems)
            _items.Add(item.Name, item);
    }
    public int GetCount(string name)
    {
        return _items[name].Count;
    }
    public InvItem GetItem(string name)
    {
        return _items[name];
    }
    public void AddCount(string name, int value)
    {
        _items[name].Count += value;
    }
    public void SetCount(string name, int value)
    {
        _items[name].Count = value;
    }
    public void Clear()
    {
        foreach (InvItem item in invItems)
            item.Count = 0;
    }

    public IEnumerator<InvItem> GetEnumerator() => _items.Select(x => x.Value).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
