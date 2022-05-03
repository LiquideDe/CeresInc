using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateOxygen : MonoBehaviour, IModule
{
    public int Type { get; }
    public int Workplaces { get; }
    public float MaxEnergy { get; }
    public float MaxOutput { get; }
    public int MaxHealthConstruction { get; }

    public TemplateOxygen()
    {
        Type = 2;
        Workplaces = 4;
        MaxEnergy = -10;
        MaxOutput = 100;
        MaxHealthConstruction = 100;
        
    }
}
