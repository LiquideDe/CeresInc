using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigSpaceFactory : SpaceBuilding
{
    
    public BigSpaceFactory(EarthCorp corp, bool isConstructed = false)
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

        DaysForConstruct = 900;
        MaxDaysWorking = 10000;
        BuildingName = "Big Space Factory";
        MaxOutput = 5;
        energy = 10;
        heavy = 300;
        light = 400;
        high = 300;

        IsConstructed = isConstructed;
        materialsCoefConsuption[0] = 1000;//H
        materialsCoefConsuption[1] = 100;//He
        materialsCoefConsuption[2] = 400;//Li
        materialsCoefConsuption[3] = 500;//Be
        materialsCoefConsuption[4] = 100;//C
        materialsCoefConsuption[5] = 520;//N
        materialsCoefConsuption[6] = 200;//Mg
        materialsCoefConsuption[7] = 500;//Al
        materialsCoefConsuption[8] = 800;//Si
        materialsCoefConsuption[9] = 400;//Ne
        materialsCoefConsuption[10] = 1000;//Ti
        materialsCoefConsuption[11] = 300;//Cr
        materialsCoefConsuption[12] = 100;//Fe
        materialsCoefConsuption[13] = 500;//Ni
        materialsCoefConsuption[14] = 400;//Cu
        materialsCoefConsuption[15] = 400;//Xe
        materialsCoefConsuption[16] = 200;//Ir
        materialsCoefConsuption[17] = 100;//Pt
        materialsCoefConsuption[18] = 20;//Po
        materialsCoefConsuption[19] = 100;//Th
        materialsCoefConsuption[20] = 10;//U
        materialsCoefConsuption[21] = 0;//Pu
        materialsCoefConsuption[22] = 0;//Ea

        materialsNeedToConstruction[0] = 0;//H
        materialsNeedToConstruction[1] = 0;//He
        materialsNeedToConstruction[2] = 2000;//Li
        materialsNeedToConstruction[3] = 0;//Be
        materialsNeedToConstruction[4] = 0;//C
        materialsNeedToConstruction[5] = 0;//N
        materialsNeedToConstruction[6] = 200;//Mg
        materialsNeedToConstruction[7] = 20000;//Al
        materialsNeedToConstruction[8] = 2000;//Si
        materialsNeedToConstruction[9] = 1000;//Ar
        materialsNeedToConstruction[10] = 5000;//Ti
        materialsNeedToConstruction[11] = 100;//Cr
        materialsNeedToConstruction[12] = 50000;//Fe
        materialsNeedToConstruction[13] = 10000;//Ni
        materialsNeedToConstruction[14] = 20000;//Cu
        materialsNeedToConstruction[15] = 0;//Xe
        materialsNeedToConstruction[16] = 650;//Ir
        materialsNeedToConstruction[17] = 5000;//Pt
        materialsNeedToConstruction[18] = 0;//Po
        materialsNeedToConstruction[19] = 1000;//Th
        materialsNeedToConstruction[20] = 0;//U
        materialsNeedToConstruction[21] = 0;//Pu
        materialsNeedToConstruction[22] = 0;//Ea
    }

    
}
