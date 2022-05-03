using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public abstract class Ship : MonoBehaviour
{
    public float WeightContruction { get; set; } 
    public float MaxWeightFuel { get; set; } 
    public float WeightFuel { get; set; }        
    public float MaxWeightPlayload { get; set; } 
    public float WeightPlayload { get; set; }    
    public int Age { get; set; }    
    public int Isp { get; set; }
    public int TypeFuel { get; set; }
    //0 - пассажирский, 1 - грузовой
    public int TypeShip { get; set; }    
    public string ShipName { get; set; }
    
    protected bool isMoneyEnough;
    public Inc OwnedInc { get; set; }
    
    [SerializeField] private LineRenderer line;
    [SerializeField] private GameObject canvasShip;
    [SerializeField] protected PanelShip panelShip;
    [SerializeField] protected Steersman steersman;
    [SerializeField] private Navigator navigator;
    [SerializeField] private Transform shipsDock;
    public Navigator Navigator { get { return navigator; } }
    public PanelShip PanelShip { get { return panelShip; } }
    public Steersman Steersman { get { return steersman; } }
    public float CostOfJourney { get; set; }
    public DockingPort Dock { get; set; }
    public Transform ShipsDock { get { return shipsDock; } }

    public GameObject CanvasShip
    {
        get { return canvasShip; }
    }

    [SerializeField] private Transform shipModel;
    public Transform ShipModel
    {
        get { return shipModel; }
    }
    public main mainClass;
    [SerializeField] protected ShipEngine engines;
    [SerializeField] private Rigidbody rigidb;
    public Rigidbody Rigidb
    {
        get { return rigidb; }
    }
    public ShipEngine Engines { get { return engines; } }
    
    protected float dockingTime;
    public int Id { get; set; }
    protected ShipButton shipButton;
    public ShipButton ShipButton { get { return shipButton; } set { shipButton = value; } }

    public void NewShip(string shipName, int typeShip,Carcass carcass, Engine engine, FuelTank fuelTank, Inc inc)
    {
        WeightContruction = carcass.Weight + engine.Weight + fuelTank.Weight;
        MaxWeightFuel = fuelTank.MaxFuel;
        MaxWeightPlayload = carcass.MaxWeightPlayload;
        Isp = engine.Isp;
        TypeFuel = engine.TypeFuel;
        ShipName = shipName;
        TypeShip = typeShip;
        WeightFuel = fuelTank.MaxFuel;
        OwnedInc = inc;
        CalculatedV();
    }

    public abstract void OpenDestinationPanel();
    public abstract void CloseDestinationPanel();
    public abstract void ChooseDestination(AsteroidForPlayer aster);
    public void CalculatedV()
    {
        Navigator.DV = (float)Math.Round(Isp * Math.Log((CalculateAllMass() + WeightFuel) / CalculateAllMass()), 0);
        //смотрим количество пунктов и высчитываем сколько dV мы можем использовать на каждый пункт. Пока что поровну считаем.
        Navigator.DvToOperation = Navigator.DV / ((Navigator.OldDestinationsCount() + 1) * 2);
        Rigidb.mass = WeightContruction + WeightPlayload + WeightFuel;
        //CalcCostJourney();
    }
    public abstract float CalculateAllMass();
    public void DrawLines()
    {
        line.positionCount = 0;
        if (Navigator.OldDestinationsCount() <= 1)
        {
            line.positionCount = Navigator.OldDestinationsCount() + 1;
        }
        else
        {
            line.positionCount = Navigator.OldDestinationsCount() + 2;
        }
        line.SetPosition(0, new Vector3(0, -172, 0));
        for (int i = 0; i < Navigator.OldDestinationsCount(); i++)
        {
            line.SetPosition(i, Navigator.GetOldDestination(i).transform.position);   
        }
        if (Navigator.OldDestinationsCount() > 1)
        {
            line.loop = true;
        }        
        
    }

    public abstract void UpdateText();
    protected bool ContainMas(AsteroidForPlayer aster)
    {
        //Содержится ли астероид в массиве
        bool contain = false;
        for(int i=0;i<Navigator.OldDestinationsCount();i++)
        {
            if (Navigator.GetOldDestination(i) == aster.MiningStation)
            {
                contain = true;
                break;
            }
        }
        return contain;
    }

    protected void Update()
    {
        if (!mainClass.IsPaused && Navigator.IsInJourney)
            Navigator.CalcTimeToJourney();
        if (shipButton.timeToReturn.text != $"Вернется через {(int)Navigator.TimeToJourney} дней")
        {
            shipButton.timeToReturn.text = $"Вернется через {(int)Navigator.TimeToJourney} дней";
        }            
    }

    public abstract void Docking(MiningStation station);
    public abstract void DockingAtWarehouse();
    public abstract void DockingAtCeresStation();
    public abstract bool ToWarehouseOrNot(int BeginOrEnd);

    public bool StartAllowed()
    {
        isMoneyEnough = CalcCostJourney();
        if (!Navigator.IsStartAllowed && isMoneyEnough && Navigator.OldDestinationsCount() > 0)
        {
            Navigator.StartAllowed();
            OwnedInc.Money -= CostOfJourney;
            mainClass.UpdateText();
        }            

        return Navigator.IsStartAllowed;
    }

    public void StartNotAllowed()
    {
        Navigator.IsStartAllowed = false;
    }

    protected bool CalcCostJourney()
    {
        bool money = false;
        CostOfJourney = MaxWeightFuel * mainClass.Materials.GetMaterial(TypeFuel).Price * 0.01f;
        if(OwnedInc.Money > CostOfJourney)
        {
            money = true;
        }
        return money;
    }

    public void SaveData(SaveLoadShip save)
    {
        save.id = Id;
        save.weightContruction = WeightContruction;
        save.maxWeightFuel = MaxWeightFuel;
        save.weightFuel = WeightFuel;
        save.maxWeightPlayload = MaxWeightPlayload;
        save.weightPlayload = WeightPlayload;
        save.isp = Isp;
        save.typeFuel = TypeFuel;
        save.typeShip = TypeShip;
        save.age = Age;
        save.timeToJourney = Navigator.TimeToJourney;
        save.dV = Navigator.DV;
        save.distance = Navigator.Distance;
        save.distanceToNear = Navigator.DistanceToNear;
        save.dvToOperation = Navigator.DvToOperation;
        save.isInJourney = Navigator.IsInJourney;
        save.isDocked = Navigator.IsDocked;
        save.isTimeForDockingEnd = Navigator.IsTimeForDockingEnd;
        save.lastDestination = Navigator.LastDestination;
        save.repeat = Navigator.Repeat;
        save.isStartAllowed = Navigator.IsStartAllowed;
        save.isToWarehouse = Navigator.IsToWarehouse;
        save.isDockedToWarehouse = Navigator.IsDockedToWarehouse;
        save.daysForDocking = Navigator.DaysForDocking;
        save.shipName = ShipName;
        save.distanceForAcc = Steersman.DistanceForAcc;
        for (int j = 0; j < Navigator.DestinationsCount(); j++)
        {
            save.idDestinations.Add(Navigator.GetDestination(j).Asteroid.Id);
        }

        for (int j = 0; j < Navigator.OldDestinationsCount(); j++)
        {
            save.idOldDestinations.Add(Navigator.GetOldDestination(j).Asteroid.Id);
        }

        save.rotated = Steersman.Rotated;
        save.towards = Steersman.Towards;
        save.rotatedLikeDock = Steersman.RotatedLikeDock;

        save.posX = transform.position.x;
        save.posY = transform.position.y;
        save.posZ = transform.position.z;
        save.quatW = transform.rotation.w;
        save.quatX = transform.rotation.x;
        save.quatY = transform.rotation.y;
        save.quatZ = transform.rotation.z;

        SaveCargo(save);
    }

    protected abstract void SaveCargo(SaveLoadShip save);

    public void LoadData(SaveLoadShip save)
    {
        Id = save.id;
        WeightContruction = save.weightContruction;
        MaxWeightFuel = save.maxWeightFuel;
        WeightFuel = save.weightFuel;
        MaxWeightPlayload = save.maxWeightPlayload;
        WeightPlayload = save.weightPlayload;
        Isp = save.isp;
        TypeFuel = save.typeFuel;
        TypeShip = save.typeShip;
        Age = save.age;
        Navigator.TimeToJourney = save.timeToJourney;
        Navigator.DV = save.dV;
        Navigator.Distance = save.distance;
        Navigator.DistanceToNear = save.distanceToNear;
        Navigator.DvToOperation = save.dvToOperation;
        Navigator.IsInJourney = save.isInJourney;
        Navigator.IsDocked = save.isDocked;
        Navigator.IsTimeForDockingEnd = save.isTimeForDockingEnd;
        Navigator.LastDestination = save.lastDestination;
        Navigator.Repeat = save.repeat;
        Navigator.IsStartAllowed = save.isStartAllowed;
        Navigator.IsToWarehouse = save.isToWarehouse;
        Navigator.IsDockedToWarehouse = save.isDockedToWarehouse;
        Navigator.DaysForDocking = save.daysForDocking;
        ShipName = save.shipName;
        Steersman.DistanceForAcc = save.distanceForAcc;

        for (int j = 0; j < save.idDestinations.Count; j++)
        {
            Navigator.SetDestination(mainClass.Asteroids.GetAsteroid(save.idDestinations[j]).MiningStation);
        }

        for (int j = 0; j < save.idOldDestinations.Count; j++)
        {
            Navigator.SetOldDestination(mainClass.Asteroids.GetAsteroid(save.idOldDestinations[j]).MiningStation);
        }

        Steersman.Rotated = save.rotated;
        Steersman.Towards = save.towards;
        Steersman.RotatedLikeDock = save.rotatedLikeDock;

        transform.position = new Vector3(save.posX, save.posY, save.posZ);
        transform.rotation = new Quaternion(save.quatX, save.quatY, save.quatZ, save.quatW);

        LoadCargo(save);
    }

    protected abstract void LoadCargo(SaveLoadShip save);
}
