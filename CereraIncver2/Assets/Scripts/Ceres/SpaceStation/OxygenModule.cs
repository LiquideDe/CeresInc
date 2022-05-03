using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenModule : SpaceStation
{
    public override void DisableInterfaceComponent()
    {
        Destroy(gameObject.GetComponent<TemplateOxygen>());
    }

    public override void GetSeasonOutput()
    {
        Ceres.GeneralOxygen += Output;
    }
}
