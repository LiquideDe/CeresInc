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

    private void CalculateTime()
    {
        DaysForTrip = ((Distance) / (DvToOperation * 86.4f/1000)) + destinations.Count * 5;
        Debug.Log($"Поездка составит {DaysForTrip} дней");
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

    private void CalculateDestination()
    {
        CalculateDistance();
        CalculatedV();        

        for (int i = 1; i < destinations.Count; i++)
        {
            if(distances[i]/distances[i-1] > 0.3)
            {
                Debug.Log($"Следующая дистанция {distances[i]} больше чем на 30% предыдущего пути {distances[i - 1]} ");
                distances.RemoveAt(i);
                destinations.RemoveAt(i);
                CalculateDestination();
                break;
            }
        }        
    }

    private void CalculatedV()
    {
        DV = (float)Math.Round(ISP * Math.Log((CalculateAllMass() + WeightFuel) / CalculateAllMass()), 0);
        //смотрим количество пунктов и высчитываем сколько dV мы можем использовать на каждый пункт. Пока что поровну считаем.
        DvToOperation = DV / ((destinations.Count + 1) * 2);        
        CalcCostJourney();
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

    private float CalculateAllMass()
    {
        return CalculateWeightElements() + Workers * 80 + Food + Equipment * 10 + Weight;
    }

    private void CalcCostJourney()
    {
        CostOfJourney = WeightFuel * MainClass.Materials.GetMaterial(TypeFuel).Price * 0.01f;
    }

    public float CostJourney()
    {
        CalcCostJourney();
        return CostOfJourney;
    }

    private void LoadCargo()
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
            Debug.Log($"Летим с {Workers} рабочих, с {Food} еды и с {Equipment} экипировки");
        }
    }

    public void AddDestination(AsteroidForSimulation asteroid)
    {
        if (!IsInJourney)
        {
            destinations.Add(asteroid);
        }
        if(TypeShip == 1)
        {
            asteroid.SetReadyToLoad();
        }
    }

    private void PreCalculation()
    {
        CalculateDestination();
        Debug.Log($"Все расчеты выполнены, точек сделано {destinations.Count}");
        if (destinations.Count > 0)
        {
            Debug.Log($"Загружаемся");
            LoadCargo();
            CalculatedV();
            CalculateDistance();
            CalculateTime();
        }        
    }

    private void DockingPassanger()
    {
        if (destinations[0].WorkersPlanned - destinations[0].WorkersOnStation > 0 && Workers >= destinations[0].WorkersPlanned - destinations[0].WorkersOnStation)
        {
            Workers -= destinations[0].WorkersPlanned - destinations[0].WorkersOnStation;
            destinations[0].WorkersOnStation += destinations[0].WorkersPlanned - destinations[0].WorkersOnStation;
        }
        destinations[0].Food += destinations[0].FoodPlanned;
        Food -= destinations[0].FoodPlanned;
        destinations[0].Equipment += destinations[0].EquipmentPlanned;
        Equipment -= destinations[0].EquipmentPlanned;
        destinations[0].FoodPlanned = 0;
        destinations[0].EquipmentPlanned = 0;
        Debug.Log($"Разргрузили рабочих и припасы на астероид {destinations[0].AsterName}, на астероиде теперь работает {destinations[0].WorkersOnStation} человек");
        destinations.RemoveAt(0);
        distances.RemoveAt(0);
        Debug.Log($"Осталось точек {destinations.Count}");
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

    public void GoToJourney()
    {
        Debug.Log($"Делаем предварительные просчеты по поездке");
        PreCalculation();
        if(destinations.Count > 0)
        {
            StartDay = MainClass.CeresTime;
            IsInJourney = true;
            Debug.Log($"ПОЕХАЛИ, стартовое время {StartDay}");
        }       

    }

    public void AtWork()
    {
        if (IsInJourney)
        {
            if (!IsLastDestination)
            {
                if (StartDay + ((distances[0] / (DvToOperation * 86.4f / 1000)) + destinations.Count * 5) < MainClass.CeresTime)
                {
                    if (TypeShip == 0)
                    {
                        DockingPassanger();
                    }
                    else
                    {
                        DockingCargo();
                    }

                }
            }
            else
            {
                if (StartDay + DaysForTrip < MainClass.CeresTime)
                {
                    IsInJourney = false;
                    Corporate.AmountResource += Cargo;
                    Cargo = 0;
                }
            }          
            
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

    public void SaveData(SaveLoadShipSim save)
    {
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

        for (int i = 0; i < CountDestination(); i++)
        {
            save.distances.Add(GetDistance(i));
        }

        for (int i = 0; i < CountDestination(); i++)
        {
            save.idDestinations.Add(GetDestination(i).Id);
        }
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

        for(int i = 0; i < save.idDestinations.Count; i++)
        {
            SetDestination(MainClass.Asteroids.GetSimAsteroid(save.idDestinations[i]));
        }

        for(int i = 0; i < save.distances.Count; i++)
        {
            SetDistances(save.distances[i]);
        }
    }
}
