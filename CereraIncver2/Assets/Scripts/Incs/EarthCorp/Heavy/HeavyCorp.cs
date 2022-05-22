using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyCorp : EarthCorp, ICorporateConsumption
{
    public float GoodsAtEarth { get { return MainClass.Earth.HeavyGoods; } set { MainClass.Earth.HeavyGoods = value; } }
    private List<HeavyBuilding> tempLatesBuilding;
    public HeavyCorp(main main)
    {
        MainClass = main;
        CorpName = "HeavyInc";
        Money = 10000000;
        AmountShare = 10000;
        ProductionFactor = 1;
        IdType = 1;
    }

    protected override void InfrastructureDevelopment()
    {
        if (reservedOutputPrevious / totalOutput > 0.8 && !IsBuildingUnderConstruction() && ProductionFactor == 1)
        {
            int idBuild = 0;
            float upkeep = tempLatesBuilding[0].UpkeepCost();
            for (int i = 0; i < tempLatesBuilding.Count; i++)
            {
                if (tempLatesBuilding[i].UpkeepCost() < upkeep && tempLatesBuilding[i].CheckResourceAvailability())
                {
                    idBuild = i;
                    upkeep = tempLatesBuilding[i].UpkeepCost();
                }
            }
            if (tempLatesBuilding[idBuild].CheckResourceAvailability() && tempLatesBuilding[idBuild].ConstructionWorth() * 2 < Money)
            {
                AddNewBuild(idBuild);
                Money -= tempLatesBuilding[idBuild].ConstructionWorth();
            }
        }
    }

    private void AddNewBuild(int id)
    {
        switch (id)
        {
            case 0:
                buildings.Add(new BigHeavyFactory(this));
                break;
            
        }
        SetId();
    }

    public void StartGame(bool isNewGame)
    {
        
        tempLatesBuilding = new List<HeavyBuilding>() { new BigHeavyFactory(this) };
        MainClass.CorpPanel.SellShareToMarket(GetIdCorp(), 200);
        if (isNewGame)
        {
            buildings.Add(new BigHeavyFactory(this, true));
            buildings[0].IndexInList = 0;
        }
    }

    protected override int GetIdCorp()
    {
        return 1;
    }

    protected override void SellGoodsToEarth()
    {
        //Продаем товар на землю, по полной стоимости, то что нужно населению, по половине стоимости остальное
        float needToBuy = MainClass.Earth.Population * MainClass.Earth.HeavyGoodsCoef;
        MainClass.Earth.HeavyGoods += SellSpecificGoods(needToBuy, MainClass.Earth.HeavyGoods);
    }

    public override void SetBuilding(int id, SaveLoadBuilding save)
    {
        buildings.Add(tempLatesBuilding[id]);
        buildings[buildings.Count - 1].GetCleanBuilding().LoadData(save);
    }
}
