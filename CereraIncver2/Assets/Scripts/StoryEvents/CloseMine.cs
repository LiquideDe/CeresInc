using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseMine : StoryEvent
{
    public int IdMaterial { get; set; }

    public CloseMine(int idMaterial, int durTime, float impFact, int dayStart, EventPanel evPanel)
    {
        DurationTime = durTime;
        ImpactFactor = impFact;
        DayOfStart = dayStart;
        EventPanel = evPanel;

        IdMaterial = idMaterial;
        IdType = 1;
    }

    public CloseMine(EventPanel evPanel)
    {
        EventPanel = evPanel;
    }
    public override void EventHappen()
    {
        DescriptionMessage = $"Одно из Месторождений {EventPanel.Main.Materials.GetMaterial(IdMaterial).ElementName} истощилось, производство упало на {ImpactFactor * 100}%. ";
        EventPanel.Main.Materials.GetMaterial(IdMaterial).CleanProduction -= ImpactFactor * EventPanel.Main.Materials.GetMaterial(IdMaterial).CleanProduction;
        Debug.Log($"производство упало на {ImpactFactor * EventPanel.Main.Materials.GetMaterial(IdMaterial).CleanProduction}");
        CheckDuration();
    }

    protected override bool ReturnToNormal()
    {
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
