using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidCenter : MonoBehaviour
{
    private List<AsteroidForPlayer> asteroids = new List<AsteroidForPlayer>();
    private List<AsteroidForSimulation> simAsteroids = new List<AsteroidForSimulation>();
    [SerializeField] private Transform asteroidList, asteroidSimList;
    [SerializeField] private GameObject Asteroid, asteroidSimTemplate;
    [SerializeField] private main mainClass;

    

    public IEnumerator CreateAsteroids(int amount, bool isNewGame = true)
    {
        for (int i = 0; i < amount; i++)
        {
            GameObject asteroidGame = Instantiate(Asteroid);
            var asteroid = asteroidGame.GetComponent<AsteroidForPlayer>();
            asteroidGame.SetActive(true);
            asteroidGame.transform.GetChild(1).gameObject.SetActive(false);
            asteroid.Id = i;
            asteroidGame.transform.SetParent(asteroidList);
            asteroids.Add(asteroid);

            GameObject gameObject = Instantiate(asteroidSimTemplate);
            gameObject.transform.SetParent(asteroidSimList);
            simAsteroids.Add(gameObject.GetComponent<AsteroidForSimulation>());
        }

        if (isNewGame)
        {
            yield return StartCoroutine(AsteroidAllocation(amount));
        }
        else
        {
            yield return new WaitForSeconds(0.01f);
        }
        
    }

    private IEnumerator AsteroidAllocation(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            yield return StartCoroutine(CreateCoordinateForAsteroid(asteroids[i].transform, i));
        }
        yield return new WaitForSeconds(0.01f);
    }

    private IEnumerator CreateCoordinateForAsteroid(Transform asteroid, int id)
    {
        Vector3 position = new Vector3(UnityEngine.Random.Range(0, 24000) - 12000, -80, UnityEngine.Random.Range(0, 10000) - 5000);
        Collider[] neighbours = Physics.OverlapSphere(position, 500);

        if (neighbours.Length > 0)
        {
            yield return StartCoroutine(CreateCoordinateForAsteroid(asteroid, id));
        }
        else
        {
            asteroid.position = position;
            GenerateProperties(id);
            CopyProperties(id);
        }
        yield return new WaitForSeconds(0.01f);
    }

    private void GenerateProperties(int id)
    {
        asteroids[id].Distance = Vector3.Distance(new Vector3(0, 0, 0), asteroids[id].transform.position);
        asteroids[id].Position = asteroids[id].transform.position;
        asteroids[id].AsteroidModel.rotation = new Quaternion(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360));
        asteroids[id].AsteroidModel.localScale = new Vector3(UnityEngine.Random.Range(10, 50), UnityEngine.Random.Range(10, 50), UnityEngine.Random.Range(10, 50));
        float maxSize = 0;
        for (int k = 0; k < 3; k++)
        {
            if (asteroids[id].AsteroidModel.localScale[k] > maxSize)
                maxSize = asteroids[id].AsteroidModel.localScale[k];
        }
        asteroids[id].Circle.transform.localScale = new Vector3(maxSize, maxSize, maxSize);
        asteroids[id].SizeofCircle = maxSize;
        asteroids[id].Element = mainClass.Materials.GetRandomMaterial();
        asteroids[id].AsterName = $"{asteroids[id].Element.ElementName}-{id}";
        asteroids[id].ElementCapacity = UnityEngine.Random.Range(1000, 50000);
        asteroids[id].ElementAbundance = 100f / UnityEngine.Random.Range(5, 200);
        if(id == 249)
        {
            Debug.Log($"Расстановка завершена");
            mainClass.PreStartIsDone();
        }
    }

    private void CopyProperties(int id)
    {
        simAsteroids[id].AsterName = asteroids[id].AsterName;
        simAsteroids[id].Distance = asteroids[id].Distance;
        simAsteroids[id].Element = asteroids[id].Element;
        simAsteroids[id].ElementAbundance = asteroids[id].ElementAbundance;
        simAsteroids[id].ElementCapacity = asteroids[id].ElementCapacity;
        simAsteroids[id].Id = asteroids[id].Id;
        simAsteroids[id].Position = asteroids[id].Position;
    }

    public AsteroidForPlayer GetAsteroid(int id)
    {
        return asteroids[id];
    }

    public int AsteroidsCount()
    {
        return asteroids.Count;
    }

    public AsteroidForSimulation GetSimAsteroid(int id)
    {
        return simAsteroids[id];
    }

    public void CalculationMonth()
    {
        for (int i = 0; i < asteroids.Count; i++)
        {
            if (asteroids[i].MiningStation != null)
            {
                asteroids[i].MiningStation.CalculateMonth();
            }
            if (simAsteroids[i].HasMiningStation)
            {
                simAsteroids[i].CalculateMonth();
            }
        }
    }
}
