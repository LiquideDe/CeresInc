using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResButton : MonoBehaviour
{
    [SerializeField] private Text nameAsteroid, textAmountRes, textFood, textWorker, textEquipment;
    public IAsteroid Asteroid { get; set; }
    public int Id { get; set; }

    public void UpdateText()
    {
        nameAsteroid.text = Asteroid.AsterName;
        textWorker.text = $"{Asteroid.Workers}/{Asteroid.WorkersPlanned}";
        textFood.text = $"{Asteroid.Food}";
        textEquipment.text = $"{Asteroid.Equipment}";
        textAmountRes.text = $"{Asteroid.ExcavatedSoil}";
    }
}
