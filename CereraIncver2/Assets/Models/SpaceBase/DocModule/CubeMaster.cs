using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMaster : MonoBehaviour
{
    public GameObject cube;
    List<GameObject> cubes = new List<GameObject>();

    private void Start()
    {
        for (int i = 0; i < 20; i++)
        {
            cubes.Add(Instantiate(cube));            
        }

        StartCoroutine(CycleCube());

        
    }


    private IEnumerator CreateCoordinateForAsteroid(Transform asteroid, int id)
    {
        
        Vector3 position = new Vector3(UnityEngine.Random.Range(0, 500) - 250, 0, UnityEngine.Random.Range(0, 500) - 250);
        Collider[] neighbours = Physics.OverlapSphere(position, 50);

        Debug.Log($"Создаем новые координаты {position}, количество вхождений {neighbours.Length}, номер куба {id}");
        if (neighbours.Length > 0)
        {
            yield return StartCoroutine(CreateCoordinateForAsteroid(asteroid, id));
        }
        else
        {
            asteroid.position = position;
        }
        yield return new WaitForSeconds(0.1f); 
    }

    private IEnumerator CycleCube()
    {
        Debug.Log($"ddd");
        for (int i = 0; i < 20; i++)
        {
            yield return StartCoroutine(CreateCoordinateForAsteroid(cubes[i].transform, i));
        }
        yield return new WaitForSeconds(0.1f);
    }
}
