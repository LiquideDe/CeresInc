using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadShipSim
{
    public float food, equipment, dV, daysForTrip, startDay, cargo, weight, iSP, weightFuel, maxWeightPlayload, weightPlayload, distance, dvToOperation, costOfJourney;
    public int workers, typeFuel, typeShip, id;
    public List<float> distances = new List<float>();
    public List<int> idDestinations = new List<int>();
    public string shipName;
    public bool isInJourney, isLastDestination;
}
