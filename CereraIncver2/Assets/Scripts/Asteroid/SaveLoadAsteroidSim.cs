using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadAsteroidSim 
{
    public float posX, posY, posZ, elementCapacity, elementAbundance, distance, id;
    public bool isStationBuilded;
    public string asterName;
    public int idElement;

    public float excavatedSoil, food, equipment, foodPlanned, equipmentPlanned, incomeLastMonth, amountReadyForLoading;
    public int workersOnStation, awaitingWorkers, workersPlanned;
}
