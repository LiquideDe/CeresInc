using System;
using System.Collections.Generic;

public class SaveLoadShip 
{
    public float weightContruction, maxWeightFuel, weightFuel, maxWeightPlayload, weightPlayload;
    public int isp, typeFuel, typeShip, age, id;
    public string shipName;

    public float timeToJourney, dV, distance, distanceToNear, dvToOperation;
    public bool isInJourney, isDocked, isTimeForDockingEnd, lastDestination, repeat, isStartAllowed, isToWarehouse, isDockedToWarehouse;
    public int daysForDocking;
    public List<int> idDestinations = new List<int>();
    public List<int> idOldDestinations = new List<int>();

    public bool rotated, towards, rotatedLikeDock;
    public float posX, posY, posZ, quatX, quatY, quatZ, quatW, distanceForAcc;

    public int cargoWorkers, awaitingCargoWorkers, demandWorkers;
    public float cargoFood, cargoEquipment, awaitingCargoFood, awaitingCargoEquipment, demandEquip, demandFood;

    public float[] cargoElement = new float[22];
}
