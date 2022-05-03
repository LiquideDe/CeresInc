using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Technology
{
    public string NameTech { get; set; }
    public bool IsResearched { get; set; }
    public int CostResearch { get; set; }
    public float HowMuchResearchedNow { get; set; }
    public string Description { get; set; }
    public float Cost { get; set; }
    public int Id { get; set; }
    public bool IsOutdated { get; set; }
    public int FirstAmountUpdate { get; set; }
    public int SecondAmountUpdate { get; set; }
    public Sciense Sciense { get; set; }


    public void ResearchTech(float value)
    {
        HowMuchResearchedNow += value;
        if(HowMuchResearchedNow >= CostResearch)
        {
            Research();
        }
    }

    protected abstract void Research();
    
    public void SaveData(SaveLoadTechnology save)
    {
        save.nameTech = NameTech;
        save.description = Description;
        save.isResearched = IsResearched;
        save.isOutdated = IsOutdated;
        save.costResearch = CostResearch;
        save.id = Id;
        save.firstAmountUpdate = FirstAmountUpdate;
        save.secondAmountUpdate = SecondAmountUpdate;
        save.howMuchResearchedNow = HowMuchResearchedNow;
        save.cost = Cost;

        SaveAnotherData(save);
    }

    protected abstract void SaveAnotherData(SaveLoadTechnology save);
    
    public void LoadData(SaveLoadTechnology save)
    {
        NameTech = save.nameTech;
        Description = save.description;
        IsResearched = save.isResearched;
        IsOutdated = save.isOutdated;
        CostResearch = save.costResearch;
        Id = save.id;
        FirstAmountUpdate = save.firstAmountUpdate;
        SecondAmountUpdate = save.secondAmountUpdate;
        HowMuchResearchedNow = save.howMuchResearchedNow;
        Cost = save.cost;

        LoadAnotherData(save);
    }

    protected abstract void LoadAnotherData(SaveLoadTechnology save);
}
