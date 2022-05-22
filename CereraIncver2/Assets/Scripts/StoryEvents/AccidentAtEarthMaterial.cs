using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccidentAtEarthMaterial : StoryEvent
{
    public int IdMaterial { get; set; }
    public AccidentAtEarthMaterial(int idMaterial, int durTime, float impFact, int dayStart, EventPanel evPanel)
    {
        DurationTime = durTime;
        ImpactFactor = impFact;
        DayOfStart = dayStart;
        EventPanel = evPanel;

        IdMaterial = idMaterial;
        IdType = 3;
    }
    public AccidentAtEarthMaterial(EventPanel evPanel)
    {
        EventPanel = evPanel;
    }
    public override void EventHappen()
    {
        DescriptionMessage = $"На шахтах по добыче {EventPanel.Main.Materials.GetMaterial(IdMaterial).ElementName}, что-то случилось. ";
        EventPanel.Main.Materials.GetMaterial(IdMaterial).CoefFromEvent = 1 - ImpactFactor;
        CheckDuration();
    }

    protected override bool ReturnToNormal()
    {
        EventPanel.Main.Materials.GetMaterial(IdMaterial).CoefFromEvent = 1;
        return true;
    }

    protected override void SaveAnotherData(SaveLoadAccident save)
    {
        save.idMaterial = IdMaterial;
    }

    protected override void LoadAnotherData(SaveLoadAccident save)
    {
        IdMaterial = save.idMaterial;
    }
}
