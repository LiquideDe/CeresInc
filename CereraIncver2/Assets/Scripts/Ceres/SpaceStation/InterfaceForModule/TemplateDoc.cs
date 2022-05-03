
using UnityEngine;

public class TemplateDoc : MonoBehaviour, IModule
{
    public int Type { get; }
    public int Workplaces { get; }
    public float MaxEnergy { get; }
    public float MaxOutput { get; }
    public int MaxHealthConstruction { get; }
    
    public TemplateDoc()
    {
        Type = 4;
        Workplaces = 5;
        MaxEnergy = -1;
        MaxHealthConstruction = 150;
        MaxOutput = 4;
        
    }
}
