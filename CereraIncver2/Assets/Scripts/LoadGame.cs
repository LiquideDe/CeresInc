using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
 
    [SerializeField] private GameObject loadCell, panelLoadGame, variableLoad, modelLoadSlots;
    [SerializeField] private main mainClass;
    [SerializeField] private LoadSlot loadSlot;
    [SerializeField] private BuildShip buildShip;
    List<GameObject> loadCells = new List<GameObject>();

    public void ShowLoads()
    {
        panelLoadGame.SetActive(true);
        List<string> dirs = new List<string>();
        dirs.AddRange(Directory.GetDirectories($"{Application.dataPath}/SaveGames"));
        for(int i = 0; i < dirs.Count; i++)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirs[i]);
            loadCells.Add(Instantiate(loadCell));
            loadCells[i].transform.SetParent(variableLoad.transform);
            var cell = loadCells[i].GetComponent<LoadButton>();
            cell.textName.text = $"{dirInfo.Name}";
            cell.textDate.text = $"{dirInfo.CreationTime}";
            cell.fullPath = $"{dirInfo.FullName}";
            loadCells[i].SetActive(true);
        }
    }

    public void ChooseLoad(string dirName)
    {
        if(dirName.Substring(dirName.Length-4) != "JSON")
        {
            CleaningLoadCells();
            variableLoad.SetActive(false);
            modelLoadSlots.SetActive(true);
            List<string> filesName = new List<string>();
            filesName.AddRange(Directory.GetFiles(dirName));
            for(int i = 0; i < filesName.Count; i++)
            {
                if (filesName[i].Substring(filesName[i].Length - 4) == "JSON")
                {
                    loadSlot.Create_Button(i, filesName[i]);
                }
            }
        }
        else
        {            
            //mainClass.incs.Add(new Inc("player", 1000000));
            mainClass.GUI.LoadGame(dirName);
        }
    }

    public string LoadHeading(string nameSave)
    {
        string text = "";
        string[] data = File.ReadAllLines(nameSave);
        SaveLoadmain loadmain = JsonUtility.FromJson<SaveLoadmain>(data[0]);
        text = $"День {loadmain.ceresTime}, на счету {loadmain.money}$, всего кораблей {loadmain.playerShips}";

        return text; 
    }

    private void CleaningLoadCells()
    {
        for(int i = 0; i < loadCells.Count; i++)
        {
            Destroy(loadCells[i].gameObject);
        }
        loadCells.Clear();
    }

    public IEnumerator LoadData(string dirName)
    {
        yield return StartCoroutine(mainClass.GUI.Prohod2());
        string[] data = File.ReadAllLines(dirName);
        int row = 0;
        //1 - загружаем заголовок
        SaveLoadmain loadmain = JsonUtility.FromJson<SaveLoadmain>(data[0]);

        mainClass.CeresTime = loadmain.ceresTime;
        mainClass.Day = loadmain.day;
        mainClass.DayBeforeArrival = loadmain.dayBeforeArrival;
        mainClass.SavePath = loadmain.savePath;
        mainClass.DB.RestorationDbPath(loadmain.savePath);
        row += 1;
        //2 - загружаем ресурсы
        for (int i = 0; i < mainClass.Materials.MaterialsCount(); i++)
        {
            SaveLoadResource loadResource = JsonUtility.FromJson<SaveLoadResource>(data[i + row]);
            mainClass.Materials.GetMaterial(i).LoadData(loadResource);
        }
        row += mainClass.Materials.MaterialsCount();
        //3 - загружаем астероиды
        mainClass.Asteroids.CreateAsteroids(loadmain.amountAster, false);
        for(int i = 0; i < loadmain.amountAster; i++)
        {
            SaveLoadAsteroid loadAsteroid = JsonUtility.FromJson<SaveLoadAsteroid>(data[i + row]);
            mainClass.Asteroids.GetAsteroid(i).LoadData(loadAsteroid, mainClass.Materials.GetMaterial(loadAsteroid.idElement));
        }
        row += loadmain.amountAster;

        //4 - загружаем астероиды симуляции
        for(int i = 0; i < loadmain.amountAster; i++)
        {
            SaveLoadAsteroid loadAsteroid = JsonUtility.FromJson<SaveLoadAsteroid>(data[i + row]);
            mainClass.Asteroids.GetSimAsteroid(i).LoadData(loadAsteroid, mainClass.Materials.GetMaterial(loadAsteroid.idElement));
        }
        row += loadmain.amountAster;

        //5 - загружаем игрока
        SaveLoadInc loadInc = JsonUtility.FromJson<SaveLoadInc>(data[row]);
        mainClass.Player.NameInc = loadInc.nameInc;
        mainClass.Player.Money = loadInc.money;
        mainClass.Player.Id = loadInc.id;
        mainClass.Player.Equipment = loadInc.equipment;
        mainClass.Player.Food = loadInc.food;
        for (int j = 0; j < loadInc.amountRes.Length; j++)
        {
            mainClass.Player.PlusAmountRes(j, loadInc.amountRes[j]);
        }
        for (int j = 0; j < loadInc.carcassPas.Count; j++)
        {
            mainClass.Player.PlusCarcassPas(j, loadInc.carcassPas[j]);
        }
        for (int j = 0; j < loadInc.carcassCargo.Count; j++)
        {
            mainClass.Player.PlusCarcassCargo(j, loadInc.carcassCargo[j]);
        }
        for (int j = 0; j < loadInc.fuelTank.Count; j++)
        {
            mainClass.Player.PlusFuelTank(j, loadInc.fuelTank[j]);
        }
        for (int j = 0; j < loadInc.engine.Count; j++)
        {
            mainClass.Player.PlusEngine(j, loadInc.engine[j]);
        }
        row += 1;

        //6 - загружаем корпорации на земле
        mainClass.Earth.StartGame(false);
        int[] amountBuildings = new int[5];
        for(int i=0; i<5; i++)
        {
            SaveLoadEarthCorp loadEarthCorp = JsonUtility.FromJson<SaveLoadEarthCorp>(data[row + i]);
            mainClass.Earth.GetEarthCorp(i).LoadData(loadEarthCorp);
            amountBuildings[i] = loadEarthCorp.amountBuildings;
        }
        row += 5;
        //7 - загружаем здания корпорации на земле
        for(int i =0; i<5; i++)
        {
            for(int idBuild = 0; idBuild < amountBuildings[i]; idBuild++)
            {
                SaveLoadBuilding loadBuilding = JsonUtility.FromJson<SaveLoadBuilding>(data[row + idBuild]);
                mainClass.Earth.GetEarthCorp(i).SetBuilding(loadBuilding.indexInTemplate, loadBuilding);

            }
            row += amountBuildings[i];
        }

        //8 - загружаем добывающие компании        
        mainClass.Corporates.CreateCorporates(false);
        int[] amountShipsAtMiningCorp = new int[mainClass.Corporates.CountMiningCorp()];
        for (int i = 0; i < mainClass.Corporates.CountMiningCorp(); i++)
        {
            SaveLoadMiningCorp loadMiningCorp = JsonUtility.FromJson<SaveLoadMiningCorp>(data[row + i]);
            mainClass.Corporates.GetMiningCorporates(i).LoadData(loadMiningCorp);
            amountShipsAtMiningCorp[i] = loadMiningCorp.ships;
        }
        row += mainClass.Corporates.CountMiningCorp();

        //9 - загружаем корабли игрока
        for(int i = 0; i < loadmain.playerShips; i++)
        {
            SaveLoadShip loadShip = JsonUtility.FromJson<SaveLoadShip>(data[row + i]);
            buildShip.BuildEmptyShip(loadShip.typeShip, loadShip);
        }
        row += loadmain.playerShips;

        //10 - загружаем корабли корпораций
        for(int corp = 0; corp < mainClass.Corporates.CountMiningCorp(); corp++)
        {
            for (int j = 0; j < amountShipsAtMiningCorp[corp]; j++)
            {
                SaveLoadShipSim loadShipSim = JsonUtility.FromJson<SaveLoadShipSim>(data[row + j]);
                mainClass.Corporates.GetMiningCorporates(corp).ShipDepartment.CreateEmptyShip(loadShipSim);
            }
            row += amountShipsAtMiningCorp[corp];
        }

        //11 - загружаем науку игрока
        SaveLoadSciense loadSciense = JsonUtility.FromJson<SaveLoadSciense>(data[row]);
        mainClass.Sciense.LoadData(loadSciense);
        row += 1;

        //12 - загружаем науку корпораций
        for(int corp = 0; corp < mainClass.Corporates.CountMiningCorp(); corp++)
        {
            loadSciense = JsonUtility.FromJson<SaveLoadSciense>(data[row + corp]);
            mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.LoadData(loadSciense);
        }
        row += mainClass.Corporates.CountMiningCorp();

        //13 - загружаем технологии игрока
        for(int i = 0; i < mainClass.Sciense.CountCarcass(); i++)
        {
            SaveLoadTechnology loadTechnology = JsonUtility.FromJson<SaveLoadTechnology>(data[row + i]);
            mainClass.Sciense.GetCarcass(i).LoadData(loadTechnology);
        }
        row += mainClass.Sciense.CountCarcass();

        for(int i = 0; i < mainClass.Sciense.CountFuelTank(); i++)
        {
            SaveLoadTechnology loadTechnology = JsonUtility.FromJson<SaveLoadTechnology>(data[row + i]);
            mainClass.Sciense.GetFuelTank(i).LoadData(loadTechnology);
        }
        row += mainClass.Sciense.CountFuelTank();

        for(int i = 0; i < mainClass.Sciense.CountEngine(); i++)
        {
            SaveLoadTechnology loadTechnology = JsonUtility.FromJson<SaveLoadTechnology>(data[row + i]);
            mainClass.Sciense.GetEngine(i).LoadData(loadTechnology);
        }
        row += mainClass.Sciense.CountEngine();

        for (int i = 0; i < mainClass.Sciense.CountResTech(); i++)
        {
            SaveLoadTechnology loadTechnology = JsonUtility.FromJson<SaveLoadTechnology>(data[row + i]);
            mainClass.Sciense.GetResTech(i).LoadData(loadTechnology);
        }
        row += mainClass.Sciense.CountResTech();

        mainClass.Sciense.PutInListAndButton();

        //14 - загружаем технологии корпораций
        for(int corp = 0; corp < mainClass.Corporates.CountMiningCorp(); corp++)
        {
            for(int tech = 0; tech < mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.CountCarcass(); tech++)
            {
                SaveLoadTechnology loadTechnology = JsonUtility.FromJson<SaveLoadTechnology>(data[row + tech]);
                mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.GetCarcass(tech).LoadData(loadTechnology);
            }
            row += mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.CountCarcass();

            for (int tech = 0; tech < mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.CountFuelTank(); tech++)
            {
                SaveLoadTechnology loadTechnology = JsonUtility.FromJson<SaveLoadTechnology>(data[row + tech]);
                mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.GetFuelTank(tech).LoadData(loadTechnology);
            }
            row += mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.CountFuelTank();

            for (int tech = 0; tech < mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.CountEngine(); tech++)
            {
                SaveLoadTechnology loadTechnology = JsonUtility.FromJson<SaveLoadTechnology>(data[row + tech]);
                mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.GetEngine(tech).LoadData(loadTechnology);
            }
            row += mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.CountEngine();

            for (int tech = 0; tech < mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.CountResTech(); tech++)
            {
                SaveLoadTechnology loadTechnology = JsonUtility.FromJson<SaveLoadTechnology>(data[row + tech]);
                mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.GetResTech(tech).LoadData(loadTechnology);
            }
            row += mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.CountResTech();
            mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.PutInListAndButton();
        }

        //15 - загружаем заказы
        for(int i = 0; i < 3; i++)
        {
            SaveLoadOrder loadOrder = JsonUtility.FromJson<SaveLoadOrder>(data[row + i]);
            mainClass.PanelOrder.GetOrder(i).LoadData(loadOrder);
        }
        row += 3;

        //16 - загружаем землю
        SaveLoadEarth loadEarth = JsonUtility.FromJson<SaveLoadEarth>(data[row]);
        mainClass.Earth.LoadData(loadEarth);
        row += 1;

        //17 загружаем параметры основной станции
        SaveLoadCeres saveLoadCeres = JsonUtility.FromJson<SaveLoadCeres>(data[row]);
        mainClass.Ceres.CreateStation();
        mainClass.Ceres.LoadData(saveLoadCeres);
        
        row += 1;

        //18 - загружаем параметры модулей
        for(int i=0; i < mainClass.Ceres.CountModules(); i++)
        {
            SaveLoadStationModule loadStationModule = JsonUtility.FromJson<SaveLoadStationModule>(data[row + i]);
            mainClass.Ceres.GetSpaceModule(i).LoadData(loadStationModule);
        }

        panelLoadGame.SetActive(false);
        mainClass.CreatePanels();
        mainClass.PanelStation.CreateButtons();
        mainClass.GameIsStarted = true;
        mainClass.HelloPanel.SetActive(false);  
    }
}
