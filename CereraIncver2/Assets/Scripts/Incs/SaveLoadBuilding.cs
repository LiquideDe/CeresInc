using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadBuilding
{
    public List<float> materialsConsuption;
    public List<float> materialsCoefConsuption;
    public List<float> materialsNeedToConstruction;

    public int idEarthCorp, daysForConstruct, startDayConstruction, finishDayConstruction, maxDaysWorking, indexInTemplate;
    public bool isConstructed, isMaterialEnough;
    public float efficiency, coefFromEvent, maxOutput;
        
}
