using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModule
{
    int Type { get; }
    int Workplaces { get; }
    float MaxEnergy { get; }
    float MaxOutput { get; }
    int MaxHealthConstruction { get; }

}
