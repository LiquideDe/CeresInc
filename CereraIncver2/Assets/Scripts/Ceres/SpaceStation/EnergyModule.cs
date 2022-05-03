using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyModule : SpaceStation
{
    public override void DisableInterfaceComponent()
    {
        Destroy(gameObject.GetComponent<TemplateEnergy>());
    }

    public override void GetSeasonOutput()
    {
        
    }
}
