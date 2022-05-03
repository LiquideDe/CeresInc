using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceCorp : EarthCorp, ICorporateConsumption
{
    public float GoodsAtEarth { get { return MainClass.Earth.SpaceGoods; } set { MainClass.Earth.SpaceGoods = value; } }

    private List<SpaceBuilding> tempLatesBuilding;
    public SpaceCorp(main main)
    {
        MainClass = main;
        CorpName = "SpaceInc";
        Money = 1000000000;
        AmountShare = 10000;
        ProductionFactor = 1;
    }

    public void StartGame(bool isNewGame)
    {        
        tempLatesBuilding = new List<SpaceBuilding>() { new BigSpaceFactory(this) };
        MainClass.CorpPanel.SellShareToMarket(GetIdCorp(), 200);
        if (isNewGame)
        {
            buildings.Add(new BigSpaceFactory(this, true));
        }
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
                buildings.Add(new BigSpaceFactory(this));
                break;

        }
    }
    protected override int GetIdCorp()
    {
        return 4;
    }

    protected override void SellGoodsToEarth()
    {
        //Продаем товар на землю, по полной стоимости, то что нужно населению, по половине стоимости остальное
        float needToBuy = MainClass.Earth.Population * MainClass.Earth.SpaceGoodsCoef;
        MainClass.Earth.SpaceGoods += SellSpecificGoods(needToBuy, MainClass.Earth.SpaceGoods);
    }

    public override void SetBuilding(int id, SaveLoadBuilding save)
    {
        buildings.Add(tempLatesBuilding[id]);
        buildings[buildings.Count - 1].GetCleanBuilding().LoadData(save);
    }
}
