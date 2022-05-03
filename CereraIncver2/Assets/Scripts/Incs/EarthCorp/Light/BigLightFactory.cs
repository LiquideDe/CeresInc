using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigLightFactory : LightBuilding
{
    
    public BigLightFactory(EarthCorp corp, bool isConstructed = false)
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

        DaysForConstruct = 400;
        MaxDaysWorking = 5475;
        BuildingName = "Big Light Factory";
        MaxOutput = 1500;
        energy = 3;
        heavy = 150;

        IsConstructed = isConstructed;
        materialsCoefConsuption[0] = 500;//H
        materialsCoefConsuption[1] = 0;//He
        materialsCoefConsuption[2] = 100;//Li
        materialsCoefConsuption[3] = 100;//Be
        materialsCoefConsuption[4] = 500;//C
        materialsCoefConsuption[5] = 100;//N
        materialsCoefConsuption[6] = 200;//Mg
        materialsCoefConsuption[7] = 200;//Al
        materialsCoefConsuption[8] = 300;//Si
        materialsCoefConsuption[9] = 50;//Ne
        materialsCoefConsuption[10] = 200;//Ti
        materialsCoefConsuption[11] = 100;//Cr
        materialsCoefConsuption[12] = 100;//Fe
        materialsCoefConsuption[13] = 400;//Ni
        materialsCoefConsuption[14] = 100;//Cu
        materialsCoefConsuption[15] = 100;//Xe
        materialsCoefConsuption[16] = 50;//Ir
        materialsCoefConsuption[17] = 50;//Pt
        materialsCoefConsuption[18] = 0;//Po
        materialsCoefConsuption[19] = 80;//Th
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
        materialsNeedToConstruction[8] = 2000;//Si
        materialsNeedToConstruction[9] = 1000;//Ar
        materialsNeedToConstruction[10] = 5000;//Ti
        materialsNeedToConstruction[11] = 100;//Cr
        materialsNeedToConstruction[12] = 20000;//Fe
        materialsNeedToConstruction[13] = 10000;//Ni
        materialsNeedToConstruction[14] = 10000;//Cu
        materialsNeedToConstruction[15] = 0;//Xe
        materialsNeedToConstruction[16] = 0;//Ir
        materialsNeedToConstruction[17] = 500;//Pt
        materialsNeedToConstruction[18] = 0;//Po
        materialsNeedToConstruction[19] = 0;//Th
        materialsNeedToConstruction[20] = 0;//U
        materialsNeedToConstruction[21] = 0;//Pu
        materialsNeedToConstruction[22] = 0;//Ea
    }
    
}
