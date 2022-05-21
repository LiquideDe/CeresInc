using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienseForPlayer : Sciense
{
    [SerializeField] private List<TechBut> techButs = new List<TechBut>();
    protected List<List<Technology>> lists = new List<List<Technology>>();
    
    public float MoreResourceFromScinse { get; set; }

    public override void CalculationMonth()
    {
        for (int i = 0; i < techButs.Count; i++)
        {
            techButs[i].MonthPass();
        }
    }

    protected override void GiveTechForBut(int id)
    {
        for (int i = 0; i < lists[id].Count; i++)
        {
            if (!lists[id][i].IsResearched)
            {
                techButs[id].technology = lists[id][i];
                techButs[id].UpdateText();
                break;
            }
        }

    }

    public override void PutInListAndButton()
    {
        lists.Add(new List<Technology>());
        lists[0].AddRange(techCarcass);
        lists.Add(new List<Technology>());
        lists[1].AddRange(techFuelTank);
        lists.Add(new List<Technology>());
        lists[2].AddRange(techEngine);
        lists.Add(new List<Technology>());
        lists[3].AddRange(techResource);
        for (int i = 0; i < 4; i++)
        {
            GiveTechForBut(i);
        }
    }

    public TechBut GetTechBut(int id)
    {
        return techButs[id];
    }

    protected override void CreateResTech()
    {
        for (int i = 0; i < 15; i++)
        {
            Resource resource = mainClass.Materials.GetMaterial(UnityEngine.Random.Range(0, mainClass.Materials.MaterialsCount()));
            techResource.Add(new ResTech($"Улучшением добычи {resource.ElementName}", (i + 1) * 1000, UnityEngine.Random.Range(1, 10), UnityEngine.Random.Range(5, 15), resource, this));
        }
    }

    public override void ResTechResearched(int id)
    {
        BetterDigFromSciense += lists[3][id].FirstAmountUpdate;
        MoreResourceFromScinse += lists[3][id].SecondAmountUpdate;
        GiveTechForBut(3);
    }

    protected override void SaveAnotherData(SaveLoadSciense save)
    {
        for(int i=0; i<4; i++)
        {
            save.sliders.Add(techButs[i].sliderCost.value);
        }
    }

    protected override void LoadAnotherData(SaveLoadSciense save)
    {
        Debug.Log($"Загружаем карточки, количество карточек сохраненных {save.sliders.Count}");
        for (int i = 0; i < 4; i++)
        {
            Debug.Log($"Загружаем слайдер {i}");
            Debug.Log($"Сейчас у техбата {techButs[i].sliderCost.value}");
            Debug.Log($"А в сохраненках {save.sliders[i]}");
            techButs[i].sliderCost.value = save.sliders[i];
        }
    }
}
