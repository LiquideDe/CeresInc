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


}
