using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StoryEvent 
{
    public EventPanel EventPanel { get; set; }
    public int DurationTime { get; set; }
    public float ImpactFactor { get; set; }
    public int DayOfStart { get; set; }
    public string DescriptionMessage { get; set; }
    public bool IsDone { get; set; }
    public bool IsItAtNews { get; set; }
    public int IdType { get; set; }

    public abstract void EventHappen();

    protected void CheckDuration()
    {
        if (!IsItAtNews)
        {
            EventPanel.SetNews(DescriptionMessage);
            IsItAtNews = true;
        }

        if (DayOfStart + DurationTime <= EventPanel.Main.CeresTime)
        {
            EndEvent();
        }
    }

    private void EndEvent()
    {
        if (ReturnToNormal())
            IsDone = true;
    }

    protected abstract bool ReturnToNormal();

    public void SaveData(SaveLoadAccident save)
    {
        save.durationTime = DurationTime;
        save.impactFactor = ImpactFactor;
        save.dayOfStart = DayOfStart;
        save.descriptionMessage = DescriptionMessage;
        save.isDone = IsDone;
        save.isItAtNews = IsItAtNews;
        save.idType = IdType;
        SaveAnotherData(save);
    }

    protected abstract void SaveAnotherData(SaveLoadAccident save);

    public void LoadData(SaveLoadAccident save)
    {
        DurationTime = save.durationTime;
        ImpactFactor = save.impactFactor;
        DayOfStart = save.dayOfStart;
        DescriptionMessage = save.descriptionMessage;
        IsDone = save.isDone;
        IsItAtNews = save.isItAtNews;
        IdType = save.idType;
        LoadAnotherData(save);
    }

    protected abstract void LoadAnotherData(SaveLoadAccident save);
}
