using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpaceStation : MonoBehaviour, IModule
{
    public int Id { get; set; }
    public int Workplaces { get; set; }
    public float MaxEnergy { get; set; }
    public float Energy { get; set; }
    public int Workers { get; set; }
    public int HealthConstruction { get; set; }
    public int MaxHealthConstruction { get; set; }
    public float Oxygen { get; set; }
    public int WorkersOnConstruction { get; set; }
    public bool IsConstructed { get; set; }
    public bool IsUnderConstruction { get; set; }
    public bool IsHasPower { get; set; }
    public int Type { get; set; }
    protected string[] typeName = new string[7] { "Жилой модуль", "Энергетический модуль", "Кислородный модль", "Модуль управления и связи", "Посадочный модуль", "Аграрный модуль", "Склад" };
    public Transform DockingPoint1 { get; set; }
    public Transform DockingPoint2 { get; set; }

    public float MaxOutput { get; set; }
    
    public float Output { get; set; }
    public Ceres Ceres { get; set; }

    public abstract void DisableInterfaceComponent();
    public abstract void GetSeasonOutput();

    public void DisconnectFromPowerGrid()
    {
        Energy = 0;
        IsHasPower = false;
    }

    public void ConnectToPowerGrid()
    {
        IsHasPower = true;
        Energy = MaxEnergy * (Workers / Workplaces);
    }

    public void CalculationOutput()
    {
        if (IsHasPower)
        {
            Output = MaxOutput * ((float)Workers / (float)Workplaces);
        }
        else
        {
            Output = 0;
        }
    }

    public string GetTypeName()
    {
        return typeName[Type];
    }

    public void SaveData(SaveLoadStationModule save)
    {
        save.id = Id;
        save.workplaces = Workplaces;
        save.workers = Workers;
        save.healthConstruction = HealthConstruction;
        save.maxHealthConstruction = MaxHealthConstruction;
        save.workersOnConstruction = WorkersOnConstruction;
        save.type = Type;
        save.maxEnergy = MaxEnergy;
        save.energy = Energy;
        save.oxygen = Oxygen;
        save.maxOutput = MaxOutput;
        save.output = Output;
        save.isConstructed = IsConstructed;
        save.isUnderConstruction = IsUnderConstruction;
        save.isHasPower = IsHasPower;
    }

    public void LoadData(SaveLoadStationModule save)
    {
        Id = save.id;
        Workplaces = save.workplaces;
        Workers = save.workers;
        HealthConstruction = save.healthConstruction;
        MaxHealthConstruction = save.maxHealthConstruction;
        WorkersOnConstruction = save.workersOnConstruction;
        Type = save.type;
        MaxEnergy = save.maxEnergy;
        Energy = save.energy;
        Oxygen = save.oxygen;
        MaxOutput = save.maxOutput;
        Output = save.output;
        IsConstructed = save.isConstructed;
        IsUnderConstruction = save.isUnderConstruction;
        IsHasPower = save.isHasPower;
    }
}
