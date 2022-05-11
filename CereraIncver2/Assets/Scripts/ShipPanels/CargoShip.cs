using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CargoShip : Ship
{
    private float[] cargoElement = new float[22];

    void CalculateWeightPlayLoad()
    {
        WeightPlayload = 0;
        for (int i = 0; i < Navigator.OldDestinationsCount(); i++)
        {
            WeightPlayload += Navigator.GetOldDestination(i).AmountReadyForLoading;
        }
    }

    public override float CalculateAllMass()
    {
        CalculateWeightPlayLoad();
        return WeightContruction + WeightPlayload;
    }

    public override void Docking(MiningStation station)
    {
        cargoElement[station.Asteroid.Element.Id] += station.AmountReadyForLoading;
        Debug.Log($"Загружаем груз в размере {station.AmountReadyForLoading}");
        station.AmountReadyForLoading = 0;
        station.ExcavatedSoil = 0;
        shipButton.Docking();
    }
    public override void DockingAtCeresStation()
    {
        shipButton.StandAtCeres();
        panelShip.UpdateText(Id);
        Navigator.StartBreakingDay = mainClass.CeresTime;

    }

    public float GetCargoElement(int id)
    {
        return cargoElement[id];
    }

    public override bool ToWarehouseOrNot(int type)
    {
        if (type == 0)
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
        for (int i= 0; i < cargoElement.Length; i++)
        {
            mainClass.Warehouse.PlusRes(i, cargoElement[i]);
            cargoElement[i] = 0;
        }
    }

    protected override void SaveCargo(SaveLoadShip save)
    {
        for (int j = 0; j < cargoElement.Length; j++)
        {
            save.cargoElement[j] = cargoElement[j];
        }
    }

    protected override void LoadCargo(SaveLoadShip save)
    {
        for (int j = 0; j < cargoElement.Length; j++)
        {
            cargoElement[j] = save.cargoElement[j];
        }
    }

    public override void DeterminingRequiredCargo(List<MiningStation> stations)
    {
        for(int i = 0; i < stations.Count; i++)
        {
            stations[i].ReadyForLoad();
        }
    }
}
