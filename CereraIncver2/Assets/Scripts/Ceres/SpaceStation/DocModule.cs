using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocModule : SpaceStation
{
    private List<DockingPort> docks = new List<DockingPort>();
    public override void DisableInterfaceComponent()
    {
        Destroy(gameObject.GetComponent<TemplateDoc>());
    }

    public override void GetSeasonOutput()
    {
        if(docks.Count > 0)
        {
            for (int i = 0; i < 4; i++)
            {
                docks[i].IsWorking = false;
            }
            for (int i = 0; i < (int)Output; i++)
            {
                docks[i].IsWorking = true;
            }
        }
        
    }

    private void Start()
    {
        docks.Add(transform.Find("DocModel/Dock1").GetComponent<DockingPort>());
        docks.Add(transform.Find("DocModel/Dock2").GetComponent<DockingPort>());
        docks.Add(transform.Find("DocModel/Dock3").GetComponent<DockingPort>());
        docks.Add(transform.Find("DocModel/Dock4").GetComponent<DockingPort>());
        GetSeasonOutput();
    }

    public DockingPort GetFreeDock()
    {
        DockingPort port = null;
        for(int i = 0; i < 4; i++)
        {
            if(docks[i].IsWorking && !docks[i].IsOccupied)
            {
                port = docks[i];

                return port;
            }
        }

        return port;
    }
}
