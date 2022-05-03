using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidForSimulation : Asteroid
{
    public int WorkersPlanned { get; set; }
    public float ExcavatedSoil { get; set; }
    public float ReadyToLoad { get; set; }
    public float Food { get; set; }
    public float FoodConsuption { get; set; }
    public float DayOffFood { get; set; }
    public float Equipment { get; set; }
    public float EquipmentConsuption { get; set; }
    public float DayOffEquipment { get; set; }
    public int FoodPlanned { get; set; }
    public float EquipmentPlanned { get; set; }
    public int WorkersOnStation { get; set; }
    public float IncomeLastMonth { get; set; }
    private float wellbeing;

    public AsteroidForSimulation()
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
        IncomeLastMonth = 100 * WorkersOnStation * ElementAbundance * wellbeing;
        ExcavatedSoil += IncomeLastMonth;
        ElementCapacity -= IncomeLastMonth;
        Element.ExcavatedAtLastMonth += IncomeLastMonth;
        CalculateSupplyConsuption();
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

    public void SetReadyToLoad()
    {
        ReadyToLoad = ExcavatedSoil;
        ExcavatedSoil = 0;
    }

    protected override void SaveAnotherData(SaveLoadAsteroid save)
    {
        if (HasMiningStation)
        {
            save.excavatedSoil = ExcavatedSoil;
            save.food = Food;
            save.equipment = Equipment;
            save.foodPlanned = FoodPlanned;
            save.equipmentPlanned = EquipmentPlanned;
            save.incomeLastMonth = IncomeLastMonth;
            save.amountReadyForLoading = ReadyToLoad;
            save.workersOnStation = WorkersOnStation;
            save.workersPlanned = WorkersPlanned;
        }
    }

    protected override void LoadAnotherData(SaveLoadAsteroid save)
    {
        if (save.hasMiningStation)
        {
            ExcavatedSoil = save.excavatedSoil;
            Food = save.food;
            Equipment = save.equipment;
            FoodPlanned = save.foodPlanned;
            EquipmentPlanned = save.equipmentPlanned;
            IncomeLastMonth = save.incomeLastMonth;
            ReadyToLoad = save.amountReadyForLoading;
            WorkersOnStation = save.workersOnStation;
            WorkersPlanned = save.workersPlanned;
        }
    }
}