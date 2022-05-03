using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyBuilding : Building, IBuilding
{
    protected float energy;
    protected float energyCoef;
    protected override void CalculationConsumprtion()
    {
        SpentMoney = 0;
        float summ = 0;
        energyCoef = TakeGoods(Corp.MainClass.Earth.Energy, energy);
        SpentMoney += Corp.MainClass.Earth.Energy.Price * energy * energyCoef;
        for (int i = 0; i < materialsCoefConsuption.Count; i++)
        {
            if (Corp.MainClass.Earth.GetAmountRes(i) >= materialsCoefConsuption[i] * energyCoef)
            {
                materialsConsuption[i] = materialsCoefConsuption[i] * energyCoef;
                Corp.MainClass.Earth.PlusResAmount(i, -materialsCoefConsuption[i] * energyCoef);
                summ += 1 * energyCoef;
            }
            else
            {
                materialsConsuption[i] = Corp.MainClass.Earth.GetAmountRes(i) * energyCoef;
                Corp.MainClass.Earth.SetAmountRes(i, 0);
                summ += materialsConsuption[i] / materialsCoefConsuption[i] * energyCoef;
            }
            SpentMoney += Corp.MainClass.Materials.GetMaterial(i).Price * materialsConsuption[i];
        }

        summ /= Corp.MainClass.Materials.MaterialsCount();
        CalculateOutput(summ);
    }

    private float TakeGoods(ICorporateConsumption corp, float needs)
    {
        float coef;
        if (corp.GetTotalOutput() - corp.GetReservedOutput() >= needs)
        {
            corp.ReservedOutput(needs);
            coef = 1;
        }
        else if ((corp.GetTotalOutput() - corp.GetReservedOutput()) + corp.GoodsAtEarth >= needs)
        {
            coef = 1;
            corp.GoodsAtEarth -= needs - (corp.GetTotalOutput() - corp.GetReservedOutput());
            corp.ReservedOutput(corp.GetTotalOutput() - corp.GetReservedOutput());

        }
        else if ((corp.GetTotalOutput() - corp.GetReservedOutput()) + corp.GoodsAtEarth > 0)
        {
            coef = (corp.GetTotalOutput() - corp.GetReservedOutput() + corp.GoodsAtEarth) / needs;
            corp.ReservedOutput(corp.GetTotalOutput() - corp.GetReservedOutput());
            corp.GoodsAtEarth = 0;
        }
        else
        {
            coef = 0;
        }

        if (float.IsNaN(corp.Price))
        {
            corp.Price = 1;
        }

        float money = needs * corp.Price * coef;
        SpentMoney += money;

        return coef;
    }
}
