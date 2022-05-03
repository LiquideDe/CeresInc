using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgroModule : SpaceStation
{
    public override void DisableInterfaceComponent()
    {
        Destroy(gameObject.GetComponent<TemplateAgro>());
    }

    public override void GetSeasonOutput()
    {
        Ceres.Food += Output;
    }
}
