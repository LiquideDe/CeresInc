using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightBuilding : Building, IBuilding
{
    protected float energy, heavy;
    protected float energyCoef, heavyCoef;

    protected override void CalculationConsumprtion()
    {
        SpentMoney = 0;
        float summ = 0;
        energyCoef = TakeGoods(Corp.MainClass.Earth.Energy, energy);
        heavyCoef = TakeGoods(Corp.MainClass.Earth.HeavyIndustry, heavy);
        SpentMoney += Corp.MainClass.Earth.Energy.Price * energy * energyCoef + Corp.MainClass.Earth.HeavyIndustry.Price * heavy * heavyCoef;
        float totalCoef = energyCoef * heavyCoef;
        for (int i = 0; i < materialsCoefConsuption.Count; i++)
        {
            if (Corp.MainClass.Earth.GetAmountRes(i) >= materialsCoefConsuption[i] * totalCoef)
            {
                materialsConsuption[i] = materialsCoefConsuption[i] * totalCoef;
                Corp.MainClass.Earth.PlusResAmount(i, -materialsCoefConsuption[i] * totalCoef);
                summ += 1 * totalCoef;
            }
            else
            {
                materialsConsuption[i] = Corp.MainClass.Earth.GetAmountRes(i) * totalCoef;
                Corp.MainClass.Earth.SetAmountRes(i, 0);
                summ += materialsConsuption[i] / materialsCoefConsuption[i] * totalCoef;
            }
            SpentMoney += materialsConsuption[i] * Corp.MainClass.Materials.GetMaterial(i).Price;
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
