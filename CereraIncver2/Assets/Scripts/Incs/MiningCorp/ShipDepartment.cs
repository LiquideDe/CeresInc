using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipDepartment
{
    public MiningCorporate Corporate { get; set; }
    public bool IsConstructShip { get; set; }
    private List<ShipForSimulation> ships = new List<ShipForSimulation>();
    private List<Route> routes = new List<Route>();

    public ShipDepartment(MiningCorporate corporate)
    {
        Corporate = corporate;
    }
    public void CreateShip(int type, int typeCarcass, int typeFuelTank, int typeEngine)
    {
        ships.Add(new ShipForSimulation(Corporate.MainClass, Corporate));
        int id = ships.Count - 1;
        ships[id].ISP = Corporate.ScienseDepartment.GetEngine(typeEngine).Isp;
        ships[id].MaxWeightPlayload = Corporate.ScienseDepartment.GetCarcass(typeCarcass).MaxWeightPlayload;
        ships[id].ShipName = "";
        ships[id].TypeFuel = Corporate.ScienseDepartment.GetEngine(typeEngine).TypeFuel;
        ships[id].TypeShip = type;
        ships[id].Weight = Corporate.ScienseDepartment.GetCarcass(typeCarcass).Weight + Corporate.ScienseDepartment.GetFuelTank(typeFuelTank).Weight + Corporate.ScienseDepartment.GetEngine(typeEngine).Weight;
        ships[id].WeightFuel = Corporate.ScienseDepartment.GetFuelTank(typeFuelTank).MaxFuel;
        ships[id].Id = id;
        
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
        Debug.Log($"Создали новый корабль, дали ему маршрут");
        SetRouteToShip(ships[id]);        
    }

    private void SetRouteToShip(ShipForSimulation ship)
    {
        Route route = null;
        for(int i = 0; i < routes.Count; i++)
        {
            if(routes[i].AmountShipsOnRoute < 2)
            {
                route = routes[i];
                break;
            }
        }
        if(route != null)
        {
            for (int i = 1; i < routes.Count; i++)
            {
                if (route.CalculateTotalLengthRoute() > routes[i].CalculateTotalLengthRoute() && routes[i].AmountShipsOnRoute < 2)
                {
                    route = routes[i];
                }
            }
            if (route.AmountShipsOnRoute < 2)
            {
                ship.SetRoute(route);
                route.AmountShipsOnRoute += 1;
                Debug.Log($"Маршрут {route.Id}");
            }
            else
            {
                Debug.Log($"никакой, routes.Count {routes.Count}, ships.Count {ships.Count}");
            }
        }        

           
    }

    public void ShipWorking()
    {
        for(int i = 0; i < ships.Count; i++)
        {
            ships[i].AtWork();
        }
        if(routes.Count * 2 > ships.Count && !IsConstructShip && !Corporate.IsNeedComponentsForNewShips)
        {
            IsConstructShip = true;
            Corporate.BuildNewShip(0);
            Corporate.BuildNewShip(1);
            IsConstructShip = false;
        }
    }

    public int CountShips()
    {
        return ships.Count;
    }

    public void CreateRoutes()
    {
        List<AsteroidForSimulation> asteroids = new List<AsteroidForSimulation>();
        asteroids.AddRange(Corporate.MiningDepartment.GetAllAsteroids());        
        for(int i = 0; i < asteroids.Count; i++)
        {
            if (!asteroids[i].IsInRoute)
            {
                routes.Add(new Route(Corporate.MainClass));
                routes[routes.Count - 1].SetDestination(asteroids[i]);
                routes[routes.Count - 1].NameRoute = $"{i}";
                asteroids[i].IsInRoute = true;
                FindNeighbor(asteroids[i], asteroids, routes[routes.Count - 1]);                
            }
        }
        SetIdToRoutes();
        Debug.Log($"Создали {routes.Count} маршрутов");
    }

    private void FindNeighbor(AsteroidForSimulation asteroid, List<AsteroidForSimulation> asteroids, Route route)
    {
        float dist;
        for (int i = 0; i < asteroids.Count; i++)
        {
            dist = Vector3.Distance(asteroid.Position, asteroids[i].Position);
            if (dist < 800 && !asteroids[i].IsInRoute)
            {
                route.SetDestination(asteroids[i]);
                asteroids[i].IsInRoute = true;
                FindNeighbor(asteroids[i], asteroids, route);
                break;
            }
        }
    }

    private void SetIdToRoutes()
    {
        for(int i = 0; i < routes.Count; i++)
        {
            routes[i].Id = i;
        }
    }

    public ShipForSimulation GetShip(int id)
    {
        return ships[id];
    }

    public void CreateEmptyShip(SaveLoadShipSim save)
    {
        ships.Add(new ShipForSimulation(Corporate.MainClass, Corporate));
        ships[ships.Count - 1].LoadData(save);
        Debug.Log($"У корпорации {Corporate.CorpName} теперь {ships.Count} кораблей, а если через shipdepartment, то {Corporate.ShipDepartment.CountShips()}");
    }

    public Route GetRoute(int id)
    {
        return routes[id];
    }

    public int CountRoutes()
    {
        return routes.Count;
    }

    public void CreateEmptyRoute(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            routes.Add(new Route(Corporate.MainClass));
        }        
    }
}
