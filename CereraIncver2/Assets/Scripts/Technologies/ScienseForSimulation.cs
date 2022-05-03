using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScienseForSimulation : Sciense
{
    private Carcass carcass;
    private FuelTank fuelTank;
    private Engine engine;
    private ResTech resTech;
    public MiningCorporate Corporate { get { return corporate; } }
    public float MoneyForTechCarcass { get; set; }
    public float MoneyForTechFuelTank { get; set; }
    public float MoneyForTechEngine { get; set; }
    public float MoneyForTechRes { get; set; }
    public float MoreAmountResourceFromSciense { get; set; }
    [SerializeField] private MiningCorporate corporate;

    public override void CalculationMonth()
    {
        carcass.ResearchTech(MoneyForTechCarcass);
        fuelTank.ResearchTech(MoneyForTechFuelTank);
        engine.ResearchTech(MoneyForTechEngine);
        resTech.ResearchTech(MoneyForTechRes);

    }

    public override void PutInListAndButton()
    {
        carcass = techCarcass[GetNotResearchedTech(0)];
        fuelTank = techFuelTank[GetNotResearchedTech(1)];
        engine = techEngine[GetNotResearchedTech(2)];
        if(Corporate.OrientRes != null)
        {
            resTech = techResource[GetNotResearchedTech(3)];
        }
        
    }

    public override void ResTechResearched(int id)
    {
        BetterDigFromSciense += resTech.FirstAmountUpdate;
        MoreAmountResourceFromSciense += resTech.SecondAmountUpdate;
        GiveTechForBut(3);
    }

    protected override void CreateResTech()
    {
        if(Corporate.OrientRes != null)
        {
            for (int i = 0; i < 15; i++)
            {
                int first = mainClass.GenerateRandomInt(1,10);
                int second = mainClass.GenerateRandomInt(5,15);
                techResource.Add(new ResTech($"Улучшением добычи {Corporate.OrientRes.ElementName}", (i + 1) * 1000, first, second, Corporate.OrientRes, this));
            }
        }
        
    }

    public void ResTechInit()
    {
        CreateResTech();
        SetId();
        GiveTechForBut(3);
    }

    protected override void GiveTechForBut(int type)
    {
        if (type == 0)
        {
            carcass = techCarcass[GetNotResearchedTech(0)];
        }
        else if (type == 1)
        {
            fuelTank = techFuelTank[GetNotResearchedTech(1)];
        }
        else if (type == 2)
        {
            engine = techEngine[GetNotResearchedTech(2)];
        }
        else if (type == 3)
        {
            resTech = techResource[GetNotResearchedTech(3)];
        }
    }

    private int GetNotResearchedTech(int type)
    {
        int answ = 0;
        if(type == 0)
        {
            for(int i=0; i < techCarcass.Count; i++)
            {
                if (!techCarcass[i].IsResearched)
                {
                    answ = i;
                    break;
                } 
            }
        }
        else if(type == 1)
        {
            for (int i = 0; i < techFuelTank.Count; i++)
            {
                if (!techFuelTank[i].IsResearched)
                {
                    answ = i;
                    break;
                }
            }
        }
        else if (type == 2)
        {
            for (int i = 0; i < techEngine.Count; i++)
            {
                if (!techEngine[i].IsResearched)
                {
                    answ = i;
                    break;
                }
            }
        }
        else if (type == 3)
        {
            for (int i = 0; i < techResource.Count; i++)
            {
                if (!techResource[i].IsResearched)
                {
                    answ = i;
                    break;
                }
            }
        }
        return answ;
    }

    protected override void SaveAnotherData(SaveLoadSciense save)
    {
        save.moneyForTechCarcass = MoneyForTechCarcass;
        save.moneyForTechFuelTank = MoneyForTechFuelTank;
        save.moneyForTechEngine = MoneyForTechEngine;
        save.moneyForTechRes = MoneyForTechRes;
        save.moreAmountResourceFromSciense = MoreAmountResourceFromSciense;
    }

    protected override void LoadAnotherData(SaveLoadSciense save)
    {
        MoneyForTechCarcass = save.moneyForTechCarcass;
        MoneyForTechFuelTank = save.moneyForTechFuelTank;
        MoneyForTechEngine = save.moneyForTechEngine;
        MoneyForTechRes = save.moneyForTechRes;
        MoreAmountResourceFromSciense = save.moreAmountResourceFromSciense;
    }
}
