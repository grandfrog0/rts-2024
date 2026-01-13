using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Inventory : MonoBehaviour
{
    public static UnitInventory Player { get; } = new();

    [SerializeField] List<InvItem> invItems = new List<InvItem>();
    
    public void Init()
    {
        Player.Init(invItems);
    }
}
