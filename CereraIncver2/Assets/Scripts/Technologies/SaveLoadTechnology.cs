using System;

[Serializable]
public class SaveLoadTechnology 
{
    public string nameTech, description;
    public bool isResearched, isOutdated;
    public int costResearch, id, firstAmountUpdate, secondAmountUpdate;
    public float howMuchResearchedNow, cost, maxFuel;

    public float weight, maxWeightPlayload;

    public int isp, elementId, typeFuel;
    public float thrust;
}
