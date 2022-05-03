using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipDepartment
{
    public MiningCorporate Corporate { get; set; }
    List<ShipForSimulation> ships = new List<ShipForSimulation>();
    private bool isShipPlanned;

    public ShipDepartment(MiningCorporate corporate)
    {
        Corporate = corporate;
    }
    public void CreateShip(int type, int typeCarcass, int typeFuelTank, int typeEngine)
    {
        ships.Add(new ShipForSimulation());
        int id = ships.Count - 1;
        ships[id].Corporate = Corporate;
        ships[id].ISP = Corporate.ScienseDepartment.GetEngine(typeEngine).Isp;
        ships[id].MainClass = Corporate.MainClass;
        ships[id].MaxWeightPlayload = Corporate.ScienseDepartment.GetCarcass(typeCarcass).MaxWeightPlayload;
        ships[id].ShipName = "";
        ships[id].TypeFuel = Corporate.ScienseDepartment.GetEngine(typeEngine).TypeFuel;
        ships[id].TypeShip = type;
        ships[id].Weight = Corporate.ScienseDepartment.GetCarcass(typeCarcass).Weight + Corporate.ScienseDepartment.GetFuelTank(typeFuelTank).Weight + Corporate.ScienseDepartment.GetEngine(typeEngine).Weight;
        ships[id].WeightFuel = Corporate.ScienseDepartment.GetFuelTank(typeFuelTank).MaxFuel;
        ships[id].Id = id;
        ships[id].MainClass = Corporate.MainClass;

        
        Corporate.PlusEngine(typeEngine, -1);
        Corporate.PlusFuelTank(typeFuelTank, -1);
        if(type == 0)
        {
            Corporate.PlusCarcasPas(typeCarcass, -1);
        }
        else
        {
            Corporate.PlusCarcasCargo(typeCarcass, -1);
        }

    }

    public void ShipWorking()
    {
        for(int i = 0; i < ships.Count; i++)
        {
            if (ships[i].IsInJourney)
            {
                ships[i].AtWork();
            }
            else if(!ships[i].IsInJourney && !isShipPlanned)
            {
                CreateRoute(ships[i]);
                isShipPlanned = true;
            }
            
        }
    }

    public int CountShips()
    {
        return ships.Count;
    }

    public void CreateRoute(ShipForSimulation ship)
    {
        
        if(ship.TypeShip == 0)
        {
            CreatePasRoute(ship);
        }
        else
        {
            CreateCargoRoute(ship);
        }
        
    }
    
    private void CreatePasRoute(ShipForSimulation ship)
    {
        float dv = (float)(ship.ISP * Math.Log((ship.Weight + ship.WeightFuel) / ship.Weight));
        for (int i = 0; i < Corporate.MiningDepartment.CountAsteroids(); i++)
        {
            if(Corporate.MiningDepartment.GetAsteroid(i).WorkersOnStation != Corporate.MiningDepartment.GetAsteroid(i).WorkersPlanned)
            {
                if(ship.CountDestination() < 3)
                {
                    ship.AddDestination(Corporate.MiningDepartment.GetAsteroid(i));
                }
                    
            }
        }
        ship.GoToJourney();
    }

    private void CreateCargoRoute(ShipForSimulation ship)
    {
        float cost = ship.CostJourney();
        float weight = 0;

        for(int i=0;i< Corporate.MiningDepartment.CountAsteroids(); i++)
        {
            weight += Corporate.MiningDepartment.GetAsteroid(i).ExcavatedSoil;
        }
        if(weight * Corporate.OrientRes.Price > cost)
        {
            
            for (int i = 0; i < Corporate.MiningDepartment.CountAsteroids(); i++)
            {
                if(Corporate.MiningDepartment.GetAsteroid(i).ExcavatedSoil > 0)
                {
                    ship.AddDestination(Corporate.MiningDepartment.GetAsteroid(i));
                }                
            }
        }
        ship.GoToJourney();
    }

    public void NewRound()
    {
        isShipPlanned = false;
    }

    public ShipForSimulation GetShip(int id)
    {
        return ships[id];
    }

    public void CreateEmptyShip(SaveLoadShipSim save)
    {
        ships.Add(new ShipForSimulation());
        ships[ships.Count - 1].MainClass = Corporate.MainClass;
        ships[ships.Count - 1].LoadData(save);
    }
}
