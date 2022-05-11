using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Navigator : MonoBehaviour
{
    [SerializeField] Ship ship;
    public float TimeToJourney { get; set; }
    public float DV { get; set; }
    public float Distance { get; set; }
    public float DistanceToNear { get; set; }
    public float DvToOperation { get; set; }
    public bool IsInJourney { get; set; }
    public bool IsDocked { get; set; }
    public bool IsTimeForDockingEnd { get; set; }
    public bool LastDestination { get; set; }
    public bool Repeat { get; set; }
    public bool IsStartAllowed { get; set; }
    public bool IsToWarehouse { get; set; }
    public int DaysForDocking { get; set; }
    public bool IsDockedToWarehouse { get; set; }
    public float BreakBetweenJourneys { get; set; }
    public float StartBreakingDay { get; set; }
    public int IdRoute { get; set; }
    private float daysAtDock;
    private List<MiningStation> destinations = new List<MiningStation>();
    private List<MiningStation> oldDestinations = new List<MiningStation>();
    void Start()
    {
        DaysForDocking = 3;        
    }

    public void CalculateTime()
    {
        TimeToJourney = ((Distance * 1000) / (DvToOperation * 86.4f)) + oldDestinations.Count * 34;

        ship.PanelShip.UpdateText(ship.Id);
    }

    public void CalculateDistance()
    {
        if (oldDestinations.Count >= 1)
        {
            for (int i = 0; i < oldDestinations.Count; i++)
            {
                if (i == 0)
                    Distance = Vector3.Distance(ship.Rigidb.position, oldDestinations[0].transform.position);
                else
                {
                    Distance += Vector3.Distance(oldDestinations[i - 1].transform.position, oldDestinations[i].transform.position);
                }
            }
            Distance += Vector3.Distance(oldDestinations[oldDestinations.Count - 1].transform.position, ship.mainClass.Ceres.transform.position);
            DistanceToNear = Vector3.Distance(ship.Rigidb.position, oldDestinations[0].transform.position);
        }
    }

    public int DestinationsCount()
    {
        return destinations.Count;
    }

    public int OldDestinationsCount()
    {
        return oldDestinations.Count;
    }

    public void RemoveOldDestination(MiningStation station)
    {
        oldDestinations.Remove(station);
    }

    public void SetDestination(MiningStation dest)
    {
        destinations.Add(dest);
    }

    public void SetOldDestination(MiningStation dest)
    {
        oldDestinations.Add(dest);
    }

    public int GetIdInListDestination(MiningStation station)
    {
        return oldDestinations.LastIndexOf(station);
    }

    public MiningStation GetDestination(int id)
    {
        return destinations[id];
    }

    public MiningStation GetOldDestination(int id)
    {
        return oldDestinations[id];
    }

    public void CalcTimeToJourney()
    {
        TimeToJourney -= Time.deltaTime * ship.mainClass.TimeSpeed;
    }

    public MiningStation NearDestination()
    {
        return destinations[0];
    }

    public void DockIsSuccessfull(bool isWarehouse)
    {
        IsDocked = true;
        daysAtDock = 0;
        IsTimeForDockingEnd = false;
        
        if (isWarehouse)
        {
            IsDockedToWarehouse = true;            
            ship.DockingAtWarehouse();
            Debug.Log($"Пристыковываемся к складу");
        }
        else if (LastDestination)
        {
            IsStartAllowed = false;
            LastDestination = false;
            ship.DockingAtCeresStation();
            Debug.Log($"Пристыковались к станции, IsStartAllowed {IsStartAllowed}, LastDestination {LastDestination}");
        }
        else
        {
            ship.Docking(destinations[0]);
            Debug.Log($"Пристыковываемся к точке");
        }
        
        if(destinations.Count > 0 && !isWarehouse)
        {
            destinations.RemoveAt(0);
            Debug.Log($"Удалили точку");
        }
        
        if(destinations.Count == 0 && IsStartAllowed)
        {
            if(ship.TypeShip == 0)
            {
                LastDestination = true;
                Debug.Log($"Точки кончились, летим обратно");
            }
            else if(!isWarehouse)
            {
                IsToWarehouse = true;
            }
            else
            {
                LastDestination = true;
            }
                        
        }

        if (isWarehouse)
        {
            IsToWarehouse = false;
        }

    }

    private void FixedUpdate()
    {
        if (!ship.mainClass.IsPaused && IsStartAllowed)
        {
            if (IsDocked && IsTimeForDockingEnd)
            {
                //Debug.Log($"IsDocked {IsDocked}, IsTimeForDockingEnd {IsTimeForDockingEnd}, делаем расстыковку");
                ship.Steersman.UnDock(ship.Dock);
                ship.ShipButton.AtJourney();
            }
            else if(IsToWarehouse && !IsDocked)
            {
                //Debug.Log($"Летим к складу");
                if (ship.Dock == null) { ship.Dock = ship.mainClass.Ceres.GetPathToWarehouse(); }
                ship.Steersman.MoveToDock(ship.Dock, true);

            }
            else if(!LastDestination && !IsDocked)
            {
                //Debug.Log($"IsDocked {IsDocked}, IsTimeForDockingEnd {IsTimeForDockingEnd}, IsToWarehouse {IsToWarehouse}, LastDestination {LastDestination}  ");
                if (ship.Dock == null) { ship.Dock = destinations[0].DockingPort; }
                ship.Steersman.MoveToDock(ship.Dock);
            }
            else if (LastDestination && !IsDocked)
            {
                //Debug.Log($"IsDocked {IsDocked}, IsTimeForDockingEnd {IsTimeForDockingEnd}, IsToWarehouse {IsToWarehouse}, LastDestination {LastDestination}  ");
                ship.Dock = ship.mainClass.Ceres.GetFreeDock();
                ship.Steersman.MoveToDock(ship.Dock);
            }
        }
    }

    private void Update()
    {
        if(IsDocked && IsStartAllowed && !IsTimeForDockingEnd)
        {
            daysAtDock += Time.deltaTime * ship.mainClass.TimeSpeed;
            if(daysAtDock >= DaysForDocking)
            {
                IsTimeForDockingEnd = true;
            }
        }

        if (!IsStartAllowed && Repeat)
        {
            if(StartBreakingDay + BreakBetweenJourneys < ship.mainClass.CeresTime)
            {
                Debug.Log($"Разрешаем старт, StartBreakingDay = {StartBreakingDay}, BreakBetweenJourneys = {BreakBetweenJourneys}");
                ship.StartAllowed();                
            }
        }
    }
     private void ReCreateDestination()
    {
        destinations = new List<MiningStation>(oldDestinations);
    }

    public void StartAllowed()
    {
        ReCreateDestination();
        ship.DeterminingRequiredCargo(oldDestinations);
        IsToWarehouse = ship.ToWarehouseOrNot(ship.TypeShip);
        IsStartAllowed = true;
    }
    
    public void SetRoute(Route route, int daysBreak)
    {
        if (!IsInJourney)
        {
            if(oldDestinations.Count != 0)
            {
                oldDestinations.Clear();
            }
            for (int i = 0; i < route.CountDestination(); i++)
            {
                oldDestinations.Add(ship.mainClass.Asteroids.GetAsteroid(route.GetDestination(i).Id).MiningStation);
            }
            ReCreateDestination();
            route.AmountShipsOnRoute += 1;
            IdRoute = route.Id;
            BreakBetweenJourneys = daysBreak;
            StartBreakingDay = -BreakBetweenJourneys;
            ship.DeterminingRequiredCargo(oldDestinations);
        }        
    }
}
