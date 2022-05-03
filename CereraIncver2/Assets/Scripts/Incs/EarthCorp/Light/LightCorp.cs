using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCorp : EarthCorp, ICorporateConsumption
{
    public float GoodsAtEarth { get { return MainClass.Earth.LightGoods; } set { MainClass.Earth.LightGoods = value; } }
    private List<LightBuilding> tempLatesBuilding;
    public LightCorp(main main)
    {
        MainClass = main;
        CorpName = "LightInc";
        Money = 10000000;
        AmountShare = 10000;
        ProductionFactor = 1;
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
                buildings.Add(new BigLightFactory(this));
                break;

        }
    }

    public void StartGame(bool isNewGame)
    {
        
        tempLatesBuilding = new List<LightBuilding>() { new BigLightFactory(this) };
        MainClass.CorpPanel.SellShareToMarket(GetIdCorp(), 200);
        if (isNewGame)
        {
            buildings.Add(new BigLightFactory(this, true));
        }
    }

    protected override int GetIdCorp()
    {
        return 2;
    }

    protected override void SellGoodsToEarth()
    {
        //Продаем товар на землю, по полной стоимости, то что нужно населению, по половине стоимости остальное
        float needToBuy = MainClass.Earth.Population * MainClass.Earth.LightGoodsCoef;
        MainClass.Earth.LightGoods += SellSpecificGoods(needToBuy, MainClass.Earth.LightGoods);        
    }

    public override void SetBuilding(int id, SaveLoadBuilding save)
    {
        buildings.Add(tempLatesBuilding[id]);
        buildings[buildings.Count - 1].GetCleanBuilding().LoadData(save);
    }
}
