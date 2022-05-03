using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigHeavyFactory : HeavyBuilding
{
    
    public BigHeavyFactory(EarthCorp corp, bool isConstructed = false)
    {
        IndexInTemplate = 0;
        CoefFromEvent = 1;
        Corp = corp;
        materialsConsuption = new List<float>();
        CreatingList(materialsConsuption, Corp.MainClass.Materials.MaterialsCount());

        materialsCoefConsuption = new List<float>();
        CreatingList(materialsCoefConsuption, Corp.MainClass.Materials.MaterialsCount());

        materialsNeedToConstruction = new List<float>();
        CreatingList(materialsNeedToConstruction, Corp.MainClass.Materials.MaterialsCount());

        DaysForConstruct = 500;
        MaxDaysWorking = 6500;
        BuildingName = "Big Heavy Factory";
        MaxOutput = 2000;
        energy = 2;

        IsConstructed = isConstructed;
        materialsCoefConsuption[0] = 0;//H
        materialsCoefConsuption[1] = 0;//He
        materialsCoefConsuption[2] = 0;//Li
        materialsCoefConsuption[3] = 0;//Be
        materialsCoefConsuption[4] = 0;//C
        materialsCoefConsuption[5] = 100;//N
        materialsCoefConsuption[6] = 200;//Mg
        materialsCoefConsuption[7] = 500;//Al
        materialsCoefConsuption[8] = 0;//Si
        materialsCoefConsuption[9] = 0;//Ne
        materialsCoefConsuption[10] = 50;//Ti
        materialsCoefConsuption[11] = 100;//Cr
        materialsCoefConsuption[12] = 500;//Fe
        materialsCoefConsuption[13] = 300;//Ni
        materialsCoefConsuption[14] = 500;//Cu
        materialsCoefConsuption[15] = 0;//Xe
        materialsCoefConsuption[16] = 0;//Ir
        materialsCoefConsuption[17] = 0;//Pt
        materialsCoefConsuption[18] = 0;//Po
        materialsCoefConsuption[19] = 50;//Th
        materialsCoefConsuption[20] = 0;//U
        materialsCoefConsuption[21] = 0;//Pu
        materialsCoefConsuption[22] = 0;//Ea

        materialsNeedToConstruction[0] = 0;//H
        materialsNeedToConstruction[1] = 0;//He
        materialsNeedToConstruction[2] = 0;//Li
        materialsNeedToConstruction[3] = 0;//Be
        materialsNeedToConstruction[4] = 0;//C
        materialsNeedToConstruction[5] = 0;//N
        materialsNeedToConstruction[6] = 200;//Mg
        materialsNeedToConstruction[7] = 10000;//Al
        materialsNeedToConstruction[8] = 0;//Si
        materialsNeedToConstruction[9] = 1000;//Ar
        materialsNeedToConstruction[10] = 5000;//Ti
        materialsNeedToConstruction[11] = 100;//Cr
        materialsNeedToConstruction[12] = 20000;//Fe
        materialsNeedToConstruction[13] = 10000;//Ni
        materialsNeedToConstruction[14] = 10000;//Cu
        materialsNeedToConstruction[15] = 0;//Xe
        materialsNeedToConstruction[16] = 0;//Ir
        materialsNeedToConstruction[17] = 100;//Pt
        materialsNeedToConstruction[18] = 0;//Po
        materialsNeedToConstruction[19] = 0;//Th
        materialsNeedToConstruction[20] = 0;//U
        materialsNeedToConstruction[21] = 0;//Pu
        materialsNeedToConstruction[22] = 0;//Ea
    }

    
}
