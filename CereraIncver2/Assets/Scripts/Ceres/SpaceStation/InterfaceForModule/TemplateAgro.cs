using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemplateAgro : MonoBehaviour, IModule
{
    public int Type { get; }
    public int Workplaces { get; }
    public float MaxEnergy { get; }
    public float MaxOutput { get; }
    public int MaxHealthConstruction { get; }
    
    public TemplateAgro()
    {
        Type = 5;
        Workplaces = 8;
        MaxEnergy = -10;
        MaxOutput = 15;
        MaxHealthConstruction = 100;        
    }
}
