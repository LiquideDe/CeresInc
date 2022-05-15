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

    public void FindAsteroids()
    {
        Debug.Log($"Ищем астероиды");
        for(int i = 0; i < Corporate.MainClass.Asteroids.AsteroidsCount(); i++)
        {
            if(Corporate.MainClass.Asteroids.GetSimAsteroid(i).Element == Corporate.OrientRes)
            {
                asteroids.Add(Corporate.MainClass.Asteroids.GetSimAsteroid(i));
                Debug.Log($"Добавили астероид {asteroids[asteroids.Count - 1].AsterName}");
            }
        }

        for(int i = 0; i < asteroids.Count; i++)
        {
            asteroids[i].WorkersPlanned = 25;
            asteroids[i].HasMiningStation = true;
            asteroids[i].CalculateSupplyConsuption();
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

    public List<AsteroidForSimulation> GetAllAsteroids()
    {
        return asteroids;
    }
}
