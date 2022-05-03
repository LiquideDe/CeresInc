using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class CargoShip : Ship
{
    [SerializeField] private GameObject routeCargoPanel;
    [SerializeField] private CanvasCargoShip canvasCargoShip;
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

    public override void OpenDestinationPanel()
    {
        CanvasShip.SetActive(true);
        routeCargoPanel.SetActive(true);
    }

    public override void CloseDestinationPanel()
    {
        CanvasShip.SetActive(false);
        routeCargoPanel.SetActive(false);
    }

    public override void ChooseDestination(AsteroidForPlayer aster)
    {
        
        if (!Navigator.IsInJourney && aster.MiningStation != null)
        {
            //¬ыбираем астероид до которого летим и добавл€ем его в массив. ≈сли массив пустой, то просто добавл€ем цель в массив целей и строим простой отрезок
            if (Navigator.OldDestinationsCount() == 0)
            {
                Navigator.SetOldDestination(aster.MiningStation);
                canvasCargoShip.Create_Button(Navigator.OldDestinationsCount()-1);
            }
            //провер€ем, что нова€ точка которую хотим добавить не така€ же как в массиве, если така€ же, то наоборот удал€ем ее из массива, если друга€. то добавл€ем
            //Ћинию перестраиваем в любом случае
            else if (ContainMas(aster) == true)
            {
                canvasCargoShip.DeleteResButton(Navigator.GetIdInListDestination(aster.MiningStation));
                Navigator.RemoveOldDestination(aster.MiningStation);
            }
            else
            {
                Navigator.SetOldDestination(aster.MiningStation);
                canvasCargoShip.Create_Button(Navigator.OldDestinationsCount() - 1);
            }
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

    public override void UpdateText()
    {
        Debug.Log($" оличество точек {Navigator.OldDestinationsCount()}");
        if (Navigator.OldDestinationsCount() > 0)
        {
            shipButton.UpdateText((int)Navigator.DvToOperation, WeightContruction + WeightFuel + WeightPlayload, CostOfJourney, Navigator.GetOldDestination(0).Asteroid.AsterName, Navigator.TimeToJourney, Age);
        }
        else
        {
            shipButton.UpdateText((int)Navigator.DvToOperation, WeightContruction + WeightFuel + WeightPlayload, CostOfJourney, "", Navigator.TimeToJourney, Age);
        }
        canvasCargoShip.UpdateText();

    }

    public override void Docking(MiningStation station)
    {
        cargoElement[station.Asteroid.Element.Id] += station.AmountReadyForLoading;
        Debug.Log($"«агружаем груз в размере {station.AmountReadyForLoading}");
        station.AmountReadyForLoading = 0;
        shipButton.Docking();
    }
    public override void DockingAtCeresStation()
    {
        shipButton.StandAtCeres();
        panelShip.UpdateText(Id);
        if (Navigator.Repeat)
        {
            StartAllowed();
            Navigator.IsStartAllowed = true;
        }
    }

    public float GetCargoElement(int id)
    {
        return cargoElement[id];
    }

    public void SetCargoElement(int id, float mas)
    {
        cargoElement[id] = mas;
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
            Debug.Log($"ѕристыковались к складу, выгружаем элемент в количестве {cargoElement[i]}");
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
}
