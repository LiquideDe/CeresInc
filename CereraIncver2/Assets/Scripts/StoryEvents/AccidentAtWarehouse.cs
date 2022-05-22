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
        IdType = 5;
    }
    public AccidentAtWarehouse(EventPanel evPanel)
    {
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

    protected override void SaveAnotherData(SaveLoadAccident save)
    {
        save.idMaterial = IdMaterial;
        save.amountRes = AmountRes;
    }

    protected override void LoadAnotherData(SaveLoadAccident save)
    {
        IdMaterial = save.idMaterial;
        AmountRes = save.amountRes;
    }
}
