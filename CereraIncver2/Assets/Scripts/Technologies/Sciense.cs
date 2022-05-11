using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Sciense: MonoBehaviour
{

    public main mainClass;
    //—писки технологий
    protected List<Carcass> techCarcass = new List<Carcass>();
    protected List<FuelTank> techFuelTank = new List<FuelTank>();
    protected List<Engine> techEngine = new List<Engine>();
    protected List<ResTech> techResource = new List<ResTech>();
    public float BetterDigFromSciense { get; set; }

    //List<Technology> lists = new List<Technology>();
    //список карточек 

    private void Start()
    {
        BetterDigFromSciense = 0;
    }
    public void StartGame()
    {
        //добавл€ем техи, первые 4 свойства одинаковые у всех: Ќазвание, изучено ли, стоимость изучени€, описание
        //ƒобавл€ем техи в каркас ¬ес, полезна€ нагрузка, стоимость
        techCarcass.Add(new Carcass("“итановый корпус", true, 1000, "ќписание", 30000, 15000, 10000, this));
        //добавл€ем техи в топливные баки: ¬ес, максимально топливо, стоимость
        techFuelTank.Add(new FuelTank("ќбычный бак", true, 1000, "Descr", 20000, 50000, 10000, this));
        //добавл€ем техи в движки: isp, вес, типо топлива, сила т€ги, стоимость
        techEngine.Add(new Engine("¬одородный двигатель", true, 1000, "Descr", 1000, 1000, 0, 245000, 10000, this));        

        for (int i = 0; i < 15; i++)
        {
            techCarcass.Add(new Carcass($"Update carcass {i}", (i + 1) * 1000, UnityEngine.Random.Range(1, 10), UnityEngine.Random.Range(1, 10), this));
        }
        for (int i = 0; i < 15; i++)
        {
            techFuelTank.Add(new FuelTank($"Update tank {i}", (i + 1) * 1000, UnityEngine.Random.Range(1, 10), UnityEngine.Random.Range(1, 10), this));
        }
        for (int i = 0; i < 15; i++)
        {
            techEngine.Add(new Engine($"Update engine {i}", (i + 1) * 1000, UnityEngine.Random.Range(1, 10), UnityEngine.Random.Range(1, 10), this));
        }
        CreateResTech();

        SetId();
        PutInListAndButton();
    }

    
    protected abstract void CreateResTech();
    protected void SetId()
    {
        //”станавливаем максимальное количество "пунктов" и смотрим в какой техе больше всего и потом ставим всем id
        //”словие чтобы избежать ошибки выхода за границы массива out of range
        int maxCarcass = CountCarcass() - 1;
        int maxEngine = CountEngine() - 1;
        int maxFuelTank = CountFuelTank() - 1;
        int maxResTech = CountResTech() - 1;

        for (int i = 0; i < Math.Max(Math.Max(maxCarcass, maxResTech), Math.Max(maxEngine, maxFuelTank)); i++)
        {
            if (maxCarcass >= i)
                techCarcass[i].Id = i;
            if (maxFuelTank >= i)
                techFuelTank[i].Id = i;
            if (maxEngine >= i)
                techEngine[i].Id = i;
            if (maxResTech >= i)
                techResource[i].Id = i;
        }
    }
    public abstract void PutInListAndButton();
    private void CreateEmptyTech(int id, int amount)
    {
        if(id == 0)
        {
            for(int i = 0; i < amount; i++)
            {
                techCarcass.Add(new Carcass(this));
            } 
        }
        else if(id == 1)
        {
            for(int i = 0; i < amount; i++)
            {
                techFuelTank.Add(new FuelTank(this));
            }
        }
        else if(id == 2)
        {
            for (int i = 0; i < amount; i++)
            {
                techEngine.Add(new Engine(this));
            }
        }
        else
        {
            for (int i = 0; i < amount; i++)
            {
                techResource.Add(new ResTech(this));
            }
        }
    }

    

    
    public Carcass GetCarcass(int id)
    {
        return techCarcass[id];
    }

    public FuelTank GetFuelTank(int id)
    {
        return techFuelTank[id];
    }

    public Engine GetEngine(int id)
    {
        return techEngine[id];
    }

    public ResTech GetResTech(int id)
    {
        return techResource[id];
    }

    public int CountCarcass()
    {
        return techCarcass.Count;
    }

    public int CountFuelTank()
    {
        return techFuelTank.Count;
    }

    public int CountEngine()
    {
        return techEngine.Count;
    }

    public int CountResTech()
    {
        return techResource.Count;
    }

    public Engine GetNewestEngine()
    {
        Engine engine = null;
        for (int i = 0; i < techEngine.Count; i++)
        {
            if (techEngine[i].IsResearched)
            {
                engine = techEngine[i];
            }
        }

        return engine;
    }

    public Carcass GetNewestCarcass()
    {
        Carcass carcass = null;
        for(int i = 0; i < techCarcass.Count; i++)
        {
            if (techCarcass[i].IsResearched)
            {
                carcass = techCarcass[i];
            }
        }

        return carcass;
    }

    public FuelTank GetNewestFuelTank()
    {
        FuelTank fuelTank = null;
        for(int i = 0; i < techFuelTank.Count; i++)
        {
            if (techFuelTank[i].IsResearched)
            {
                fuelTank = techFuelTank[i];
            }
        }

        return fuelTank;
    }



    public abstract void CalculationMonth();    

    public void TechnologyResearched(int id, int firstUpdateAmount, int secondUpdateAmount, int indexTech)
    {
        if(id == 0)
        {
            techCarcass[indexTech - 1].IsOutdated = true;
            techCarcass[indexTech].NameTech = techCarcass[indexTech - 1].NameTech + $"-{indexTech}";
            techCarcass[indexTech].Weight = techCarcass[indexTech - 1].Weight - (techCarcass[indexTech - 1].Weight*(firstUpdateAmount/100));
            techCarcass[indexTech].MaxWeightPlayload = techCarcass[indexTech - 1].MaxWeightPlayload - (techCarcass[indexTech - 1].MaxWeightPlayload * (secondUpdateAmount / 100));
            techCarcass[indexTech].Cost = techCarcass[indexTech - 1].Cost + (techCarcass[indexTech - 1].Cost * ((firstUpdateAmount + secondUpdateAmount)/100));            
        }
        else if(id == 1)
        {
            techFuelTank[indexTech - 1].IsOutdated = true;
            techFuelTank[indexTech].NameTech = techFuelTank[indexTech - 1].NameTech + $"-{indexTech}";
            techFuelTank[indexTech].Weight = techFuelTank[indexTech - 1].Weight - (techFuelTank[indexTech - 1].Weight * (firstUpdateAmount / 100));
            techFuelTank[indexTech].MaxFuel = techFuelTank[indexTech - 1].MaxFuel - (techFuelTank[indexTech - 1].MaxFuel * (secondUpdateAmount / 100));
            techFuelTank[indexTech].Cost = techFuelTank[indexTech - 1].Cost + (techFuelTank[indexTech - 1].Cost * ((firstUpdateAmount + secondUpdateAmount) / 100));
        }
        else if (id == 2)
        {
            techEngine[indexTech - 1].IsOutdated = true;
            techEngine[indexTech].NameTech = techEngine[indexTech - 1].NameTech + $"-{indexTech}";
            techEngine[indexTech].Weight = techEngine[indexTech - 1].Weight - (techEngine[indexTech - 1].Weight * (firstUpdateAmount / 100));
            techEngine[indexTech].Isp = techEngine[indexTech - 1].Isp - (techEngine[indexTech - 1].Isp * (secondUpdateAmount / 100));
            techEngine[indexTech].Cost = techEngine[indexTech - 1].Cost + (techEngine[indexTech - 1].Cost * ((firstUpdateAmount + secondUpdateAmount) / 100));
        }
        GiveTechForBut(id);

    }

    public abstract void ResTechResearched(int id);

    protected abstract void GiveTechForBut(int id);

    public void SaveData(SaveLoadSciense save)
    {
        save.countCarcassTech = CountCarcass();
        save.countFuelTankTech = CountFuelTank();
        save.countEngineTech = CountEngine();
        save.countResTech = CountResTech();
        save.betterDigFromSciense = BetterDigFromSciense;

        SaveAnotherData(save);
    }

    protected abstract void SaveAnotherData(SaveLoadSciense save);

    public void LoadData(SaveLoadSciense save)
    {
        BetterDigFromSciense = save.betterDigFromSciense;
        CreateEmptyTech(0, save.countCarcassTech);
        CreateEmptyTech(1, save.countFuelTankTech);
        CreateEmptyTech(2, save.countEngineTech);
        CreateEmptyTech(3, save.countResTech);

        LoadAnotherData(save);
    }

    protected abstract void LoadAnotherData(SaveLoadSciense save);

}
