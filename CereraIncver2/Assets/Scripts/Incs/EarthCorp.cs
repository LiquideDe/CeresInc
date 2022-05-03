using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EarthCorp: ICorporateShare
{
    public main MainClass { get; set; }
    public string CorpName { get; set; }
    public float Money { get; set; }
    public float Price { get; set; }
    public float PriceShare { get; set; }
    public int AmountShare { get; set; }
    public float IncomeMoney { get; set; }
    public float OutlayMoney { get; set; }
    public float ProductionFactor { get; set; }
    protected List<float> materialsConsuption;
    protected List<float> materialsCoefConsuption;
    protected List<float> totalConsumption = new List<float>();
    protected List<IBuilding> buildings = new List<IBuilding>();
    protected float totalOutput, reservedOutput, reservedOutputPrevious;

    public void CalculateSeason()
    {
        reservedOutputPrevious = reservedOutput;
        reservedOutput = 0;
        totalConsumption.Clear();
        for (int i = 0; i < buildings.Count; i++)
        {
            CalculateConsumption(buildings[i].CalculationSeason());
        }
        CalculateTotalOutput();
        CalculationPrice();        
        InfrastructureDevelopment();        
    }

    public void PostCalculation()
    {
        SellGoodsToEarth();
        CalculationProductionFactor();
        ReceivingAProfit();
        CalculationPriceShare();
        SellShare();
    }

    private void CalculateConsumption(List<float> building)
    {
        CheckLengthConsumption();
        

        for (int i = 0; i < building.Count; i++)
        {
            totalConsumption[i] += building[i];
        }
    }

    private void CheckLengthConsumption()
    {
        if (totalConsumption.Count != MainClass.Materials.MaterialsCount())
        {
            CreatingList(totalConsumption, MainClass.Materials.MaterialsCount());
        }
    }

    private void CalculateTotalOutput()
    {
        totalOutput = 0;
        for (int i = 0; i < buildings.Count; i++)
        {
            totalOutput += buildings[i].GetOutput();
        }
    }

    public float GetConsumption(int id)
    {
        CheckLengthConsumption();
        return totalConsumption[id];
    }

    public float GetTotalOutput()
    {
        return totalOutput;
    }

    public float GetReservedOutput()
    {
        return reservedOutput;
    }

    private void CreatingList(List<float> list, int capacity)
    {
        for (int i = 0; i < capacity; i++)
        {
            list.Add(0);
        }
    }

    public void ReservedOutput(float value)
    {
        reservedOutput += value;
        IncomeMoney += value * Price;
    }

    private void CalculationPrice()
    {
        float sum = 0;
        for(int i = 0; i < buildings.Count; i++)
        {
            sum += buildings[i].GetSpentMoney();
        }
        if(totalOutput > 0)
        {
            Price = sum / totalOutput;
        }

        Price += Price * 0.2f;
        OutlayMoney += sum;
    }

    protected abstract void InfrastructureDevelopment();

    protected bool IsBuildingUnderConstruction()
    {
        bool answ = false;
        for(int i = 0; i < buildings.Count; i++)
        {
            if (!buildings[i].IsConstructed)
            {
                answ = true;
                break;
            }
        }

        return answ;
    }

    private void CalculationPriceShare()
    {
        PriceShare = (Money + Price * totalOutput + Price * reservedOutputPrevious) / 10000;
    }

    private void SellShare()
    {
        float mon = Money;
        if(Money < 0)
        {
            mon = 1;
        }
        if (Money < 1000000 && PriceShare > 1)
        {
            int amount = (1000000 - (int)mon) / (int)PriceShare;
            if (amount > AmountShare || amount > 3000)
            {
                if(AmountShare <= 3000)
                {
                    amount = 0;
                }
                else
                {
                    amount = AmountShare - 3000;
                }
            }
            else
            {
                amount = 0;
            }
            MainClass.CorpPanel.SellShareToMarket(GetIdCorp(), amount);
            Money += amount * PriceShare;
            AmountShare -= amount;
        }
    }

    protected abstract int GetIdCorp();

    protected abstract void SellGoodsToEarth();

    protected float SellSpecificGoods(float needToBuy, float goods)
    {
        float soldGoods = 0;
        if (goods <= needToBuy && GetTotalOutput() - GetReservedOutput() >= needToBuy)
        {
            soldGoods += needToBuy;
            ReservedOutput(needToBuy);
            Money += (GetTotalOutput() - GetReservedOutput()) * Price * 0.5f;
        }
        else if (goods < needToBuy && GetTotalOutput() - GetReservedOutput() < needToBuy)
        {
            soldGoods += GetTotalOutput() - GetReservedOutput();
            ReservedOutput(GetTotalOutput() - GetReservedOutput());
        }
        else if (goods >= needToBuy && GetTotalOutput() - GetReservedOutput() > 0)
        {
            soldGoods += GetTotalOutput() - GetReservedOutput();
            Money += (GetTotalOutput() - GetReservedOutput()) * Price * 0.5f;
        }
        return soldGoods;
    }

    private void ReceivingAProfit()
    {
        Money += IncomeMoney - OutlayMoney;
        IncomeMoney = 0;
        OutlayMoney = 0;
    }

    private void CalculationProductionFactor()
    {
        if(totalOutput > 0)
        {
            if(GetReservedOutput() / totalOutput > 0.9)
            {
                ProductionFactor += ProductionFactor * 0.1f;
            }
            else if(GetReservedOutput() / totalOutput < 0.85)
            {
                ProductionFactor -= ProductionFactor * 0.1f;
            }            
            
            if(ProductionFactor > 1)
            {
                ProductionFactor = 1;
            }
        }
        
        if(ProductionFactor <= 0)
        {
            ProductionFactor = 0.1f;
        }        
    }

    public int CountBuildings()
    {
        return buildings.Count;
    }

    public IBuilding GetBuilding(int id)
    {
        return buildings[id];
    }

    public void SaveData(SaveLoadEarthCorp save)
    {
        save.money = Money;
        save.price = Price;
        save.priceShare = PriceShare;
        save.incomeMoney = IncomeMoney;
        save.outlayMoney = OutlayMoney;
        save.productionFactor = ProductionFactor;
        save.amountShare = AmountShare;
        save.amountBuildings = CountBuildings();
    }

    public void LoadData(SaveLoadEarthCorp save)
    {
        Money = save.money;
        Price = save.price;
        PriceShare = save.priceShare;
        IncomeMoney = save.incomeMoney;
        OutlayMoney = save.outlayMoney;
        ProductionFactor = save.productionFactor;
        AmountShare = save.amountShare;
    }

    public abstract void SetBuilding(int id, SaveLoadBuilding save);
}
