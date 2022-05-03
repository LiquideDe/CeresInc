using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OilPowerPlant : EnergyBuilding
{
    public OilPowerPlant(EarthCorp corp, bool isConstructed = false)
    {
        IndexInTemplate = 1;
        CoefFromEvent = 1;
        Corp = corp;
        materialsConsuption = new List<float>();
        CreatingList(materialsConsuption, Corp.MainClass.Materials.MaterialsCount());

        materialsCoefConsuption = new List<float>();
        CreatingList(materialsCoefConsuption, Corp.MainClass.Materials.MaterialsCount());

        materialsNeedToConstruction = new List<float>();
        CreatingList(materialsNeedToConstruction, Corp.MainClass.Materials.MaterialsCount());

        DaysForConstruct = 200;
        MaxDaysWorking = 3600;
        BuildingName = "Oil power plant";
        MaxOutput = 30;
        IsConstructed = isConstructed;
        materialsCoefConsuption[0] = 0;//H
        materialsCoefConsuption[1] = 0;//He
        materialsCoefConsuption[2] = 0;//Li
        materialsCoefConsuption[3] = 0;//Be
        materialsCoefConsuption[4] = 0;//C
        materialsCoefConsuption[5] = 0;//N
        materialsCoefConsuption[6] = 0;//Mg
        materialsCoefConsuption[7] = 0;//Al
        materialsCoefConsuption[8] = 0;//Si
        materialsCoefConsuption[9] = 0;//Ar
        materialsCoefConsuption[10] = 0;//Ti
        materialsCoefConsuption[11] = 0;//Cr
        materialsCoefConsuption[12] = 0;//Fe
        materialsCoefConsuption[13] = 0;//Ni
        materialsCoefConsuption[14] = 0;//Cu
        materialsCoefConsuption[15] = 0;//Xe
        materialsCoefConsuption[16] = 0;//Ir
        materialsCoefConsuption[17] = 0;//Pt
        materialsCoefConsuption[18] = 0;//Po
        materialsCoefConsuption[19] = 0;//Th
        materialsCoefConsuption[20] = 0;//U
        materialsCoefConsuption[21] = 0;//Pu
        materialsCoefConsuption[22] = 1000;//Ea

        materialsNeedToConstruction[0] = 0;//H
        materialsNeedToConstruction[1] = 0;//He
        materialsNeedToConstruction[2] = 0;//Li
        materialsNeedToConstruction[3] = 0;//Be
        materialsNeedToConstruction[4] = 0;//C
        materialsNeedToConstruction[5] = 0;//N
        materialsNeedToConstruction[6] = 0;//Mg
        materialsNeedToConstruction[7] = 5000;//Al
        materialsNeedToConstruction[8] = 100;//Si
        materialsNeedToConstruction[9] = 500;//Ar
        materialsNeedToConstruction[10] = 1000;//Ti
        materialsNeedToConstruction[11] = 0;//Cr
        materialsNeedToConstruction[12] = 10000;//Fe
        materialsNeedToConstruction[13] = 5000;//Ni
        materialsNeedToConstruction[14] = 5000;//Cu
        materialsNeedToConstruction[15] = 0;//Xe
        materialsNeedToConstruction[16] = 100;//Ir
        materialsNeedToConstruction[17] = 200;//Pt
        materialsNeedToConstruction[18] = 0;//Po
        materialsNeedToConstruction[19] = 0;//Th
        materialsNeedToConstruction[20] = 0;//U
        materialsNeedToConstruction[21] = 0;//Pu
        materialsNeedToConstruction[22] = 0;//Ea
    }
    
}
