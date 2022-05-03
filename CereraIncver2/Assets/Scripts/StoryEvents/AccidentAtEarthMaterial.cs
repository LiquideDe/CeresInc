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
    }
    public override void EventHappen()
    {
        DescriptionMessage = $"�� ������ �� ������ {EventPanel.Main.Materials.GetMaterial(IdMaterial).ElementName}, ���-�� ���������. ";
        EventPanel.Main.Materials.GetMaterial(IdMaterial).CoefFromEvent = 1 - ImpactFactor;
        CheckDuration();
    }

    protected override bool ReturnToNormal()
    {
        EventPanel.Main.Materials.GetMaterial(IdMaterial).CoefFromEvent = 1;
        return true;
    }
}
