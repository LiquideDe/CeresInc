using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveModule : SpaceStation
{
    public override void DisableInterfaceComponent()
    {
        Destroy(gameObject.GetComponent<TemplateLive>());
    }

    public override void GetSeasonOutput()
    {
        Ceres.MaxFreeWorkers = (int)Output;
    }
}
