using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Route
{
    public main MainClass { get; set; }
    private List<IAsteroid> destinations = new List<IAsteroid>();
    public string NameRoute { get; set; }
    public float Distance { get; set; }
    public int AmountShipsOnRoute { get; set; }
    public int Id { get; set; }

    public Route(main main)
    {
        MainClass = main;
    }
    public IAsteroid GetDestination(int id)
    {
        return destinations[id];
    }

    public int CountDestination()
    {
        return destinations.Count;
    }

    public void SetDestination(IAsteroid asteroid)
    {
        destinations.Add(asteroid);
    }

    public void RemoveDestination(IAsteroid asteroid)
    {
        destinations.Remove(asteroid);
    }

    public int GetIdDestination(IAsteroid asteroid)
    {
        return destinations.IndexOf(asteroid);
    }
    public bool ContainMas(IAsteroid aster)
    {
        //Содержится ли астероид в массиве
        bool contain = false;
        for (int i = 0; i < destinations.Count; i++)
        {
            if (destinations[i] == aster)
            {
                contain = true;
                break;
            }
        }
        return contain;
    }

    public float CalculateTotalLengthRoute()
    {
        float totalLength = 0;
        if (destinations.Count > 0)
        {            
            totalLength += Vector3.Distance(new Vector3(0, 0, 0), destinations[0].Position);
            for (int i = 1; i < destinations.Count; i++)
            {
                totalLength += Vector3.Distance(destinations[i - 1].Position, destinations[i].Position);
            }
            Distance = totalLength;
        }
        
        return totalLength;
    }

    public void SaveData(SaveLoadRoute save)
    {
        Debug.Log($"Сохраняем параметры пути");
        save.nameRoute = NameRoute;
        save.distance = Distance;
        save.id = Id;
        save.amountShipsOnRoute = AmountShipsOnRoute;

        for(int i = 0; i < destinations.Count; i++)
        {
            save.idDestinations.Add(destinations[i].Id);
        }
    }

    public void LoadData(SaveLoadRoute save, bool isPlayers)
    {
        NameRoute = save.nameRoute;
        Distance = save.distance;
        Id = save.id;
        AmountShipsOnRoute = save.amountShipsOnRoute;

        if (isPlayers)
        {
            for (int i = 0; i < save.idDestinations.Count; i++)
            {
                destinations.Add(MainClass.Asteroids.GetAsteroid(save.idDestinations[i]));
            }
        }
        else
        {
            for (int i = 0; i < save.idDestinations.Count; i++)
            {
                destinations.Add(MainClass.Asteroids.GetSimAsteroid(save.idDestinations[i]));
            }
        }
        
    }
}
