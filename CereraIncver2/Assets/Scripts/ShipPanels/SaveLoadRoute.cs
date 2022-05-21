using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadRoute 
{
    public string nameRoute;
    public float distance;
    public int amountShipsOnRoute, id;

    public List<int> idDestinations = new List<int>();
}
