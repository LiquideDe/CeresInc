using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccidentAtMiningStation : StoryEvent
{
    public int AmountWorkers { get; set; }
    public MiningStation Station { get; set; }

    public AccidentAtMiningStation(MiningStation station, int amountWorkers,string textMessage, int durTime, float impFact, int dayStart, EventPanel evPanel)
    {
        DurationTime = durTime;
        ImpactFactor = impFact;
        DayOfStart = dayStart;
        EventPanel = evPanel;
        AmountWorkers = amountWorkers;
        DescriptionMessage = textMessage;
        Station = station;
        IdType = 4;
    }

    public AccidentAtMiningStation(EventPanel evPanel)
    {
        EventPanel = evPanel;
    }
    public override void EventHappen()
    {
        Station.WorkersOnStation -= AmountWorkers;
        CheckDuration();
    }

    protected override bool ReturnToNormal()
    {
        return true;
    }

    protected override void SaveAnotherData(SaveLoadAccident save)
    {
        save.amountWorkers = AmountWorkers;
        save.idStation = Station.Asteroid.Id;
    }

    protected override void LoadAnotherData(SaveLoadAccident save)
    {
        AmountWorkers = save.amountWorkers;
        Station = EventPanel.Main.Asteroids.GetAsteroid(save.idStation).MiningStation;
    }
}
