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
        IdType = 2;
    }
    public AccidentAtEarthBuilding(EventPanel evPanel)
    {
        EventPanel = evPanel;
    }
    public override void EventHappen()
    {
        DescriptionMessage = $"На предприятии {Building.BuildingName} корпорации {EarthCorp.CorpName} произошел прорыв, а может даже рывок. Восстановление нормальной работы ожидается через {DurationTime + DayOfStart - (int)EventPanel.Main.CeresTime} дней. ";
        Building.CoefFromEvent = 1 - ImpactFactor;
        CheckDuration();
    }

    protected override bool ReturnToNormal()
    {
        Building.CoefFromEvent = 1;
        return true;
    }

    protected override void SaveAnotherData(SaveLoadAccident save)
    {
        save.idEarthCorp = EarthCorp.IdType;
        save.idBuilding = Building.IndexInList;
    }

    protected override void LoadAnotherData(SaveLoadAccident save)
    {
        EarthCorp = EventPanel.Main.Earth.GetEarthCorp(save.idEarthCorp);
        Building = EventPanel.Main.Earth.GetEarthCorp(save.idEarthCorp).GetBuilding(save.idBuilding);
    }
}
