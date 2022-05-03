using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleStartForShip : ToggleForShip
{
    protected override void TurningOff()
    {
        ship.StartNotAllowed();
    }

    protected override bool TurningOn()
    {
        return ship.StartAllowed();
    }
}
