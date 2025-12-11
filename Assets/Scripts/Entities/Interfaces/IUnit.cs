using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUnit
{
    public string Name { get; set; }
    public float MovementSpeed { get; set; }
    public float MaxHealth { get; set; }
    public List<(string, int)> TrainingCost { get; set; }
    public float DetectionRange { get; set; }
}