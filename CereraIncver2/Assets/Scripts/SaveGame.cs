using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveGame : Spisok
{
    [SerializeField] private main mainClass;
    [SerializeField] private GameObject SavePanel;
    [SerializeField] private InputField saveName;

    public void SaveData(string nameSave)
    {
        //Сохранение игрового процесса. Сначала создаем путь, куда сохранять с названием и расширением
        //Затем создаем переменную куда впихиваем в виде текста все значения
        var path = Path.Combine(mainClass.SavePath, nameSave + ".JSON");
        List<string> data = new List<string>();

        //1 - создаем сохранение общих данных. количество астероидов, кораблей, технологий, дней.
        SaveLoadmain saveLoadmain = new SaveLoadmain();
        saveLoadmain.amountAster = mainClass.Asteroids.AsteroidsCount();
        saveLoadmain.ceresTime = mainClass.CeresTime;
        saveLoadmain.day = mainClass.Day;
        saveLoadmain.dayBeforeArrival = mainClass.DayBeforeArrival;
        saveLoadmain.savePath = mainClass.SavePath;
        saveLoadmain.playerShips = mainClass.PanelShip.ShipCount();
        saveLoadmain.money = mainClass.Player.Money;

        data.Add(JsonUtility.ToJson(saveLoadmain));
        //2 - Сохраняем ресурсы
        List<SaveLoadResource> saveLoadResources = new List<SaveLoadResource>();
        for(int i = 0; i < mainClass.Materials.MaterialsCount(); i++)
        {
            saveLoadResources.Add(new SaveLoadResource());
            mainClass.Materials.GetMaterial(i).SaveData(saveLoadResources[i]);

            data.Add(JsonUtility.ToJson(saveLoadResources[i]));
        }

        //3 - Сохраняем астероиды
        int amountAster = mainClass.Asteroids.AsteroidsCount();
        List<SaveLoadAsteroid> saveLoadAsteroids = new List<SaveLoadAsteroid>();
        for(int i = 0; i < amountAster; i++)
        {
            saveLoadAsteroids.Add(new SaveLoadAsteroid());
            mainClass.Asteroids.GetAsteroid(i).SaveData(saveLoadAsteroids[i]);

            data.Add(JsonUtility.ToJson(saveLoadAsteroids[i]));
        }

        //4 - Сохраняем астероиды для симуляции        
        for (int i= 0; i< amountAster; i++)
        {
            saveLoadAsteroids.Add(new SaveLoadAsteroid());
            mainClass.Asteroids.GetSimAsteroid(i).SaveData(saveLoadAsteroids[i + amountAster]);
            
            data.Add(JsonUtility.ToJson(saveLoadAsteroids[i + amountAster]));
        }

        //5 - Сохраняем игрока
        SaveLoadInc saveLoadInc = new SaveLoadInc();

        saveLoadInc.nameInc = mainClass.Player.NameInc;
        saveLoadInc.money = mainClass.Player.Money;
        saveLoadInc.id = mainClass.Player.Id;
        saveLoadInc.equipment = mainClass.Player.Equipment;
        saveLoadInc.food = mainClass.Player.Food;
        for (int j = 0; j < mainClass.Player.CountLengthAmountRes(); j++)
            {
            saveLoadInc.amountRes[j] = mainClass.Player.GetAmountRes(j);
            }
        for (int j = 0; j < mainClass.Player.CountCarcassPas(); j++)
            {
            saveLoadInc.carcassPas.Add(mainClass.Player.CountCarcassPas(j));
            }
        for (int j = 0; j < mainClass.Player.CountCarcassCargo(); j++)
            {
            saveLoadInc.carcassCargo.Add(mainClass.Player.CountCarcassCargo(j));
            }
        for (int j = 0; j < mainClass.Player.CountFuelTank(); j++)
            {
            saveLoadInc.fuelTank.Add(mainClass.Player.CountFuelTank(j));
            }
        for (int j = 0; j < mainClass.Player.CountEngine(); j++)
            {
            saveLoadInc.engine.Add(mainClass.Player.CountEngine(j));
            }
        for (int j = 0; j < mainClass.Player.CountAsteroids(); j++)
            {
            saveLoadInc.asteroids.Add(mainClass.Player.GetAsteroid(j).Id);
            }
        data.Add(JsonUtility.ToJson(saveLoadInc));

        //6 - Сохраняем корпорации на земле
        List<SaveLoadEarthCorp> saveLoadEarthCorps = new List<SaveLoadEarthCorp>();
        for(int i =0; i < 5; i++)
        {
            saveLoadEarthCorps.Add(new SaveLoadEarthCorp());
            mainClass.Earth.GetEarthCorp(i).SaveData(saveLoadEarthCorps[i]);

            data.Add(JsonUtility.ToJson(saveLoadEarthCorps[i]));
        }

        //7 - Сохраняем здания корпораций на земле
        List<SaveLoadBuilding> saveLoadBuildings = new List<SaveLoadBuilding>();
        for(int i=0; i < 5; i++)
        {
            for(int idBuild = 0; idBuild < mainClass.Earth.GetEarthCorp(i).CountBuildings(); idBuild++)
            {
                saveLoadBuildings.Add(new SaveLoadBuilding());
                saveLoadBuildings[i + idBuild].idEarthCorp = i;
                mainClass.Earth.GetEarthCorp(i).GetBuilding(idBuild).GetCleanBuilding().SaveData(saveLoadBuildings[i + idBuild]);

                data.Add(JsonUtility.ToJson(saveLoadBuildings[i + idBuild]));
            }
        }

        //8 - Сохраняем добывающие корпорации
        List<SaveLoadMiningCorp> saveLoadMiningCorps = new List<SaveLoadMiningCorp>();
        for(int i = 0; i < mainClass.Corporates.CountMiningCorp(); i++)
        {
            saveLoadMiningCorps.Add(new SaveLoadMiningCorp());
            mainClass.Corporates.GetMiningCorporates(i).SaveData(saveLoadMiningCorps[i]);            

            data.Add(JsonUtility.ToJson(saveLoadMiningCorps[i]));
        }

        //9 - Сохраняем корабли игрока
        List<SaveLoadShip> saveLoadShips = new List<SaveLoadShip>();
        for(int i = 0; i < mainClass.PanelShip.ShipCount(); i++)
        {
            saveLoadShips.Add(new SaveLoadShip());
            Ship ship = mainClass.PanelShip.GetShip(i).GetComponent<Ship>();
            ship.SaveData(saveLoadShips[i]);
            
            data.Add(JsonUtility.ToJson(saveLoadShips[i]));
        }

        //10 - Сохраняем корабли добывающих компаний
        List<SaveLoadShipSim> saveLoadShipSims = new List<SaveLoadShipSim>();
        for(int idCorp = 0; idCorp < mainClass.Corporates.CountMiningCorp(); idCorp++)
        {
            for(int idShip = 0; idShip < mainClass.Corporates.GetMiningCorporates(idCorp).AmountShip; idShip++)
            {
                saveLoadShipSims.Add(new SaveLoadShipSim());
                mainClass.Corporates.GetMiningCorporates(idCorp).ShipDepartment.GetShip(idShip).SaveData(saveLoadShipSims[idShip]);
                
                data.Add(JsonUtility.ToJson(saveLoadShipSims[idCorp + idShip]));
            }            
        }

        //11 - сохраняем науку игрока
        SaveLoadSciense saveLoadSciense = new SaveLoadSciense();
        mainClass.Sciense.SaveData(saveLoadSciense);

        data.Add(JsonUtility.ToJson(saveLoadSciense));

        //12 - сохраняем науку корпораций
        List<SaveLoadSciense> saveLoadScienseList = new List<SaveLoadSciense>();
        for(int corp = 0; corp < mainClass.Corporates.CountMiningCorp(); corp++)
        {
            saveLoadScienseList.Add(new SaveLoadSciense());
            mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.SaveData(saveLoadScienseList[corp]);            

            data.Add(JsonUtility.ToJson(saveLoadScienseList[corp]));
        }

        //13 - сохраняем технологии игрока
        List<SaveLoadTechnology> saveLoadTechnologies = new List<SaveLoadTechnology>();
        int amountTech = 0;
        for(int i = 0; i < mainClass.Sciense.CountCarcass(); i++)
        {
            saveLoadTechnologies.Add(new SaveLoadTechnology());
            mainClass.Sciense.GetCarcass(i).SaveData(saveLoadTechnologies[i]);

            data.Add(JsonUtility.ToJson(saveLoadTechnologies[i]));
        }
        amountTech += mainClass.Sciense.CountCarcass();

        for (int i = 0; i < mainClass.Sciense.CountFuelTank(); i++)
        {
            saveLoadTechnologies.Add(new SaveLoadTechnology());
            mainClass.Sciense.GetFuelTank(i).SaveData(saveLoadTechnologies[i + amountTech]);

            data.Add(JsonUtility.ToJson(saveLoadTechnologies[i + amountTech]));
        }
        amountTech += mainClass.Sciense.CountFuelTank();

        for (int i = 0; i < mainClass.Sciense.CountEngine(); i++)
        {
            saveLoadTechnologies.Add(new SaveLoadTechnology());
            mainClass.Sciense.GetEngine(i).SaveData(saveLoadTechnologies[i + amountTech]);

            data.Add(JsonUtility.ToJson(saveLoadTechnologies[i + amountTech]));
        }
        amountTech += mainClass.Sciense.CountEngine();

        for (int i = 0; i < mainClass.Sciense.CountResTech(); i++)
        {
            saveLoadTechnologies.Add(new SaveLoadTechnology());
            mainClass.Sciense.GetResTech(i).SaveData(saveLoadTechnologies[i + amountTech]);

            data.Add(JsonUtility.ToJson(saveLoadTechnologies[i + amountTech]));
        }
        amountTech += mainClass.Sciense.CountResTech();

        //14 - сохраняем технологии корпораций
        int amountMinTech = 0;
        int allAmount;
        for(int corp = 0; corp < mainClass.Corporates.CountMiningCorp(); corp++)
        {
            for(int tech = 0; tech < mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.CountCarcass(); tech++)
            {
                saveLoadTechnologies.Add(new SaveLoadTechnology());
                allAmount = amountTech + tech + amountMinTech;
                mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.GetCarcass(tech).SaveData(saveLoadTechnologies[allAmount]);

                data.Add(JsonUtility.ToJson(saveLoadTechnologies[allAmount]));
            }
            amountMinTech += mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.CountCarcass();

            for (int tech = 0; tech < mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.CountFuelTank(); tech++)
            {
                saveLoadTechnologies.Add(new SaveLoadTechnology());
                allAmount = amountTech + tech + amountMinTech;
                mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.GetFuelTank(tech).SaveData(saveLoadTechnologies[allAmount]);

                data.Add(JsonUtility.ToJson(saveLoadTechnologies[allAmount]));
            }
            amountMinTech += mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.CountFuelTank();

            for (int tech = 0; tech < mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.CountEngine(); tech++)
            {
                saveLoadTechnologies.Add(new SaveLoadTechnology());
                allAmount = amountTech + tech + amountMinTech;
                mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.GetEngine(tech).SaveData(saveLoadTechnologies[allAmount]);

                data.Add(JsonUtility.ToJson(saveLoadTechnologies[allAmount]));
            }
            amountMinTech += mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.CountEngine();

            for (int tech = 0; tech < mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.CountResTech(); tech++)
            {
                saveLoadTechnologies.Add(new SaveLoadTechnology());
                allAmount = amountTech + tech + amountMinTech;
                mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.GetResTech(tech).SaveData(saveLoadTechnologies[allAmount]);

                data.Add(JsonUtility.ToJson(saveLoadTechnologies[allAmount]));
            }
            amountMinTech += mainClass.Corporates.GetMiningCorporates(corp).ScienseDepartment.CountResTech();
        }

        //15 - сохраняем заказы
        List<SaveLoadOrder> saveLoadOrders = new List<SaveLoadOrder>();
        for (int i = 0; i < 3; i++)
        {
            saveLoadOrders.Add(new SaveLoadOrder());
            mainClass.PanelOrder.GetOrder(i).SaveData(saveLoadOrders[i]);
            
            data.Add(JsonUtility.ToJson(saveLoadOrders[i]));
        }

        //16 - сохраняем землю
        SaveLoadEarth saveLoadEarths = new SaveLoadEarth();
        mainClass.Earth.SaveData(saveLoadEarths);
        data.Add(JsonUtility.ToJson(saveLoadEarths));

        //17 - сохраняем параметры основной станции
        SaveLoadCeres saveLoadCeres = new SaveLoadCeres();
        mainClass.Ceres.SaveData(saveLoadCeres);

        data.Add(JsonUtility.ToJson(saveLoadCeres));

        //18 - сохраняем параметры модулей
        List<SaveLoadStationModule> saveLoadStationModules = new List<SaveLoadStationModule>();
        for(int i=0; i < mainClass.Ceres.CountModules(); i++)
        {
            saveLoadStationModules.Add(new SaveLoadStationModule());
            mainClass.Ceres.GetSpaceStation(i).SaveData(saveLoadStationModules[i]);

            data.Add(JsonUtility.ToJson(saveLoadStationModules[i]));
        }
        File.WriteAllLines(path, data);

    }

    public void CanSave(string nameSave)
    {
        saveName.text = nameSave;
    }

    protected override void BuildElement(int id, string text)
    {
        RectTransform clone = Instantiate(element);
        clone.SetParent(scroll.content);
        clone.localScale = Vector3.one;
        clone.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
        SaveSlot item = clone.GetComponent<SaveSlot>();
        item.nameText.text = text.Substring(mainClass.SavePath.Length, text.Length - mainClass.SavePath.Length - 5);
        var fileInf = new FileInfo(text);
        item.dateText.text = $"{fileInf.CreationTime}";
        buttons.Add(clone);
    }

    protected override void UpdateList(int id)
    {
        
    }

    public void OpenSavePanel()
    {
        SavePanel.SetActive(true);
        List<string> filesName = new List<string>();
        filesName.AddRange(Directory.GetFiles(mainClass.SavePath));
        
        for (int i = 0; i < filesName.Count; i++)
        {
            if (filesName[i].Substring(filesName[i].Length - 4) == "JSON")
            {
                Create_Button(i, filesName[i]);
            }
        }
    }

    public void ConfirmSave()
    {
        if(saveName.text != "")
        {
            SaveData(saveName.text);
        }
        CloseSavePanel();
    }

    public void CloseSavePanel()
    {
        SavePanel.SetActive(false);
        ClearList();
    }
}
