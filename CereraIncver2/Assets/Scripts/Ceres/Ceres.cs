using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ceres : MonoBehaviour
{
    public List<GameObject> templateModules = new List<GameObject>();
    private List<GameObject> modules = new List<GameObject>();
    private List<SpaceStation> spaceStation = new List<SpaceStation>();
    [SerializeField] Rigidbody planet;
    [SerializeField] GameObject stationOnOrbit;
    [SerializeField] CeresPanel ceresPanel;
    [SerializeField] main mainClass;
    public int FreeWorkers { get; set; }
    public int WorkersAwaiting { get; set; }
    public int MaxFreeWorkers { get; set; }
    public float FreeEnergy { get; set; }
    public int ShipsAtJourney { get; set; }
    public int MaxShipsAtJourney { get; set; }
    public float Food { get { return mainClass.Player.Food; } set { mainClass.Player.Food = value; } }
    public float GeneralOxygen { get; set; }
    


    public void CreateStation()
    {
        FreeWorkers = 50;
        for(int i = 0; i < templateModules.Count; i++)
        {
            modules.Add(Instantiate(templateModules[i]));
            InstallComponent(i, modules[i]);
            CreateModule(modules[i], modules[i].GetComponent<SpaceStation>(), modules[i].GetComponent<IModule>(), i);
            spaceStation[i].Workers = spaceStation[i].Workplaces;
            FreeWorkers -= spaceStation[i].Workplaces;
            spaceStation[i].ConnectToPowerGrid();
            spaceStation[i].CalculationOutput();
            ceresPanel.Create_Button(i, spaceStation[i].GetTypeName() + " " + i.ToString());
        }
        CalculationSeason();
        ceresPanel.UpdateText();
    }

    private void InstallComponent(int type, GameObject module)
    {
        switch (type)
        {
            case 0:
                module.AddComponent<LiveModule>();
                break;
            case 1:
                module.AddComponent<EnergyModule>();
                break;
            case 2:
                module.AddComponent<OxygenModule>();
                break;
            case 3:
                module.AddComponent<ComModule>();
                break;
            case 4:
                module.AddComponent<DocModule>();
                break;
            case 5:
                module.AddComponent<AgroModule>();
                break;
            case 6:
                module.AddComponent<WarehouseModule>();
                break;
        }
        module.GetComponent<SpaceStation>().Ceres = this;
    }

    private void CreateModule(GameObject newModule, SpaceStation station, IModule template, int id)
    {
        station.Type = template.Type;
        station.Workplaces = template.Workplaces;
        station.MaxEnergy = template.MaxEnergy;
        station.MaxHealthConstruction = template.MaxHealthConstruction;
        station.HealthConstruction = template.MaxHealthConstruction;
        station.Id = id;
        station.IsConstructed = true;
        station.MaxOutput = template.MaxOutput;
        
        newModule.SetActive(true);
        newModule.transform.SetParent(stationOnOrbit.transform);        
        station.DockingPoint1 = newModule.transform.Find("DockingPoint1");
        station.DockingPoint2 = newModule.transform.Find("DockingPoint1/DockingPoint2");
        if(spaceStation.Count > 0)
        {
            station.DockingPoint1.position = spaceStation[spaceStation.Count - 1].DockingPoint2.position;
            station.DockingPoint1.rotation = spaceStation[spaceStation.Count - 1].DockingPoint2.rotation;
        }
        else
        {
            newModule.transform.position = new Vector3(0, 0, 0);
            newModule.transform.Rotate(new Vector3(0f, 75f));
        }

        spaceStation.Add(station);
        station.DisableInterfaceComponent();

    }

    private void CalculateEnergy(int type = 6)
    {
        
        if(EnergyEnough())
        {
            for (int i = 0; i < spaceStation.Count; i++)
            {
                spaceStation[i].ConnectToPowerGrid();
            }
        }
        else
        {
            for(int i = 0; i < spaceStation.Count; i++)
            {
                if (spaceStation[i].Type == type)
                {
                    spaceStation[i].DisconnectFromPowerGrid();
                    if (EnergyEnough())
                    {
                        break;
                    }
                }
                    
            }
            if (!EnergyEnough())
            {
                if (type > 0)
                {
                    CalculateEnergy(type - 1);
                }
                
            }
        }
        mainClass.ResPanel.UpdateText(5, $"{FreeEnergy}");
    }

    private bool EnergyEnough()
    {
        float energy = 0;
        for (int i = 0; i < spaceStation.Count; i++)
        {
            energy += spaceStation[i].Energy;
            if (spaceStation[i].Type == 1)
            {                
                energy += spaceStation[i].Output;
            }
        }
        FreeEnergy = energy;
        if(energy >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public SpaceStation GetSpaceStation(int id)
    {
        return spaceStation[id];
    }
    
    public void CalculationSeason()
    {
        CalculateEnergy();
        for(int i = 0; i < spaceStation.Count; i++)
        {
            spaceStation[i].CalculationOutput();
            spaceStation[i].GetSeasonOutput();
        }
        GeneralOxygen -= FreeWorkers;
        GeneralOxygen = GeneralOxygen / spaceStation.Count;
        for(int i = 0; i < spaceStation.Count; i++)
        {
            spaceStation[i].Oxygen += GeneralOxygen;
        }
        for (int i = 0; i < spaceStation.Count; i++)
        {
            spaceStation[i].Oxygen += GeneralOxygen;
            spaceStation[i].Oxygen -= spaceStation[i].Workers;            
        }        
    }

    public void OpenPanel()
    {
        ceresPanel.OpenPanel();
    }
    
    public void ChangeAmountWorkers(int amount, int id)
    {
        if(spaceStation[id].Workers > 0 && amount < 0)
        {
            spaceStation[id].Workers -= 1;
        }
        else if(spaceStation[id].Workers < spaceStation[id].Workplaces && amount > 0)
        {
            spaceStation[id].Workers += 1;
        }
        spaceStation[id].CalculationOutput();
        ceresPanel.UpdateText();
        mainClass.ResPanel.UpdateText(2, $"{FreeWorkers}");
    }

    public DockingPort GetFreeDock()
    {
        DockingPort dock = null;
        for (int i = 0; i < spaceStation.Count; i++)
        {
            if(spaceStation[i].Type == 4)
            {
                dock = modules[i].GetComponent<DocModule>().GetFreeDock();
                    
            }
        }

        return dock;
    }

    public DockingPort GetPathToWarehouse()
    {
        DockingPort dock = null;
        for (int i = 0; i < spaceStation.Count; i++)
        {
            if (spaceStation[i].Type == 6)
            {
                dock = modules[i].GetComponent<WarehouseModule>().IsDockFree();

            }
        }

        return dock;
    }

    private void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler((new Vector3(0,2,0)) * Time.fixedDeltaTime);
        planet.MoveRotation(planet.rotation * deltaRotation);
    }

    public int CountModules()
    {
        return spaceStation.Count;
    }

    public SpaceStation GetSpaceModule(int id)
    {
        return spaceStation[id];
    }

    public void SaveData(SaveLoadCeres save)
    {
        save.freeWorkers = FreeWorkers;
        save.workersAwaiting = WorkersAwaiting;
        save.maxFreeWorkers = MaxFreeWorkers;
        save.shipsAtJourney = ShipsAtJourney;
        save.maxShipsAtJourney = MaxShipsAtJourney;
        save.freeEnergy = FreeEnergy;
        save.generalOxygen = GeneralOxygen;
    }

    public void LoadData(SaveLoadCeres save)
    {
        FreeWorkers = save.freeWorkers;
        WorkersAwaiting = save.workersAwaiting;
        MaxFreeWorkers = save.maxFreeWorkers;
        ShipsAtJourney= save.shipsAtJourney;
        MaxShipsAtJourney= save.maxShipsAtJourney;
        FreeEnergy = save.freeEnergy;
        GeneralOxygen = save.generalOxygen;
    }
}
