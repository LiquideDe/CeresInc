using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComModule : SpaceStation
{
    public override void DisableInterfaceComponent()
    {
        Destroy(gameObject.GetComponent<TemplateCom>());
    }

    public override void GetSeasonOutput()
    {
        Ceres.MaxShipsAtJourney = (int)Output;
    }
}
