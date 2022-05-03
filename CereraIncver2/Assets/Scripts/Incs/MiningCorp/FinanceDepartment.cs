using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinanceDepartment 
{
    public float PriceShare { get; set; }
    public float AmountShare { get; set; }
    public MiningCorporate corporate;

    public FinanceDepartment(MiningCorporate corporate)
    {
        this.corporate = corporate;
        AmountShare = 10000;
    }

    public void CalculationPriceShare()
    {
        float sum = 0;
        for(int i=0;i< corporate.MiningDepartment.CountAsteroids(); i++)
        {
            sum += corporate.MiningDepartment.GetAsteroid(i).ElementCapacity;
        }
        PriceShare = (corporate.Money + sum * corporate.OrientRes.Price/5 + corporate.AmountResource * corporate.OrientRes.Price + corporate.ShipDepartment.CountShips() * 1000)/10000;
        DecisionBuyOrSellShare();
    }

    private void DecisionBuyOrSellShare()
    {
        if(corporate.Money < 1000000)
        {
            int amount = (1000000 - (int)corporate.Money)/(int)PriceShare;
            if(amount > AmountShare || amount > 3000)
            {
                amount = 3000;
            }
            else
            {
                amount = 0;
            }
            corporate.ShareMarket.SellShareToMarket(corporate.OrientRes.Id + 5, amount);
            corporate.Money += amount * PriceShare;
            AmountShare -= amount;
        }
    }
}
