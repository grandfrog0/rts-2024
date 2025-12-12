using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class SerializableUnit
{
    public string Name;
    public float MovementSpeed;
    public float MaxHealth;
    public List<TrainingCost> TrainingCost;
    public float DetectionRange;
}