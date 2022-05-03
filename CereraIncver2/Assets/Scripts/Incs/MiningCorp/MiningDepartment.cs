using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiningDepartment
{
    public MiningCorporate Corporate { get; set; }
    private List<AsteroidForSimulation> asteroids = new List<AsteroidForSimulation>();

    public MiningDepartment(MiningCorporate corporate)
    {
        Corporate = corporate;
    }   

    public void FindNearestAsteroid()
    {
        int index = 0;
        float distance = 10000;
        if(asteroids.Count == 0)
        {
            for (int i = 0; i < Corporate.MainClass.Asteroids.AsteroidsCount(); i++)
            {
                if (Corporate.MainClass.Asteroids.GetSimAsteroid(i).Distance < distance && Corporate.MainClass.Asteroids.GetSimAsteroid(i).Element == Corporate.OrientRes)
                {
                    index = i;
                    distance = Corporate.MainClass.Asteroids.GetSimAsteroid(i).Distance;
                }
            }
            AddAsteroid(Corporate.MainClass.Asteroids.GetSimAsteroid(index));
        }
        else
        {
            for (int i = 0; i < Corporate.MainClass.Asteroids.AsteroidsCount(); i++)
            {
                if (!Corporate.MainClass.Asteroids.GetSimAsteroid(i).HasMiningStation && Vector3.Distance(asteroids.Last().Position, Corporate.MainClass.Asteroids.GetSimAsteroid(i).Position) < distance 
                    && Corporate.MainClass.Asteroids.GetSimAsteroid(i).Element == Corporate.OrientRes)
                {
                    index = i;
                    distance = Vector3.Distance(asteroids.Last().Position, Corporate.MainClass.Asteroids.GetSimAsteroid(i).Position);
                }
            }
            if(index != 0)
            {
                AddAsteroid(Corporate.MainClass.Asteroids.GetSimAsteroid(index));
            }
            else
            {
                DistributionWorkers();
            }
            
        }
        
    }

    private void AddAsteroid(AsteroidForSimulation asteroid)
    {
        asteroids.Add(asteroid);
        asteroid.HasMiningStation = true;
        Debug.Log($"ƒобавл€ем на астероид {asteroid.AsterName} {Corporate.GetFreeWorkers(20)} рабочих, всего астероидов {asteroids.Count}");
        asteroid.WorkersPlanned += Corporate.GetFreeWorkers(20);
        asteroid.CalculateSupplyConsuption();
    }

    private void DistributionWorkers()
    {
        for(int i = 0; i < asteroids.Count; i++)
        {
            if(asteroids[i].WorkersOnStation + asteroids[i].WorkersPlanned < 20)
            {
                asteroids[i].WorkersPlanned += Corporate.GetFreeWorkers(20 - (asteroids[i].WorkersOnStation + asteroids[i].WorkersPlanned));
            }
        }
    }

    public int CountAsteroids()
    {
        return asteroids.Count;
    }

    public AsteroidForSimulation GetAsteroid(int id)
    {
        return asteroids[id];
    }

    public void SetAsteroid(AsteroidForSimulation asteroid)
    {
        asteroids.Add(asteroid);
    }
}
