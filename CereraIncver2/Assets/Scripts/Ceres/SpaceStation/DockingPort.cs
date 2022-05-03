using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockingPort : MonoBehaviour
{
    private Ship mooredShip;
    public bool IsOccupied { get; set; }
    public bool IsWorking { get; set; }

    public void Docking(Ship ship)
    {
        mooredShip = ship;
        IsOccupied = true;
    }

    private void Start()
    {
        IsOccupied = false;
        IsWorking = false;
    }

    public void UnDocking()
    {
        mooredShip = null;
        IsOccupied = false;
    }
}
