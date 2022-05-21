using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadMiningCorp 
{
    public int freeWorkers, plannedWorkerForWork, idOrientRes, ships, amountRoutes;
    public float food, equipment, amountResource, priceShare, price, amountShare, money;
    public bool isConstructShip, isRoutesCreate, isNeedComponentsForNewShips;

    public List<int> carcassPas = new List<int>();
    public List<int> carcassCargo = new List<int>();
    public List<int> engine = new List<int>();
    public List<int> fuelTank = new List<int>();

    public List<int> asteroids = new List<int>();
}
