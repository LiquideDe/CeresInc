using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarehouseModule : SpaceStation
{
    private DockingPort dockingPort;
    public override void DisableInterfaceComponent()
    {
        Destroy(gameObject.GetComponent<TemplateWarehouse>());
    }

    public override void GetSeasonOutput()
    {
        if(dockingPort != null)
        {
            dockingPort.IsWorking = true;
        }        
    }

    public DockingPort IsDockFree()
    {
        if (!dockingPort.IsOccupied)
        {
            return dockingPort;
        }
        else
        {
            return null;
        }
    }

    private void Start()
    {
        dockingPort = transform.Find("WarehouseModel/Dock").GetComponent<DockingPort>();
    }
}
