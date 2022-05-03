using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleRepeatForShip : ToggleForShip
{
    protected override void TurningOff()
    {
        ship.Navigator.Repeat = false;
    }

    protected override bool TurningOn()
    {
        ship.Navigator.Repeat = true;
        return true;
    }
}
