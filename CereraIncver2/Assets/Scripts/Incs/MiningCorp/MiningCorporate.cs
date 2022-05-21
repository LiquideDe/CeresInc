using UnityEngine;
using System.Collections.Generic;


public class MiningCorporate : Corporate, ICorporateShare
{
    public MiningDepartment MiningDepartment { get; set; }
    public ScienseForSimulation ScienseDepartment { get { return sciense; } }
    public ShipDepartment ShipDepartment { get; set; }
    public FinanceDepartment FinanceDepartment { get; set; }
    public int FreeWorkers { get; set; }
    public int PlannedWorkerForWork { get; set; }
    public Resource OrientRes { get; set; }
    public float Food {get;set;}
    public float Equipment { get; set; }
    public float AmountResource { get; set; }
    public float PriceShare { get { return FinanceDepartment.PriceShare; } }
    public float Price { get; set; }
    public int AmountShip { get; set; }
    public int AmountRoutes { get; set; }
    public bool IsRoutesCreate { get; set; }
    public bool IsNeedComponentsForNewShips { get; set; }

    private List<int> carcassPas = new List<int>();
    private List<int> carcassCargo = new List<int>();
    private List<int> engine = new List<int>();
    private List<int> fuelTank = new List<int>();
    [SerializeField] ScienseForSimulation sciense;
    [SerializeField] CorpPanel corpPanel;
    public int ships;
    public int routes;

    public CorpPanel ShareMarket { get { return corpPanel; } }

    private void Start()
    {
        MiningDepartment = new MiningDepartment(this);
        ShipDepartment = new ShipDepartment(this);
        FinanceDepartment = new FinanceDepartment(this);
        Debug.Log($"—оздали новые подразделени€");
    }

    public void StartNewGame()
    {
        Food = 1000;
        Equipment = 1000;
        carcassPas.Add(1);
        carcassCargo.Add(1);
        engine.Add(2);
        fuelTank.Add(2);
        FreeWorkers = 70;
        Money = 10000000;
        ScienseDepartment.StartGame();
        
    }
    public void BuildNewShip(int type)
    {
        if (IsAllParts(type))
        {
            
            ShipDepartment.CreateShip(type, GetFreeDevice(carcassPas), GetFreeDevice(fuelTank), GetFreeDevice(engine));
            AmountShip += 1;
        }
        else
        {
            IsNeedComponentsForNewShips = true;
            Debug.Log($"IsNeedComponentsForNewShips {IsNeedComponentsForNewShips}");
        }
    }
    private bool IsAllParts(int type)
    {
        int parts = 0;
        if(type == 0)
        {
            if (CountDevice(carcassPas) > 0)
            {
                parts += 1;
            }
        }
        else
        {
            if (CountDevice(carcassCargo) > 0)
            {
                parts += 1;
            }
        }
        if(CountDevice(fuelTank) > 0)
        {
            parts += 1;
        }
        if(CountDevice(engine) > 0)
        {
            parts += 1;
        }
        
        if(parts == 3)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    public int CountCarcassPas(int id = -1)
    {
        //≈сли мы получаем id то пишем количество техники в €чейке, если не получаем, то даем количество €чеек, а не что в них.
        if (id >= 0)
        {
            return carcassPas[id];
        }
        else
            return carcassPas.Count;
    }

    public int CountCarcassCargo(int id = -1)
    {
        if (id >= 0)
        {
            return carcassCargo[id];
        }
        else
            return carcassCargo.Count;
    }

    public int CountEngine(int id = -1)
    {
        if (id >= 0)
        {
            return engine[id];
        }
        else
            return engine.Count;
    }

    public int CountFuelTank(int id = -1)
    {
        ///<summary>
        ///Ѕез параметров - общее количество по индексу, с параметрами - количество именно по индексу 
        ///</summary>
        if (id >= 0)
        {
            return fuelTank[id];
        }
        else
            return fuelTank.Count;
    }

    public void PlusCarcasPas(int id, int value)
    {
        carcassPas[id] += value;
    }

    public void PlusCarcasCargo(int id, int value)
    {
        carcassCargo[id] += value;
    }

    public void PlusEngine(int id, int value)
    {
        engine[id] += value;
    }
    public void PlusFuelTank(int id, int value)
    {
        fuelTank[id] += value;
    }

    private int CountDevice(List<int> device)
    {
        int sum = 0;
        for (int i = 0; i < device.Count; i++)
        {
            sum += device[i];
        }
        return sum;
    }

    private int GetFreeDevice(List<int> device)
    {
        int answ = 0;
        for (int i = 0; i < device.Count; i++)
        {
            if (device[i] > 0)
            {
                answ = i;
                break;
            }
        }
        return answ;
    }

    public int GetFreeWorkers(int value)
    {
        if(FreeWorkers - PlannedWorkerForWork - value >= 0)
        {
            PlannedWorkerForWork += value;
            return value;
        }
        else
        {
            if(FreeWorkers - PlannedWorkerForWork > 0)
            {
                int worker = FreeWorkers - PlannedWorkerForWork;
                PlannedWorkerForWork += worker;
                return worker;
            }
            else
            {
                return 0;
            }
        }
    }

    public void Arrival()
    {
        if (AmountResource >= 0)
        {
            //MainClass.Earth.PlusResAmount(OrientRes.Id, AmountResource);
            Money += AmountResource * OrientRes.Price;
            AmountResource = 0;            
        }
        FreeWorkers += 25;
        Food += 10000;
        Equipment += 10000;
        if (IsNeedComponentsForNewShips)
        {
            IsNeedComponentsForNewShips = false;
            PlusCarcasPas(ScienseDepartment.GetNewestCarcass().Id, 1);
            PlusCarcasCargo(ScienseDepartment.GetNewestCarcass().Id, 1);
            PlusFuelTank(ScienseDepartment.GetNewestFuelTank().Id, 2);
            PlusEngine(ScienseDepartment.GetNewestEngine().Id, 2);

            Debug.Log($" упили обороудование дл€ двух новых кораблей");
        }
    }

    private void Update()
    {
        if(MainClass.GameIsStarted && !IsRoutesCreate)
        {
            IsRoutesCreate = true;
            MiningDepartment.FindAsteroids();
            ShipDepartment.CreateRoutes();
        }
        if(!MainClass.IsPaused)
        {            
            ShipDepartment.ShipWorking();
        }

        //Debug.Log($" оличество кораблей {ShipDepartment.CountShips()}");
        ships = ShipDepartment.CountShips();
        routes = ShipDepartment.CountRoutes();
    }

    public void SaveData(SaveLoadMiningCorp save)
    {
        save.freeWorkers = FreeWorkers;
        save.plannedWorkerForWork = PlannedWorkerForWork;
        save.idOrientRes = OrientRes.Id;
        save.ships = ShipDepartment.CountShips();
        save.food = Food;
        save.equipment = Equipment;
        save.amountResource = AmountResource;
        save.priceShare = PriceShare;
        save.price = Price;
        save.amountShare = FinanceDepartment.AmountShare;
        save.money = Money;
        save.isConstructShip = ShipDepartment.IsConstructShip;
        save.amountRoutes = ShipDepartment.CountRoutes();
        save.isRoutesCreate = IsRoutesCreate;
        save.isNeedComponentsForNewShips = IsNeedComponentsForNewShips;

        CopyDataFromFirstArrayToSecond(carcassPas, save.carcassPas);
        CopyDataFromFirstArrayToSecond(carcassCargo, save.carcassCargo);
        CopyDataFromFirstArrayToSecond(fuelTank, save.fuelTank);
        CopyDataFromFirstArrayToSecond(engine, save.engine);

        for (int j = 0; j < MiningDepartment.CountAsteroids(); j++)
        {
            save.asteroids.Add(MiningDepartment.GetAsteroid(j).Id);
        }
    }
    private void CopyDataFromFirstArrayToSecond(List<int> firstArray, List<int> secondArray)
    {
        for (int i = 0; i < firstArray.Count; i++)
        {
            secondArray.Add(firstArray[i]);
        }
    }

    public void LoadData(SaveLoadMiningCorp save)
    {
        Start();
        FreeWorkers = save.freeWorkers;
        PlannedWorkerForWork = save.plannedWorkerForWork;
        Food = save.food;
        Equipment = save.equipment;
        AmountResource = save.amountResource;
        FinanceDepartment.PriceShare = save.priceShare;
        Price = save.price;
        FinanceDepartment.AmountShare = save.amountShare;
        Money = save.money;
        AmountShip = save.ships;
        ShipDepartment.IsConstructShip = save.isConstructShip;
        AmountRoutes = save.amountRoutes;
        IsRoutesCreate = save.isRoutesCreate;
        IsNeedComponentsForNewShips = save.isNeedComponentsForNewShips;

        CopyDataFromFirstArrayToSecond(save.carcassPas, carcassPas);
        CopyDataFromFirstArrayToSecond(save.carcassCargo, carcassCargo);
        CopyDataFromFirstArrayToSecond(save.fuelTank, fuelTank);
        CopyDataFromFirstArrayToSecond(save.engine, engine);

        for (int j = 0; j < save.asteroids.Count; j++)
        {
            MiningDepartment.SetAsteroid(MainClass.Asteroids.GetSimAsteroid(save.asteroids[j]));
        }
    }
}
