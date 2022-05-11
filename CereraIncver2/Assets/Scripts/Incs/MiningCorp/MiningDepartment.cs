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

    public void FindNearestAsteroid(int workers)
    {
        int index = 0;
        float distance = 100000;
        if(asteroids.Count < 3 || IsFreeWorkplaces())
        {
            if (asteroids.Count == 0)
            {
                for (int i = 0; i < Corporate.MainClass.Asteroids.AsteroidsCount(); i++)
                {
                    if (Corporate.MainClass.Asteroids.GetSimAsteroid(i).Distance < distance && Corporate.MainClass.Asteroids.GetSimAsteroid(i).Element == Corporate.OrientRes)
                    {
                        index = i;
                        distance = Corporate.MainClass.Asteroids.GetSimAsteroid(i).Distance;
                    }
                }
                AddAsteroid(Corporate.MainClass.Asteroids.GetSimAsteroid(index), workers);
                Debug.Log($"Обосновались на первом астероиде - {asteroids[0].AsterName}");
            }
            else
            {
                distance = 100000000000;
                for (int i = 0; i < Corporate.MainClass.Asteroids.AsteroidsCount(); i++)
                {
                    if (!Corporate.MainClass.Asteroids.GetSimAsteroid(i).HasMiningStation && Vector3.Distance(asteroids.Last().Position, Corporate.MainClass.Asteroids.GetSimAsteroid(i).Position) < distance
                        && Corporate.MainClass.Asteroids.GetSimAsteroid(i).Element == Corporate.OrientRes)
                    {
                        index = i;
                        distance = Vector3.Distance(asteroids.Last().Position, Corporate.MainClass.Asteroids.GetSimAsteroid(i).Position);
                    }
                }
                if (index != 0)
                {
                    AddAsteroid(Corporate.MainClass.Asteroids.GetSimAsteroid(index), workers);
                    Debug.Log($"Обосновались на втором астероиде. ");
                }
                else
                {
                    Debug.Log($"Не нашли астероида");
                    DistributionWorkers();
                    Corporate.NoMoreAsteroids = true;
                }
            }
        }
        
    }

    private void AddAsteroid(AsteroidForSimulation asteroid, int workers)
    {
        asteroids.Add(asteroid);
        asteroid.HasMiningStation = true;
        if(workers > 20)
        {
            asteroid.WorkersPlanned += Corporate.GetFreeWorkers(20);
            workers -= 20;
            asteroid.CalculateSupplyConsuption();
            FindNearestAsteroid(workers);
        }
        else
        {
            asteroid.WorkersPlanned += Corporate.GetFreeWorkers(workers);
            asteroid.CalculateSupplyConsuption();
        }     
        
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

    private bool IsFreeWorkplaces()
    {
        bool answ = false;
        for(int i = 0; i < asteroids.Count; i++)
        {
            if(asteroids[i].WorkersPlanned - asteroids[i].WorkersOnStation != 0)
            {
                answ = true;
                break;
            }
        }

        return answ;
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

    public List<AsteroidForSimulation> GetAllAsteroids()
    {
        return asteroids;
    }
}
