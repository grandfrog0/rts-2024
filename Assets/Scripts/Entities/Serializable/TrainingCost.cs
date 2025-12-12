using System;

[Serializable]
public class TrainingCost
{
    public string Name;
    public int Cost;

    public TrainingCost(string name, int cost)
    {
        Name = name; 
        Cost = cost;
    }
}