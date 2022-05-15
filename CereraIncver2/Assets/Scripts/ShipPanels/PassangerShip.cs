using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PassangerShip : Ship
{
    public int CargoWorkers { get; set; }
    public float CargoFood { get; set; }
    public float CargoEquipment { get; set; }
    private int demandWorkers;
    private float demandEquip, demandFood;
    public int DemandWorkers { get { return demandWorkers; } set { if(value>0) demandWorkers = value; } }
    public float DemandEquip { get { return demandEquip; } set { if(value>0) demandEquip = value; } }
    public float DemandFood { get { return demandFood; } set { if(value>0) demandFood = value; } }

    public override float CalculateAllMass()
    {
        CalculateWeightPlayLoad();
        return WeightContruction + WeightPlayload;
    }

    public void CalculateWeightPlayLoad()
    {
        WeightPlayload = DemandWorkers * 80 + DemandFood + DemandEquip * 10;        
    }

    public override void Docking(MiningStation station)
    {        
       
        if (station.WorkersPlanned - station.WorkersOnStation > 0 && CargoWorkers >= station.WorkersPlanned - station.WorkersOnStation)
        {
            CargoWorkers -= station.WorkersPlanned - station.WorkersOnStation;
            station.WorkersOnStation += station.WorkersPlanned - station.WorkersOnStation;
        }            
        else if (station.WorkersPlanned - station.WorkersOnStation > 0 && CargoWorkers < station.WorkersPlanned - station.WorkersOnStation)
            {
            station.WorkersOnStation += CargoWorkers;
                CargoWorkers = 0;
            }
        else if(station.AwaitingWorkers > 0)
        {
            CargoWorkers += station.AwaitingWorkers;
            station.WorkersOnStation -= station.AwaitingWorkers;
            station.AwaitingWorkers = 0;
        }
        
        if (CargoEquipment >= station.EquipmentPlanned)
            {
            station.Equipment += station.EquipmentPlanned;
            CargoEquipment -= (int)station.EquipmentPlanned;
            station.EquipmentPlanned = 0;
            }            
        else
            {
            station.Equipment += CargoEquipment;
            station.EquipmentPlanned -= CargoEquipment;
            CargoEquipment = 0;
            }       
        
        if(CargoFood >= station.FoodPlanned)
            {
            station.Food += station.FoodPlanned;
            CargoFood -= station.FoodPlanned;
            station.FoodPlanned = 0;
            }
        else
            {
            station.Food += CargoFood;
            station.FoodPlanned -= (int)CargoFood;
            CargoFood = 0;
            }
            shipButton.Docking();
            CalculateWeightPlayLoad();

        
    }

    public override void DockingAtCeresStation()
    {
        shipButton.StandAtCeres();
        panelShip.UpdateText(Id);
        Navigator.StartBreakingDay = mainClass.CeresTime;
        Debug.Log($"Пристыковались {Navigator.StartBreakingDay}");
        if(CargoWorkers > 0)
        {
            mainClass.Ceres.FreeWorkers += CargoWorkers;
            CargoWorkers = 0;
        }        
    }

    private void LoadingCargos()
    {
        CargoWorkers = demandWorkers;
        CargoEquipment = demandEquip;
        CargoFood = demandFood;
        mainClass.Ceres.WorkersAwaiting -= CargoWorkers;
        mainClass.Player.Equipment -= CargoEquipment;
        mainClass.Player.Food -= CargoFood;
    }

    public override bool ToWarehouseOrNot(int beginOrEnd)
    {
        if(beginOrEnd == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void DockingAtWarehouse()
    {
        LoadingCargos();
    }

    protected override void SaveCargo(SaveLoadShip save)
    {
        save.cargoWorkers = CargoWorkers;
        save.demandWorkers = DemandWorkers;
        save.cargoFood = CargoFood;
        save.cargoEquipment = CargoEquipment;
        save.demandEquip = DemandEquip;
        save.demandFood = DemandFood;
    }

    protected override void LoadCargo(SaveLoadShip save)
    {
        CargoWorkers = save.cargoWorkers;
        DemandWorkers = save.demandWorkers;
        CargoFood = save.cargoFood;
        CargoEquipment = save.cargoEquipment;
        DemandEquip = save.demandEquip;
        DemandFood = save.demandFood;
    }

    public override void DeterminingRequiredCargo(List<MiningStation> stations)
    { 
        demandWorkers = 0;
        demandEquip = 0;
        demandFood = 0;

        for (int i = 0; i < stations.Count; i++)
        {
            demandWorkers += stations[i].WorkersPlanned - stations[i].WorkersOnStation;
            demandEquip += stations[i].EquipmentPlanned;
            demandFood += stations[i].FoodPlanned;
        }
        Debug.Log($"demandWorkers = {demandWorkers}");
    }
}
