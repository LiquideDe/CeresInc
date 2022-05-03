using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindNewMine : StoryEvent
{
    public int IdMaterial { get; set; }

    public FindNewMine(int idMaterial, int durTime, float impFact, int dayStart, EventPanel evPanel)
    {
        DurationTime = durTime;
        ImpactFactor = impFact;
        DayOfStart = dayStart;
        EventPanel = evPanel;

        IdMaterial = idMaterial;
    }
    public override void EventHappen()
    {
        DescriptionMessage = $"Было найдено новое месторождение {EventPanel.Main.Materials.GetMaterial(IdMaterial).ElementName}, производство выросло на {ImpactFactor * 100}%. ";
        EventPanel.Main.Materials.GetMaterial(IdMaterial).Production += ImpactFactor * EventPanel.Main.Materials.GetMaterial(IdMaterial).Production;
        CheckDuration();
    }

    protected override bool ReturnToNormal()
    {
        return true;
    }
}
