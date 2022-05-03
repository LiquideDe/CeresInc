using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccidentAtWarehouse : StoryEvent
{
    public int IdMaterial { get; set; }
    public float AmountRes { get; set; }

    public AccidentAtWarehouse(int idMaterial, int durTime, float impFact, int dayStart, EventPanel evPanel)
    {
        IdMaterial = idMaterial;
        DurationTime = durTime;
        ImpactFactor = impFact;
        DayOfStart = dayStart;
        EventPanel = evPanel;       
        
    }

    public override void EventHappen()
    {
        AmountRes = EventPanel.Main.Warehouse.GetRes(IdMaterial) * ImpactFactor;
        if (AmountRes > 0)
        {
            DescriptionMessage = $"На складе произошел хлопок из-за которого произошло высвобождение {EventPanel.Main.Materials.GetMaterial(IdMaterial).ElementName} в количестве {AmountRes}. ";
        }
        else
        {
            DescriptionMessage = $"На складе произошел хлопок в зоне {EventPanel.Main.Materials.GetMaterial(IdMaterial).ElementName}. ";
        }

        
        if(AmountRes > 0 && EventPanel.Main.Warehouse.GetRes(IdMaterial) >= AmountRes)
        {
            EventPanel.Main.Warehouse.PlusRes(IdMaterial, -(AmountRes));
        }
        CheckDuration();
    }

    protected override bool ReturnToNormal()
    {
        return true;
    }
}
