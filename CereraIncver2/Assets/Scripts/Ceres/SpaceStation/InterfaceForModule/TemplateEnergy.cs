using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateEnergy : MonoBehaviour, IModule
{    public int Type { get; }
    public int Workplaces { get; }
    public float MaxEnergy { get; }
    public float MaxOutput { get; }
    public int MaxHealthConstruction { get; }
    
    public TemplateEnergy()
    {
        Type = 1;
        Workplaces = 10;
        MaxEnergy = 0;
        MaxOutput = 50;
        MaxHealthConstruction = 300;
        
    }
}
