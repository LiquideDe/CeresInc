using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCorp : EarthCorp, ICorporateConsumption
{
    public float GoodsAtEarth { get { return 0; } set { } }
    private List<EnergyBuilding> tempLatesBuilding;
    public EnergyCorp(main main)
    {
        MainClass = main;
        CorpName = "EnergyInc";     
        Money = 100000000;
        AmountShare = 10000;
        ProductionFactor = 1;
        IdType = 0;
    }

    public void StartGame(bool isNewGame)
    {        
        tempLatesBuilding = new List<EnergyBuilding>() { new SolarBuild(this), new OilPowerPlant(this), new UraniumAtomic(this) };
        MainClass.CorpPanel.SellShareToMarket(GetIdCorp(), 200);
        if (isNewGame)
        {
            buildings.Add(new SolarBuild(this, true));
            buildings[0].IndexInList = 0;
        }
    }

    protected override void InfrastructureDevelopment()
    {
        if (reservedOutputPrevious / totalOutput > 0.8 && !IsBuildingUnderConstruction() && ProductionFactor == 1)
        {
            int idBuild = 0;
            float worthOutput = tempLatesBuilding[0].UpkeepCost()/ tempLatesBuilding[0].MaxOutput;
            for (int i = 0; i < tempLatesBuilding.Count; i++)
            {
                if (tempLatesBuilding[i].UpkeepCost() / tempLatesBuilding[i].MaxOutput < worthOutput)
                {
                    idBuild = i;
                    worthOutput = tempLatesBuilding[i].UpkeepCost() / tempLatesBuilding[i].MaxOutput;                    
                }
            }
            if(tempLatesBuilding[idBuild].ConstructionWorth() * 2 < Money && tempLatesBuilding[idBuild].CheckResourceAvailability())
            {
                AddNewBuild(idBuild);
                Money -= tempLatesBuilding[idBuild].ConstructionWorth();
                Debug.Log($"!!!!! В {CorpName} строит новое здание {tempLatesBuilding[idBuild].BuildingName} стоимостью {tempLatesBuilding[idBuild].ConstructionWorth()}");
            }
            else
            {
                Debug.Log($"!!! У{CorpName} у компании не хватает денег на {tempLatesBuilding[idBuild].BuildingName} стоимостью {tempLatesBuilding[idBuild].ConstructionWorth()}");
            }
            
        }
    }

    private void AddNewBuild(int id)
    {
        switch (id)
        {
            case 0:
                buildings.Add(new SolarBuild(this));                
                break;
            case 1:
                buildings.Add(new OilPowerPlant(this));
                break;
            case 2:
                buildings.Add(new UraniumAtomic(this));
                break;
        }
        SetId();
    }

    protected override int GetIdCorp()
    {
        return 0;
    }

    protected override void SellGoodsToEarth()
    {
        float needToBuy = MainClass.Earth.Population * MainClass.Earth.EnergyGoodsCoef;
        
        if (GetTotalOutput() - GetReservedOutput() >= needToBuy)
        {
            ReservedOutput(needToBuy);
        }
        else if (GetTotalOutput() - GetReservedOutput() > 0)
        {
            ReservedOutput(GetTotalOutput() - GetReservedOutput());
        }
    }

    public override void SetBuilding(int id, SaveLoadBuilding save)
    {
        buildings.Add(tempLatesBuilding[id]);
        buildings[buildings.Count - 1].GetCleanBuilding().LoadData(save);
    }
}
