
using UnityEngine;

public class TemplateLive : MonoBehaviour, IModule
{
    public int Type { get; }
    public int Workplaces { get; }
    public float MaxEnergy { get; }
    public float MaxOutput { get; }
    public int MaxHealthConstruction { get; }


    public TemplateLive()
    {
        Type = 0;
        Workplaces = 2;
        MaxEnergy = -10;
        MaxOutput = 50;
        MaxHealthConstruction = 300;

    }
}
