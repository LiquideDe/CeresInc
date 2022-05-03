using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighCorp : EarthCorp, ICorporateConsumption
{
    public float GoodsAtEarth { get { return MainClass.Earth.HighGoods; } set { MainClass.Earth.HighGoods = value; } }
    private List<HighBuilding> tempLatesBuilding;
    public HighCorp(main main)
    {
        MainClass = main;
        CorpName = "HighInc";
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
                buildings.Add(new BigHighFactory(this));
                break;

        }
    }

    public void StartGame(bool isNewGame)
    {        
        tempLatesBuilding = new List<HighBuilding>() { new BigHighFactory(this) };
        MainClass.CorpPanel.SellShareToMarket(GetIdCorp(), 200);
        if (isNewGame)
        {
            buildings.Add(new BigHighFactory(this, true));
        }
    }

    protected override int GetIdCorp()
    {
        return 3;
    }

    protected override void SellGoodsToEarth()
    {
        //Продаем товар на землю, по полной стоимости, то что нужно населению, по половине стоимости остальное
        float needToBuy = MainClass.Earth.Population * MainClass.Earth.HighGoodsCoef;
        MainClass.Earth.HighGoods += SellSpecificGoods(needToBuy, MainClass.Earth.HighGoods);
    }

    public override void SetBuilding(int id, SaveLoadBuilding save)
    {
        buildings.Add(tempLatesBuilding[id]);
        buildings[buildings.Count - 1].GetCleanBuilding().LoadData(save);
    }
}
