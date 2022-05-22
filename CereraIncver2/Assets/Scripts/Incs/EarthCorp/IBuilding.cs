using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilding
{
    public string BuildingName { get; set; }
    public bool IsConstructed { get; set; }
    public float CoefFromEvent { get; set; }
    public int IndexInTemplate { get; set; }
    public List<float> CalculationSeason();

    public float GetOutput();

    public float GetSpentMoney();

    public Building GetCleanBuilding();
    public int IndexInList { get; set; }
}
