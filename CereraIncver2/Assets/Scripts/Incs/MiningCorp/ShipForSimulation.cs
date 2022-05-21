using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipForSimulation
{
    public main MainClass { get; set; }
    public MiningCorporate Corporate {get;set;}
    public float Food{get;set;}
    public float Equipment { get; set; }
    public float DV { get; set; }
    public int Workers { get; set; }
    List<AsteroidForSimulation> destinations = new List<AsteroidForSimulation>();
    List<float> distances = new List<float>();
    public float DaysForTrip { get; set; }
    public float StartDay { get; set; }
    public float Cargo { get; set; }
    public float Weight { get; set; }
    public float ISP { get; set; }
    public int TypeFuel { get; set; }
    public float WeightFuel { get; set; }
    public float MaxWeightPlayload { get; set; }
    public float WeightPlayload { get; set; }
    public int TypeShip { get; set; }
    public string ShipName { get; set; }
    public float Distance { get; set; }
    public float DvToOperation { get; set; }
    public float CostOfJourney { get; set; }
    public bool IsInJourney { get; set; }
    public bool IsLastDestination { get; set; }
    public int Id { get; set; }
    public Route Route { get; set; }
    public float StartBreakingDay { get; set; }
    public float BreakBetweenJourneys { get; set; }
    public bool IsDocking { get; set; }

    public ShipForSimulation(main main, MiningCorporate corporate)
    {
        MainClass = main;
        Corporate = corporate;
    }

    public void AtWork()
    {
        if (IsInJourney)
        {
            if (!IsLastDestination)
            {
                if (StartDay + ((distances[0] / (DvToOperation * 86.4f / 1000)) + destinations.Count * 5) < MainClass.CeresTime && !IsDocking)
                {
                    IsDocking = true;
                    if (TypeShip == 0)
                    {
                        Debug.Log($"Корабль {TypeShip} на маршруте {Route.Id}, Пристыковываемся, IsDocking {IsDocking}");
                        for(int i = 0; i < destinations.Count; i++)
                        {
                            Debug.Log($"Пункт {i} - {destinations[i].AsterName}");
                        }
                        DockingPassanger();
                    }
                    else
                    {
                        DockingCargo();
                    }
                    IsDocking = false;
                }
            }
            else
            {
                if (StartDay + DaysForTrip < MainClass.CeresTime)
                {
                    IsInJourney = false;
                    Debug.Log($"Корабль {TypeShip} на маршруте {Route.Id}, прибыл на базу");
                    Corporate.AmountResource += Cargo;
                    Cargo = 0;
                    StartBreakingDay = MainClass.CeresTime;
                    UpdateDestinations();
                }
            }
        }
        else if(StartBreakingDay + BreakBetweenJourneys < MainClass.CeresTime)
        {
            GoToJourney();
        }
    }

    private void GoToJourney()
    {
        Debug.Log($"Корабль {TypeShip} на маршруте {Route.Id}, Делаем предварительные просчеты по поездке");
        
        if (PreCalculation())
        {
            StartDay = MainClass.CeresTime;
            IsInJourney = true;
            Debug.Log($"Корабль {TypeShip} на маршруте {Route.Id}, ПОЕХАЛИ, стартовое время {StartDay}");
        }

    }

    private bool PreCalculation()
    {
        CalcCostJourney();
        if (LoadCargo() && CalculatedV())
        {
            CalculateDistance();
            CalculateTime();
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool LoadCargo()
    {
        if (TypeShip == 0)
        {
            for (int i = 0; i < destinations.Count; i++)
            {

                if (Corporate.FreeWorkers >= destinations[i].WorkersPlanned - destinations[i].WorkersOnStation && destinations[i].WorkersPlanned - destinations[i].WorkersOnStation > 0)
                {
                    Workers += destinations[i].WorkersPlanned - destinations[i].WorkersOnStation;
                    Corporate.FreeWorkers -= destinations[i].WorkersPlanned - destinations[i].WorkersOnStation;
                    Corporate.PlannedWorkerForWork -= destinations[i].WorkersPlanned - destinations[i].WorkersOnStation;
                }
                else if (destinations[i].WorkersPlanned - destinations[i].WorkersOnStation > 0)
                {
                    Workers += Corporate.FreeWorkers;
                    Corporate.FreeWorkers = 0;
                    Corporate.PlannedWorkerForWork = 0;
                }

                if (Corporate.Food >= destinations[i].FoodPlanned)
                {
                    Food += destinations[i].FoodPlanned;
                    Corporate.Food -= destinations[i].FoodPlanned;

                }
                else if (Corporate.Food > 0)
                {
                    Food += Corporate.Food;
                    Corporate.Food = 0;
                }

                if (Corporate.Equipment >= destinations[i].EquipmentPlanned)
                {
                    Equipment += destinations[i].EquipmentPlanned;
                    Corporate.Equipment -= destinations[i].EquipmentPlanned;
                }
                else if (Corporate.Equipment > 0)
                {
                    Equipment += Corporate.Equipment;
                    Corporate.Equipment = 0;
                }
            }
            Debug.Log($"Корабль {TypeShip} на маршруте {Route.Id}, Летим с {Workers} рабочих, с {Food} еды и с {Equipment} экипировки");
            if(Workers < 0 || Food < 0 || Equipment < 0 || (Workers == 0 && Equipment == 0 && Food == 0))
            {
                return false;
            }
        }
        else
        {
            float sum = 0;
            for(int i = 0; i < destinations.Count; i++)
            {
                destinations[i].SetReadyToLoad();
                sum += destinations[i].ReadyToLoad;
            }

            if(sum * Corporate.OrientRes.Price < CostOfJourney)
            {
                StartBreakingDay += 50;
                return false;
            }
        }
        return true;
    }
    private bool CalculatedV()
    {
        DV = (float)Math.Round(ISP * Math.Log((CalculateAllMass() + WeightFuel) / CalculateAllMass()), 0);
        //смотрим количество пунктов и высчитываем сколько dV мы можем использовать на каждый пункт. Пока что поровну считаем.
        DvToOperation = DV / ((destinations.Count + 1) * 2);        
        if(DvToOperation > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void CalculateDistance()
    {
        distances.Clear();
        if (destinations.Count >= 1)
        {
            for (int i = 0; i < destinations.Count; i++)
            {
                if (i == 0)
                {
                    Distance = Vector3.Distance(new Vector3(0, 0, 0), destinations[0].Position);
                    distances.Add(Distance);
                }
                else
                {
                    distances.Add(Vector3.Distance(destinations[i - 1].Position, destinations[i].Position));
                    Distance += distances[i];
                }
            }
            Distance += Vector3.Distance(destinations[destinations.Count - 1].Position, new Vector3(0, 0, 0));
        }
    }
    private void CalculateTime()
    {
        DaysForTrip = ((Distance) / (DvToOperation * 86.4f/1000)) + destinations.Count * 5;
        Debug.Log($"Корабль {TypeShip}, Поездка составит {DaysForTrip} дней");
    }
    private float CalculateAllMass()
    {
        return CalculateWeightElements() + Workers * 80 + Food + Equipment * 10 + Weight;
    }
    private float CalculateWeightElements()
    {
        float answ = 0;
        if(TypeShip == 1)
        {
            for (int i = 0; i < destinations.Count; i++)
            {
                answ += destinations[i].ReadyToLoad;
            }
        }
        return answ;
    }   

    private void CalcCostJourney()
    {
        CostOfJourney = WeightFuel * MainClass.Materials.GetMaterial(TypeFuel).Price * 0.01f;
    }
    private void DockingPassanger()
    {
        if (destinations[0].WorkersPlanned - destinations[0].WorkersOnStation > 0 && Workers >= destinations[0].WorkersPlanned - destinations[0].WorkersOnStation)
        {
            Workers -= destinations[0].WorkersPlanned - destinations[0].WorkersOnStation;
            destinations[0].WorkersOnStation += destinations[0].WorkersPlanned - destinations[0].WorkersOnStation;
        }

        if(Food > destinations[0].FoodPlanned)
        {
            destinations[0].Food += destinations[0].FoodPlanned;
            Food -= destinations[0].FoodPlanned;
        }
        else if(Food > 0)
        {
            destinations[0].Food += Food;
            Food = 0;
        }

        if(Equipment > destinations[0].EquipmentPlanned)
        {
            destinations[0].Equipment += destinations[0].EquipmentPlanned;
            Equipment -= destinations[0].EquipmentPlanned;
        }
        else if(Equipment > 0)
        {
            destinations[0].Equipment += Equipment;
            Equipment = 0;
        }
        
        destinations.RemoveAt(0);
        distances.RemoveAt(0);
        CheckLastDestination();
    }

    private void DockingCargo()
    {
        Cargo += destinations[0].ReadyToLoad;
        destinations[0].ReadyToLoad = 0;
        destinations.RemoveAt(0);
        distances.RemoveAt(0);
        CheckLastDestination();
    }

    private void CheckLastDestination()
    {
        if(distances.Count == 0)
        {
            IsLastDestination = true;
        }
    }    

    public int CountDestination()
    {
        return destinations.Count;
    }

    public AsteroidForSimulation GetDestination(int id)
    {
        return destinations[id];
    }

    public float GetDistance(int id)
    {
        return distances[id];
    }

    public void SetDestination(AsteroidForSimulation asteroid)
    {
        destinations.Add(asteroid);
    }

    public void SetDistances(float dist)
    {
        distances.Add(dist);
    }

    public void SetRoute(Route route)
    {
        Route = route;
        UpdateDestinations();
        CalculatedV();
        BreakBetweenJourneys = (route.CalculateTotalLengthRoute() / (DvToOperation * 86.4f / 1000))/2;
        StartBreakingDay = -BreakBetweenJourneys + (BreakBetweenJourneys/2 * TypeShip);
    }

    private void UpdateDestinations()
    {
        if(destinations.Count != 0)
        {
            destinations.Clear();
        }
        for (int i = 0; i < Route.CountDestination(); i++)
        {
            destinations.Add(MainClass.Asteroids.GetSimAsteroid(Route.GetDestination(i).Id));
        }
    }

    public void SaveData(SaveLoadShipSim save)
    {
        Debug.Log($"Начинаем процесс сохранения");
        save.food = Food;
        save.equipment = Equipment;
        save.dV = DV;
        save.daysForTrip = DaysForTrip;
        save.startDay = StartDay;
        save.cargo = Cargo;
        save.weight = Weight;
        save.iSP = ISP;
        save.weightFuel = WeightFuel;
        save.maxWeightPlayload = MaxWeightPlayload;
        save.weightPlayload = WeightPlayload;
        save.distance = Distance;
        save.dvToOperation = DvToOperation;
        save.costOfJourney = CostOfJourney;
        save.workers = Workers;
        save.typeFuel = TypeFuel;
        save.typeShip = TypeShip;
        save.id = Id;
        save.shipName = ShipName;
        save.isInJourney = IsInJourney;
        save.isLastDestination = IsLastDestination;
        save.routeId = Route.Id;
        save.startBreakingDay = StartBreakingDay;
        save.breakBetweenJourneys = BreakBetweenJourneys;
        save.isDocking = IsDocking;

        Debug.Log($"Количество пунктов {CountDestination()}");
        for (int i = 0; i < distances.Count; i++)
        {
            save.distances.Add(GetDistance(i));
        }
        Debug.Log($"Количество пунктов {CountDestination()}");
        for (int i = 0; i < CountDestination(); i++)
        {
            Debug.Log($"пункт {i}");
            Debug.Log($"id пункта {GetDestination(i).Id}");
            save.idDestinations.Add(GetDestination(i).Id);
            Debug.Log($"добавили");
        }
        Debug.Log($"Из цикла вышли");
    }

    public void LoadData(SaveLoadShipSim save)
    {
        
        Food = save.food;
        Equipment = save.equipment;
        DV = save.dV;
        DaysForTrip = save.daysForTrip;
        StartDay = save.startDay;
        Cargo = save.cargo;
        Weight = save.weight;
        ISP = save.iSP;
        WeightFuel = save.weightFuel;
        MaxWeightPlayload = save.maxWeightPlayload;
        WeightPlayload = save.weightPlayload;
        Distance = save.distance;
        DvToOperation = save.dvToOperation;
        CostOfJourney = save.costOfJourney;
        Workers = save.workers;
        TypeFuel = save.typeFuel;
        TypeShip = save.typeShip;
        Id = save.id;
        ShipName = save.shipName;
        IsInJourney = save.isInJourney;
        IsLastDestination = save.isLastDestination;
        Route = Corporate.ShipDepartment.GetRoute(save.routeId);
        StartBreakingDay = save.startBreakingDay;
        BreakBetweenJourneys = save.breakBetweenJourneys;
        IsDocking = save.isDocking;
        Debug.Log($"Привет, я загружен, меня зовут корабль {ShipName}, я тип {TypeShip}, и принадлежу корпорации {Corporate.CorpName}");
        for (int i = 0; i < save.idDestinations.Count; i++)
        {
            SetDestination(MainClass.Asteroids.GetSimAsteroid(save.idDestinations[i]));
        }

        for(int i = 0; i < save.distances.Count; i++)
        {
            SetDistances(save.distances[i]);
        }
    }
}
