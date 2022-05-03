using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : Technology
{
    public int Isp { get; set; }
    public float Weight { get; set; }
    public int TypeFuel { get; set; }
    public float Thrust { get; set; }

    public Engine(string techName, bool isResearched, int costResearch, string description, int isp, float weight, int typeFuel, float thrust, float cost, Sciense sciense)
    {
        NameTech = techName;
        IsResearched = isResearched;
        CostResearch = costResearch;
        Description = description;
        Isp = isp;
        Weight = weight;
        TypeFuel = typeFuel;
        Thrust = thrust;
        Cost = cost;
        Sciense = sciense;
        IsOutdated = false;
    }

    public Engine(string techName, int costResearch, int firstUpdate, int secondUpdate, Sciense sciense)
    {
        NameTech = techName;
        IsResearched = false;
        CostResearch = costResearch;
        FirstAmountUpdate = firstUpdate;
        SecondAmountUpdate = secondUpdate;
        Sciense = sciense;
        IsOutdated = false;
        Description = $"»спользу€ новые технологии в миниатюризации, новые сплавы, разработки улучшенных сопел и систем подачи топлива позволит снизить вес двигателей на {firstUpdate}%, а удельный импуль увеличить на {secondUpdate}";

    }

    public Engine(Sciense sciense)
    {
        Sciense = sciense;
    }

    protected override void Research()
    {
        IsResearched = true;
        Sciense.TechnologyResearched(2, FirstAmountUpdate, SecondAmountUpdate, Id);
    }

    protected override void SaveAnotherData(SaveLoadTechnology save)
    {
        save.isp = Isp;
        save.weight = Weight;
        save.typeFuel = TypeFuel;
        save.thrust = Thrust;
    }

    protected override void LoadAnotherData(SaveLoadTechnology save)
    {
        Isp = save.isp;
        Weight = save.weight;
        TypeFuel = save.typeFuel;
        Thrust = save.thrust;
    }
}
