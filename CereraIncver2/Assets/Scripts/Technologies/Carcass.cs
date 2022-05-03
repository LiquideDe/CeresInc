using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carcass : Technology
{
   
    public float Weight { get; set; }
    public float MaxWeightPlayload { get; set; }

    public Carcass(string techName, bool isResearched, int costResearch, string description, float weight, float maxPlayLoad, float cost, Sciense sciense)
    {
        NameTech = techName;
        IsResearched = isResearched;
        CostResearch = costResearch;
        Description = description;

        Weight = weight;
        MaxWeightPlayload = maxPlayLoad;
        Cost = cost;

        Sciense = sciense;

        IsOutdated = false;
    }

    public Carcass(string techName, int costResearch, int firstUpdate, int secondUpdate, Sciense sciense)
    {
        NameTech = techName;
        IsResearched = false;
        CostResearch = costResearch;
        FirstAmountUpdate = firstUpdate;
        SecondAmountUpdate = secondUpdate;
        Sciense = sciense;
        Description = $"»спользу€ новые технологии в миниатюризации, и новые сплавы удастьс€ снизить вес остова на {firstUpdate}%, а полезную нагрузку увеличить на {secondUpdate}";   

        IsOutdated = false;
    }

    public Carcass(Sciense sciense)
    {
        Sciense = sciense;
    }

    protected override void Research()
    {
        IsResearched = true;
        Sciense.TechnologyResearched(0, FirstAmountUpdate, SecondAmountUpdate, Id);
    }

    protected override void SaveAnotherData(SaveLoadTechnology save)
    {
        save.weight = Weight;
        save.maxWeightPlayload = MaxWeightPlayload;
    }

    protected override void LoadAnotherData(SaveLoadTechnology save)
    {
        Weight = save.weight;
        MaxWeightPlayload = save.maxWeightPlayload;
    }
}
