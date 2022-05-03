using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TechBut : MonoBehaviour, IPointerClickHandler
{
    public Text textName, textDescription, textTimeRemain, textCost;
    public Slider sliderCost;
    public ScienseForPlayer sciense;
    public main mainClass;
    public Technology technology;
    private int maxPayForResearch = 1000;

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void ChangeSlider()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        int timeToResearch = (int)((technology.CostResearch - technology.HowMuchResearchedNow) / (sliderCost.value * maxPayForResearch / 50));
        textName.text = technology.NameTech;
        textDescription.text = technology.Description;
        if (timeToResearch < 0)
        {
            textTimeRemain.text = $"Никогда не будет изучено";
        }
        else
        {
            textTimeRemain.text = $"Будет изучено через {timeToResearch} месяцев";
        }
        
        
        textCost.text = $"На разработку в месяц уходит {(int)(maxPayForResearch*sliderCost.value)}";
    }

    public void MonthPass()
    {
        technology.ResearchTech(sliderCost.value * maxPayForResearch / 50);
        mainClass.Player.Money -= maxPayForResearch * sliderCost.value;
        UpdateText();
    }
}
