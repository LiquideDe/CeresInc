using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyBuilding : Building, IBuilding
{
    protected override void CalculationConsumprtion()
    {
        SpentMoney = 0;
        float summ = 0;
        for (int i = 0; i < materialsCoefConsuption.Count; i++)
        {
            if (Corp.MainClass.Earth.GetAmountRes(i) >= materialsCoefConsuption[i])
            {
                materialsConsuption[i] = materialsCoefConsuption[i];
                Corp.MainClass.Earth.PlusResAmount(i, -materialsCoefConsuption[i]);
            }
            else
            {
                materialsConsuption[i] = Corp.MainClass.Earth.GetAmountRes(i);
                Corp.MainClass.Earth.SetAmountRes(i, 0);
            }
            SpentMoney += materialsConsuption[i] * Corp.MainClass.Materials.GetMaterial(i).Price;
        }

        for (int i = 0; i < materialsCoefConsuption.Count; i++)
        {
            if (materialsConsuption[i] == materialsCoefConsuption[i])
            {
                summ += 1;
            }
            else
            {
                summ += materialsConsuption[i] / materialsCoefConsuption[i];
            }
        }
        summ = summ / Corp.MainClass.Materials.MaterialsCount();
        CalculateOutput(summ);
    }

    
}
