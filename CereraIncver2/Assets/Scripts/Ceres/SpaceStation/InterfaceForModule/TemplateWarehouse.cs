using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateWarehouse : MonoBehaviour, IModule
{
    public int Type { get; }
    public int Workplaces { get; }
    public float MaxEnergy { get; }
    public float MaxOutput { get; }
    public int MaxHealthConstruction { get; }


    public TemplateWarehouse()
    {
        Type = 6;
        Workplaces = 2;
        MaxEnergy = -3;
        MaxOutput = 0;
        MaxHealthConstruction = 200;
    }
}
