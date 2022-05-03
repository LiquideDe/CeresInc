using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateCom : MonoBehaviour, IModule
{
    public int Type { get; }
    public int Workplaces { get; }
    public float MaxEnergy { get; }
    public float MaxOutput { get; }
    public int MaxHealthConstruction { get; }
    
    public TemplateCom()
    {
        Type = 3;
        Workplaces = 4;
        MaxEnergy = -8;
        MaxOutput = 12;
        MaxHealthConstruction = 100;
        
    }
}
