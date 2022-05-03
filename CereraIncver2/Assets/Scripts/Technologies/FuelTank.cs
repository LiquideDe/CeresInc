using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelTank : Technology
{
    public float Weight { get; set; }
    public float MaxFuel { get; set; }

    public FuelTank(string techName, bool isResearched, int costResearch, string description, float weight, float maxFuel, float cost, Sciense sciense)
    {
        NameTech = techName;
        IsResearched = isResearched;
        CostResearch = costResearch;
        Description = description;

        Weight = weight;
        MaxFuel = maxFuel;
        Cost = cost;
        Sciense = sciense;
        IsOutdated = false;
    }

    public FuelTank(string techName, int costResearch, int firstUpdate, int secondUpdate, Sciense sciense)
    {
        NameTech = techName;
        IsResearched = false;
        CostResearch = costResearch;
        FirstAmountUpdate = firstUpdate;
        SecondAmountUpdate = secondUpdate;
        Sciense = sciense;
        Description = $"»спользу€ новые технологии в миниатюризации, новые сплавы и разработка более плотной компоновки удастьс€ снизить вес баков на {firstUpdate}%, а объем топлива увеличить на {secondUpdate}";

        IsOutdated = false;
        
    }

    public FuelTank(Sciense sciense) 
    {
        Sciense = sciense;
    }

    protected override void Research()
    {
        IsResearched = true;
        Sciense.TechnologyResearched(1, FirstAmountUpdate, SecondAmountUpdate, Id);
    }

    protected override void SaveAnotherData(SaveLoadTechnology save)
    {
        save.weight = Weight;
        save.maxFuel = MaxFuel;
    }

    protected override void LoadAnotherData(SaveLoadTechnology save)
    {
        Weight = save.weight;
        MaxFuel = save.maxFuel;
    }
}
