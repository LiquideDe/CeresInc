using Mono.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame : MonoBehaviour
{
 
    [SerializeField] private GameObject loadCell, panelLoadGame, variableLoad, modelLoadSlots;
    [SerializeField] private main mainClass;
    [SerializeField] private LoadSlot loadSlot;
    [SerializeField] private BuildShip buildShip;
    List<GameObject> loadCells = new List<GameObject>();
    private bool gameIsLoaded;

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
            if (mainClass.GameIsStarted)
            {
                GlobalForLoad.IsFromOldGame = true;
                GlobalForLoad.Path = dirName;
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
            else
            {
                mainClass.GUI.LoadGame(dirName);
            }
            
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
        Debug.Log($"1 - загружаем заголовок, row = {row}");
        SaveLoadmain loadmain = JsonUtility.FromJson<SaveLoadmain>(data[0]);

        mainClass.CeresTime = loadmain.ceresTime;
        mainClass.Day = loadmain.day;
        mainClass.DayBeforeArrival = loadmain.dayBeforeArrival;
        mainClass.SavePath = loadmain.savePath;
        mainClass.DB.RestorationDbPath(loadmain.savePath);
        row += 1;
        yield return StartCoroutine(LoadResources(loadmain, row, data));
    }

    private IEnumerator LoadResources(SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        //2 - загружаем ресурсы
        Debug.Log($"2 - загружаем ресурсы, row = {row}");
        for (int i = 0; i < mainClass.Materials.MaterialsCount(); i++)
        {
            SaveLoadResource loadResource = JsonUtility.FromJson<SaveLoadResource>(data[i + row]);
            mainClass.Materials.GetMaterial(i).LoadData(loadResource);
        }
        row += mainClass.Materials.MaterialsCount();
        yield return StartCoroutine(LoadAsteroids(loadmain, row, data));
    }

    private IEnumerator LoadAsteroids(SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"3 - загружаем астероиды, row = {row}");
        StartCoroutine(mainClass.Asteroids.CreateAsteroids(loadmain.amountAster, false));
        Debug.Log($"Количество астероидов {loadmain.amountAster}, row = {row}");
        for (int i = 0; i < loadmain.amountAster; i++)
        {
            SaveLoadAsteroid loadAsteroid = JsonUtility.FromJson<SaveLoadAsteroid>(data[i + row]);
            mainClass.Asteroids.GetAsteroid(i).LoadData(loadAsteroid, mainClass.Materials.GetMaterial(loadAsteroid.idElement));
        }
        row += loadmain.amountAster;
        yield return StartCoroutine(LoadSimAsteroids(loadmain, row, data));
    }

    private IEnumerator LoadSimAsteroids(SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"4 - загружаем астероиды симуляции, row = {row}");
        for (int i = 0; i < loadmain.amountAster; i++)
        {
            SaveLoadAsteroid loadAsteroid = JsonUtility.FromJson<SaveLoadAsteroid>(data[i + row]);
            mainClass.Asteroids.GetSimAsteroid(i).LoadData(loadAsteroid, mainClass.Materials.GetMaterial(loadAsteroid.idElement));
        }
        row += loadmain.amountAster;
        yield return StartCoroutine(LoadPlayer(loadmain, row, data));
    }

    private IEnumerator LoadPlayer(SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"5 - загружаем игрока, row = {row}");
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
        yield return StartCoroutine(LoadCorpOnEarth(loadmain, row, data));
    }

    private IEnumerator LoadCorpOnEarth(SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"6 - загружаем корпорации на земле, row = {row}");
        mainClass.Earth.StartGame(false);
        int[] amountBuildings = new int[5];
        for (int i = 0; i < 5; i++)
        {
            SaveLoadEarthCorp loadEarthCorp = JsonUtility.FromJson<SaveLoadEarthCorp>(data[row + i]);
            mainClass.Earth.GetEarthCorp(i).LoadData(loadEarthCorp);
            amountBuildings[i] = loadEarthCorp.amountBuildings;
        }
        row += 5;
        yield return StartCoroutine(LoadBuildingsOnEarth(amountBuildings, loadmain, row, data));
    }

    private IEnumerator LoadBuildingsOnEarth(int[] amountBuildings, SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"7 - загружаем здания корпорации на земле, row = {row}");
        for (int i = 0; i < 5; i++)
        {
            for (int idBuild = 0; idBuild < amountBuildings[i]; idBuild++)
            {
                SaveLoadBuilding loadBuilding = JsonUtility.FromJson<SaveLoadBuilding>(data[row + idBuild]);
                mainClass.Earth.GetEarthCorp(i).SetBuilding(loadBuilding.indexInTemplate, loadBuilding);

            }
            row += amountBuildings[i];
        }
        yield return StartCoroutine(LoadMiningCorp(loadmain, row, data));
    }

    private IEnumerator LoadMiningCorp(SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"8 - загружаем добывающие компании, row = {row}");
        yield return StartCoroutine(mainClass.Corporates.CreateCorporates(false));
        int[] amountShipsAtMiningCorp = new int[mainClass.Corporates.CountMiningCorp()];
        for (int i = 0; i < mainClass.Corporates.CountMiningCorp(); i++)
        {
            SaveLoadMiningCorp loadMiningCorp = JsonUtility.FromJson<SaveLoadMiningCorp>(data[row + i]);
            mainClass.Corporates.GetMiningCorporates(i).LoadData(loadMiningCorp);
            amountShipsAtMiningCorp[i] = loadMiningCorp.ships;
        }
        row += mainClass.Corporates.CountMiningCorp();
        yield return StartCoroutine(LoadPlayersRoutes(amountShipsAtMiningCorp, loadmain, row, data));
    }

    private IEnumerator LoadPlayersRoutes(int[] amountShipsAtMiningCorp, SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"9 - загружаем маршруты игрока, row = {row}");
        ShipRoutes shipRoutes = mainClass.PanelShip.ShipRoutes;
        shipRoutes.CreateEmptyRoutes(loadmain.amountPlayerRoutes);
        for (int i = 0; i < loadmain.amountPlayerRoutes; i++)
        {
            SaveLoadRoute loadRoute = JsonUtility.FromJson<SaveLoadRoute>(data[row + i]);
            shipRoutes.GetRoute(i).LoadData(loadRoute, true);
        }
        row += loadmain.amountPlayerRoutes;
        yield return StartCoroutine(LoadMiningCorpsRoutes(amountShipsAtMiningCorp, loadmain, row, data));
    }

    private IEnumerator LoadMiningCorpsRoutes(int[] amountShipsAtMiningCorp, SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"10 - загружаем маршруты компаний, row = {row}");
        for (int corp = 0; corp < mainClass.Corporates.CountMiningCorp(); corp++)
        {
            ShipDepartment shipDepartment = mainClass.Corporates.GetMiningCorporates(corp).ShipDepartment;
            shipDepartment.CreateEmptyRoute(shipDepartment.Corporate.AmountRoutes);
            for (int route = 0; route < shipDepartment.Corporate.AmountRoutes; route++)
            {
                SaveLoadRoute loadRoute = JsonUtility.FromJson<SaveLoadRoute>(data[row + route]);
                shipDepartment.GetRoute(route).LoadData(loadRoute, false);
            }
            row += shipDepartment.Corporate.AmountRoutes;
        }
        yield return StartCoroutine(LoadPlayersShips(amountShipsAtMiningCorp, loadmain, row, data));
    }

    private IEnumerator LoadPlayersShips(int[] amountShipsAtMiningCorp, SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"11 - загружаем корабли игрока, их {loadmain.playerShips}, row = {row}");
        for (int i = 0; i < loadmain.playerShips; i++)
        {
            SaveLoadShip loadShip = JsonUtility.FromJson<SaveLoadShip>(data[row + i]);
            buildShip.BuildEmptyShip(loadShip.typeShip, loadShip);
        }
        row += loadmain.playerShips;
        yield return StartCoroutine(LoadMiningCorpsShips(amountShipsAtMiningCorp, loadmain, row, data));
    }

    private IEnumerator LoadMiningCorpsShips(int[] amountShipsAtMiningCorp, SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"12 - загружаем корабли корпораций, корпораций всего {mainClass.Corporates.CountMiningCorp()}, row = {row}");
        for (int corp = 0; corp < mainClass.Corporates.CountMiningCorp(); corp++)
        {
            Debug.Log($"Смотрим {corp} корпорацию, кораблей у нее {amountShipsAtMiningCorp[corp]}");
            for (int j = 0; j < amountShipsAtMiningCorp[corp]; j++)
            {
                SaveLoadShipSim loadShipSim = JsonUtility.FromJson<SaveLoadShipSim>(data[row + j]);
                mainClass.Corporates.GetMiningCorporates(corp).ShipDepartment.CreateEmptyShip(loadShipSim);
            }
            Debug.Log($"row {row} += {amountShipsAtMiningCorp[corp]}");
            row += amountShipsAtMiningCorp[corp];
        }
        yield return StartCoroutine(LoadPlayersSciense(loadmain, row, data));
    }

    private IEnumerator LoadPlayersSciense(SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"13 - загружаем науку игрока, row = {row}");
        SaveLoadSciense loadSciense = JsonUtility.FromJson<SaveLoadSciense>(data[row]);
        mainClass.Sciense.LoadData(loadSciense);
        row += 1;
        yield return StartCoroutine(LoadMiningCorpsSciense(loadmain, row, data));
    }

    private IEnumerator LoadMiningCorpsSciense(SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"14 - загружаем науку корпораций, row = {row}");
        for (int corp = 0; corp < mainClass.Corporates.CountMiningCorp(); corp++)
        {
            SaveLoadSciense loadSciense = JsonUtility.FromJson<SaveLoadSciense>(data[row + corp]);
            mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.LoadData(loadSciense);
        }
        row += mainClass.Corporates.CountMiningCorp();
        yield return StartCoroutine(LoadPlayersTechs(loadmain, row, data));
    }

    private IEnumerator LoadPlayersTechs(SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"15 - загружаем технологии игрока, row = {row}");
        for (int i = 0; i < mainClass.Sciense.CountCarcass(); i++)
        {
            SaveLoadTechnology loadTechnology = JsonUtility.FromJson<SaveLoadTechnology>(data[row + i]);
            mainClass.Sciense.GetCarcass(i).LoadData(loadTechnology);
        }
        row += mainClass.Sciense.CountCarcass();

        for (int i = 0; i < mainClass.Sciense.CountFuelTank(); i++)
        {
            SaveLoadTechnology loadTechnology = JsonUtility.FromJson<SaveLoadTechnology>(data[row + i]);
            mainClass.Sciense.GetFuelTank(i).LoadData(loadTechnology);
        }
        row += mainClass.Sciense.CountFuelTank();

        for (int i = 0; i < mainClass.Sciense.CountEngine(); i++)
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
        yield return StartCoroutine(LoadMiningCorpsTechs(loadmain, row, data));
    }

    private IEnumerator LoadMiningCorpsTechs(SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"16 - загружаем технологии корпораций, row = {row}");
        for (int corp = 0; corp < mainClass.Corporates.CountMiningCorp(); corp++)
        {
            for (int tech = 0; tech < mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.CountCarcass(); tech++)
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
        yield return StartCoroutine(LoadOrders(loadmain, row, data));
    }

    private IEnumerator LoadOrders(SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"17 - загружаем заказы, row = {row}");
        for (int i = 0; i < 3; i++)
        {
            SaveLoadOrder loadOrder = JsonUtility.FromJson<SaveLoadOrder>(data[row + i]);
            mainClass.PanelOrder.GetOrder(i).LoadData(loadOrder);
        }
        row += 3;
        yield return StartCoroutine(LoadEarth(loadmain, row, data));
    }

    private IEnumerator LoadEarth(SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"18 - загружаем землю, row = {row}");
        SaveLoadEarth loadEarth = JsonUtility.FromJson<SaveLoadEarth>(data[row]);
        mainClass.Earth.LoadData(loadEarth);
        row += 1;
        yield return StartCoroutine(LoadCeresBase(loadmain, row, data));
    }

    private IEnumerator LoadCeresBase(SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"19 загружаем параметры основной станции, row = {row}");
        SaveLoadCeres saveLoadCeres = JsonUtility.FromJson<SaveLoadCeres>(data[row]);
        mainClass.Ceres.CreateStation();
        mainClass.Ceres.LoadData(saveLoadCeres);
        row += 1;
        yield return StartCoroutine(LoadModulesParametr(loadmain, row, data));
    }

    private IEnumerator LoadModulesParametr(SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"20 - загружаем параметры модулей, row = {row}");
        for (int i = 0; i < mainClass.Ceres.CountModules(); i++)
        {
            SaveLoadStationModule loadStationModule = JsonUtility.FromJson<SaveLoadStationModule>(data[row + i]);
            mainClass.Ceres.GetSpaceModule(i).LoadData(loadStationModule);
        }
        row += mainClass.Ceres.CountModules();
        yield return StartCoroutine(LoadEventPanel(loadmain, row, data));
    }

    private IEnumerator LoadEventPanel(SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        SaveLoadEvent loadEvent = JsonUtility.FromJson<SaveLoadEvent>(data[row]);
        mainClass.EventPanel.LoadData(loadEvent);
        row += 1;
        yield return StartCoroutine(LoadAccidents(loadmain, row, data));
    }

    private IEnumerator LoadAccidents(SaveLoadmain loadmain, int row, string[] data)
    {
        yield return new WaitForEndOfFrame();
        for(int i = 0; i < mainClass.EventPanel.CountEvents(); i++)
        {
            SaveLoadAccident loadAccident = JsonUtility.FromJson<SaveLoadAccident>(data[row + i]);
            mainClass.EventPanel.GetEvent(i).LoadData(loadAccident);            
        }
        row += mainClass.EventPanel.CountEvents();
        yield return StartCoroutine(LoadAnothers());
    }
    private IEnumerator LoadAnothers()
    {
        
        yield return new WaitForEndOfFrame();
        Debug.Log($"Загружаем остальное");
        panelLoadGame.SetActive(false);
        mainClass.CreatePanels();
        yield return StartCoroutine(mainClass.PanelStation.CreateButtons());
        mainClass.HelloPanel.SetActive(false);
        mainClass.GameIsStarted = true;
        yield return new WaitForEndOfFrame();
    }
}
