using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Building 
{
    protected List<float> materialsConsuption;
    protected List<float> materialsCoefConsuption;
    protected List<float> materialsNeedToConstruction;    
    public EarthCorp Corp { get; set; }
    public bool IsConstructed { get; set; }
    public int DaysForConstruct { get; set; }
    public int StartDayConstruction { get; set; }
    public int FinishDayConstruction { get; set; }
    public int MaxDaysWorking { get; set; }
    public float Efficiency { get; set; }
    public string BuildingName { get; set; }
    public float MaxOutput { get; set; }
    public float Output { get; set; }
    public bool IsMaterialEnough { get; set; }
    public float SpentMoney { get; set; }
    public float CoefFromEvent { get; set; }
    public int IndexInTemplate { get; set; }

    protected void Ñonstruction(int dayNow)
    {
        if (!IsMaterialEnough)
        {
            float sum = 0;
            for (int i = 0; i < materialsNeedToConstruction.Count; i++)
            {
                if(Corp.MainClass.Earth.GetAmountRes(i) > materialsNeedToConstruction[i] && materialsNeedToConstruction[i] != 0)
                {
                    Corp.MainClass.Earth.PlusResAmount(i, -materialsNeedToConstruction[i]);
                    materialsNeedToConstruction[i] = 0;
                }
                sum += materialsNeedToConstruction[i];
            }

            if(sum == 0)
            {
                IsMaterialEnough = true;
                StartDayConstruction =(int)Corp.MainClass.CeresTime;
            }
            else
            {                
                DaysForConstruct += 30;
            }
        }

        if(dayNow > StartDayConstruction + DaysForConstruct && IsMaterialEnough)
        {
            IsConstructed = true;
            FinishDayConstruction = dayNow;
        }
    }

    private void CalculateEfficiency(int dayNow)
    {
        Efficiency = (1 - (dayNow - FinishDayConstruction) / MaxDaysWorking);
        if(Efficiency < 0)
        {
            Efficiency = 0;
        }
    }

    protected void CalculateOutput(float coef)
    {
        Output = MaxOutput * coef * Efficiency * Corp.ProductionFactor * CoefFromEvent;
    }

    protected void Working()
    {
        CalculateEfficiency((int)Corp.MainClass.CeresTime);
        ZeroingConsumption();
        CalculationConsumprtion();
        RepairBuild();
    }
    protected abstract void CalculationConsumprtion();
    private void ZeroingConsumption()
    {
        for(int i = 0; i < materialsConsuption.Count; i++)
        {
            materialsConsuption[i] = 0;
        }
    }

    public float GetOutput()
    {
        return Output;
    }

    protected void CreatingList(List<float> list, int capacity)
    {
        for(int i = 0; i < capacity; i++)
        {
            list.Add(0);
        }
    }

    public List<float> CalculationSeason()
    {
        if (IsConstructed)
        {
            Working();
        }
        else
        {
            Ñonstruction((int)Corp.MainClass.CeresTime);
        }
        return materialsConsuption;
    }

    public float GetSpentMoney()
    {
        return SpentMoney;
    }

    public float UpkeepCost()
    {
        float sum = 0;
        for(int i = 0; i < materialsCoefConsuption.Count; i++)
        {
            sum += materialsCoefConsuption[i] * Corp.MainClass.Materials.GetMaterial(i).Price;
        }
        return sum;
    }

    public float ConstructionWorth(int coef = 1)
    {
        float sum = 0;
        for(int i=0;i< materialsNeedToConstruction.Count; i++)
        {
            sum += materialsNeedToConstruction[i] * coef * Corp.MainClass.Materials.GetMaterial(i).Price;
        }
        return sum;
    }

    public bool CheckResourceAvailability()
    {
        bool answ = false;
        int counterAvailableRes = 0;
        for(int i = 0; i < materialsConsuption.Count; i++)
        {
            if (materialsConsuption[i] == 0)
            {
                counterAvailableRes++;
            }
            else if (Corp.MainClass.Materials.GetMaterial(i).Production - Corp.MainClass.Earth.GetTotalResConsumption(i) > materialsConsuption[i])
            {
                counterAvailableRes++;
            }
        }

        if(counterAvailableRes == materialsConsuption.Count)
        {
            answ = true;
        }

        return answ;
    }

    private void RepairBuild()
    {
        if(Efficiency < 0.6)
        {
            MaxDaysWorking += 200;
        }
    }

    public Building GetCleanBuilding()
    {
        return this;
    }

    public void SaveData(SaveLoadBuilding save)
    {
        save.daysForConstruct = DaysForConstruct;
        save.startDayConstruction = StartDayConstruction;
        save.finishDayConstruction = FinishDayConstruction;
        save.maxDaysWorking = MaxDaysWorking;
        save.indexInTemplate = IndexInTemplate;
        save.isConstructed = IsConstructed;
        save.isMaterialEnough = IsMaterialEnough;
        save.efficiency = Efficiency;
        save.coefFromEvent = CoefFromEvent;
        save.maxOutput = MaxOutput;
    }

    public void LoadData(SaveLoadBuilding save)
    {
        DaysForConstruct = save.daysForConstruct;
        StartDayConstruction = save.startDayConstruction;
        FinishDayConstruction = save.finishDayConstruction;
        MaxDaysWorking = save.maxDaysWorking;
        IsConstructed = save.isConstructed;
        IsMaterialEnough = save.isMaterialEnough;
        Efficiency = save.efficiency;
        CoefFromEvent = save.coefFromEvent;
        MaxOutput = save.maxOutput;
    }
}
