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
    public int AwaitingCargoWorkers { get; set; }
    public float AwaitingCargoFood { get; set; }
    public float AwaitingCargoEquipment { get; set; }
    private int demandWorkers;
    private float demandEquip, demandFood;
    public int DemandWorkers { get { return demandWorkers; } set { if(value>0) demandWorkers = value; } }
    public float DemandEquip { get { return demandEquip; } set { if(value>0) demandEquip = value; } }
    public float DemandFood { get { return demandFood; } set { if(value>0) demandFood = value; } }
    
    [SerializeField] private GameObject routePassangerPanel;
    [SerializeField] private CanvasPasShip canvasPasShip;

    public override float CalculateAllMass()
    {
        CalculateWeightPlayLoad();
        return WeightContruction + WeightPlayload;
    }

    public void CalculateWeightPlayLoad()
    {
        WeightPlayload = AwaitingCargoWorkers * 80 + AwaitingCargoFood + AwaitingCargoEquipment * 10;        
    }

    public override void OpenDestinationPanel()
    {
        CanvasShip.SetActive(true);
        routePassangerPanel.SetActive(true);
    }

    public override void CloseDestinationPanel()
    {
        CanvasShip.SetActive(false);
        routePassangerPanel.SetActive(false);
    }

    public override void ChooseDestination(AsteroidForPlayer aster)
    {
        if (!Navigator.IsInJourney && aster.MiningStation != null)
        {
            //Выбираем астероид до которого летим и добавляем его в массив. Если массив пустой, то просто добавляем цель в массив целей и строим простой отрезок
            if (Navigator.OldDestinationsCount() == 0)
            {                
                Navigator.SetOldDestination(aster.MiningStation);                
            }
            //проверяем, что новая точка которую хотим добавить не такая же как в массиве, если такая же, то наоборот удаляем ее из массива, если другая. то добавляем
            //Линию перестраиваем в любом случае
            else if (ContainMas(aster) == true)
            {
                Navigator.RemoveOldDestination(aster.MiningStation);
            }
            else
            {
                Navigator.SetOldDestination(aster.MiningStation); 
            }

            
            CalculateDemands();
            Navigator.CalculateDistance();
            CalculatedV();
            Navigator.CalculateTime();
            if (Navigator.OldDestinationsCount() > 0)
            {
                shipButton.UpdateText((int)Navigator.DvToOperation, WeightContruction + WeightFuel + WeightPlayload, CostOfJourney, Navigator.GetOldDestination(0).Asteroid.AsterName, Navigator.TimeToJourney, Age);
            }
            else
            {
                shipButton.UpdateText((int)Navigator.DvToOperation, WeightContruction + WeightFuel + WeightPlayload, CostOfJourney, "", Navigator.TimeToJourney, Age);
            }



            DrawLines();
            UpdateText();


        }

    }

    void CalculateDemands()
    {
        demandWorkers = 0;
        demandEquip = 0;
        demandFood = 0;

        for (int i = 0; i < Navigator.OldDestinationsCount(); i++)
        {
            demandWorkers += Navigator.GetOldDestination(i).WorkersPlanned;
            demandEquip += (int)Navigator.GetOldDestination(i).EquipmentPlanned;
            demandFood += (int)Navigator.GetOldDestination(i).FoodPlanned;
        }
        
    }

    

    public override void UpdateText()
    {
        canvasPasShip.UpdateText();
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
        if(CargoWorkers > 0)
        {
            mainClass.Ceres.FreeWorkers += CargoWorkers;
            CargoWorkers = 0;
        }
        if (Navigator.Repeat)
        {
            StartAllowed();
            Navigator.IsStartAllowed = true;            
        }
    }

    private void LoadingCargos()
    {
        CargoWorkers = AwaitingCargoWorkers;
        CargoEquipment = AwaitingCargoEquipment;
        CargoFood = AwaitingCargoFood;
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
        save.awaitingCargoWorkers = AwaitingCargoWorkers;
        save.demandWorkers = DemandWorkers;
        save.cargoFood = CargoFood;
        save.cargoEquipment = CargoEquipment;
        save.awaitingCargoFood = AwaitingCargoFood;
        save.awaitingCargoEquipment = AwaitingCargoEquipment;
        save.demandEquip = DemandEquip;
        save.demandFood = DemandFood;
    }

    protected override void LoadCargo(SaveLoadShip save)
    {
        CargoWorkers = save.cargoWorkers;
        AwaitingCargoWorkers = save.awaitingCargoWorkers;
        DemandWorkers = save.demandWorkers;
        CargoFood = save.cargoFood;
        CargoEquipment = save.cargoEquipment;
        AwaitingCargoFood = save.awaitingCargoFood;
        AwaitingCargoEquipment = save.awaitingCargoEquipment;
        DemandEquip = save.demandEquip;
        DemandFood = save.demandFood;
    }
}
