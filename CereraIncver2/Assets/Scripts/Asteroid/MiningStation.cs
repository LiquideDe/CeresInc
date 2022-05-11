using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiningStation : MonoBehaviour
{
    [SerializeField] main mainClass;
    public float ExcavatedSoil { get; set; }
    public AsteroidForPlayer Asteroid { get; set; }
    public float Food { get; set; }
    public float FoodConsuption { get; set; }
    public float DayOffFood { get; set; }
    public float Equipment { get; set; }
    public float EquipmentConsuption { get; set; }
    public float DayOffEquipment { get; set; }
    public int FoodPlanned { get; set; }
    public float EquipmentPlanned { get; set; }
    public int WorkersOnStation { get; set; }
    public int AwaitingWorkers { get; set; }
    public int WorkersPlanned { get; set; }
    public float IncomeLastMonth { get; set; }
    public float AmountReadyForLoading { get; set; }
    private float wellbeing;
    
    [SerializeField] DockingPort dockingPort;
    [SerializeField] Rigidbody rigidbodyStation;
    public DockingPort DockingPort { get { return dockingPort; } }
    
    void Start()
    {
        FoodConsuption = 0.5f;
        EquipmentConsuption = 0.1f;
    }

    public void CalculateMonth()
    {
        Food -= WorkersOnStation * FoodConsuption;
        Equipment -= WorkersOnStation * EquipmentConsuption;
        if (Food + Equipment < 2)
            wellbeing = (Food + Equipment) / 2;
        else
            wellbeing = 1;
        IncomeLastMonth = 100 * WorkersOnStation * Asteroid.ElementAbundance * wellbeing;
        IncomeLastMonth += IncomeLastMonth * mainClass.Sciense.BetterDigFromSciense / 100;
        ExcavatedSoil += IncomeLastMonth;
        //Asteroid.ElementCapacity -= IncomeLastMonth;
        CalculateSupplyConsuption();
        Asteroid.AsterPanel.UpdateText();
    }

    public void CalculateSupplyConsuption()
    {
        if (WorkersOnStation > 0)
        {
            DayOffFood = (float)Math.Round(Food / (float)(WorkersOnStation * 0.5), 1);
            DayOffEquipment = (float)Math.Round(Equipment / (float)(WorkersOnStation * 0.1), 1);
        }
        if (WorkersPlanned > 0)
        {
            FoodPlanned = 365 * (int)Math.Round((float)(WorkersPlanned * 0.5), 0) - (int)Food;
            EquipmentPlanned = 365 * (float)Math.Round((float)(WorkersPlanned * 0.1), 2) - Equipment;
        }
    }

    public void ReadyForLoad()
    {
        AmountReadyForLoading = ExcavatedSoil;
    }

    private void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler((new Vector3(0, 80, 0)) * Time.fixedDeltaTime);
        rigidbodyStation.MoveRotation(rigidbodyStation.rotation * deltaRotation);
    }
}
