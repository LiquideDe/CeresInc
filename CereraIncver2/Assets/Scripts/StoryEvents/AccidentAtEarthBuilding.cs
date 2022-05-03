using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccidentAtEarthBuilding : StoryEvent
{
    public EarthCorp EarthCorp { get; set; }
    public IBuilding Building { get; set; }

    public AccidentAtEarthBuilding(EarthCorp corp, IBuilding build, int durTime, float impFact, int dayStart, EventPanel evPanel)
    {
        DurationTime = durTime;
        ImpactFactor = impFact;
        DayOfStart = dayStart;
        EventPanel = evPanel;

        EarthCorp = corp;
        Building = build;
    }
    public override void EventHappen()
    {
        DescriptionMessage = $"�� ����������� {Building.BuildingName} ���������� {EarthCorp.CorpName} ��������� ������, � ����� ���� �����. �������������� ���������� ������ ��������� ����� {DurationTime + DayOfStart - (int)EventPanel.Main.CeresTime} ����. ";
        Building.CoefFromEvent = 1 - ImpactFactor;
        CheckDuration();
    }

    protected override bool ReturnToNormal()
    {
        Building.CoefFromEvent = 1;
        return true;
    }
}
