using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BuildShip : MonoBehaviour
{
    [SerializeField] private Dropdown dpDCarcas, dpDFuel, dpDEngine, dpdRoute;
    [SerializeField] private Text textOut, textBreak;
    [SerializeField] private GameObject passangerShip, cargoShip, panelBuildShip, panelShip, panelChooseType, buttonBuild;
    [SerializeField] private InputField nameNewShip;
    [SerializeField] private ScienseForPlayer sciense;
    [SerializeField] private Slider sliderDaysBreak;
    private int typeShip = 0;
    public main mainClass;
    Carcass carcass = null;
    Engine engine = null;
    FuelTank fuelTank = null;

    public void ChooseType()
    {
        panelChooseType.SetActive(true);
    }

    public void CloseChooseType()
    {
        panelChooseType.SetActive(false);
    }
    public void OpenBuildPanel(int typeShip)
    {
        panelChooseType.SetActive(false);
        panelBuildShip.SetActive(true);
        this.typeShip = typeShip;
        //Очищаем формы дропдаун
        dpDCarcas.ClearOptions();
        dpDFuel.ClearOptions();
        dpDEngine.ClearOptions();
        dpdRoute.ClearOptions();

        //Через три цикла заполняем каждую дпд, в зависимости от наличия деталей на складе
        if(typeShip == 0)
            for (int i = 0; i < mainClass.Player.CountCarcassPas(); i++)
            {
                Debug.Log($"Количество деталей {mainClass.Player.CountCarcassPas()} название технологии ");
                if(mainClass.Player.CountCarcassPas(i) != 0)
                dpDCarcas.options.Add(new Dropdown.OptionData(sciense.GetCarcass(i).NameTech));
            }
        else
            for (int i = 0; i < mainClass.Player.CountCarcassCargo(); i++)
            {
                if(mainClass.Player.CountCarcassCargo(i) != 0)
                dpDCarcas.options.Add(new Dropdown.OptionData(sciense.GetCarcass(i).NameTech));
            }

        for (int i = 0; i < mainClass.Player.CountFuelTank(); i++)
        {
            if (mainClass.Player.CountFuelTank(i) != 0)
                dpDFuel.options.Add(new Dropdown.OptionData(sciense.GetFuelTank(i).NameTech));
        }
        for (int i = 0; i < mainClass.Player.CountEngine(); i++)
        {
            if (mainClass.Player.CountEngine(i) != 0)
                dpDEngine.options.Add(new Dropdown.OptionData(sciense.GetEngine(i).NameTech));
        }

        for(int i = 0; i < mainClass.PanelShip.ShipRoutes.CountRoutes(); i++)
        {
            dpdRoute.options.Add(new Dropdown.OptionData(mainClass.PanelShip.ShipRoutes.GetRoute(i).NameRoute));
        }
        dpDCarcas.RefreshShownValue();
        dpDFuel.RefreshShownValue();
        dpDEngine.RefreshShownValue();
        dpdRoute.RefreshShownValue();

        UpdateTextOut();

        if (dpDCarcas.options.Count !=0 && dpDEngine.options.Count != 0 && dpDFuel.options.Count != 0)
        {
            buttonBuild.SetActive(true);
        }
        else
        {
            buttonBuild.SetActive(false);
        }

    }
    public void UpdateTextOut()
    {
        if (dpDCarcas.options.Count != 0 && dpDEngine.options.Count != 0 && dpDFuel.options.Count != 0)
        {
            carcass = sciense.GetCarcass(dpDCarcas.value);
            fuelTank = sciense.GetFuelTank(dpDFuel.value);
            engine = sciense.GetEngine(dpDEngine.value);
            var weight = carcass.Weight + fuelTank.Weight + engine.Weight;
            var isp = engine.Isp;
            //var finCost = sciense.GetCarcass(dpDCarcas.value).Cost + sciense.GetFuelTank(dpDFuel.value).Cost + sciense.GetEngine(dpDEngine.value).Cost;

            textOut.text = $"Корабль будет общим весом {weight}  кг, \n " +
                $"ISP = {isp} \n Тип топлива {mainClass.Materials.GetMaterial(sciense.GetEngine(dpDEngine.value).TypeFuel).ElementName} максимальная вместимость {sciense.GetFuelTank(dpDFuel.value).MaxFuel}";
        }
        else
        {
            textOut.text = $"Не хватает запчастей";
        }
        
    }

    public void FinalyBuildShip()
    {       
        var weight = carcass.Weight + fuelTank.Weight + engine.Weight;
        //var finCost = sciense.GetCarcass(dpDCarcas.value).Cost + sciense.GetFuelTank(dpDFuel.value).Cost + sciense.GetEngine(dpDEngine.value).Cost;
        bool isCreated = false;
        for (int i = 0; i < mainClass.PanelShip.ShipCount(); i++)
        {
            if (mainClass.PanelShip.GetShip(i).GetComponent<Ship>().ShipName == nameNewShip.text)
                isCreated = true;
        }
        if (nameNewShip.text != "" && !isCreated && mainClass.Ceres.GetFreeDock() != null)
        {
            if (typeShip == 0)
            {
                mainClass.PanelShip.NewShip(Instantiate(passangerShip));
            }
            else { 
                mainClass.PanelShip.NewShip(Instantiate(cargoShip));
            }
            var ship = mainClass.PanelShip.GetLastShip().GetComponent<Ship>();
            ship.Id = mainClass.PanelShip.ShipCount() - 1;
            ship.NewShip(nameNewShip.text, typeShip, carcass, engine, fuelTank, mainClass.Player);            
            mainClass.PanelShip.GetLastShip().SetActive(true);
            AddToListButton(ship.Id, nameNewShip.text, ship);
            var dock = mainClass.Ceres.GetFreeDock();
            ship.Rigidb.rotation = dock.transform.rotation;
            ship.Rigidb.position = new Vector3(dock.transform.position.x, dock.transform.position.y + Vector3.Distance(ship.Rigidb.position, ship.ShipsDock.position), dock.transform.position.z);
            dock.Docking(ship);
            ship.Dock = dock;
            ship.Navigator.IsDocked = true;
            //mainClass.incs[0].Money -= finCost;
            if (typeShip == 0)
            {
                mainClass.Player.PlusCarcassPas(carcass.Id, -1);                
            }
            else
            {
                mainClass.Player.PlusCarcassCargo(carcass.Id, -1);
            }                
            mainClass.Player.PlusEngine(engine.Id, -1);
            mainClass.Player.PlusFuelTank(fuelTank.Id, -1);
            mainClass.Warehouse.UpdateText();
            if(mainClass.ShipRoutes.GetRoute(dpdRoute.value) != null)
            {
                ship.Navigator.SetRoute(mainClass.ShipRoutes.GetRoute(dpdRoute.value), (int)sliderDaysBreak.value);
                Debug.Log($"Поставили маршрут");
            }
            CloseBuildPanel();
        }

    }

    private void AddToListButton(int id, string name, Ship ship)
    {
        mainClass.PanelShip.Create_Button(id, name);

        var but = mainClass.PanelShip.GetLastButton().GetComponent<ShipButton>();
        but.nameShip.text = nameNewShip.text;
        if(typeShip == 0)
        {
            but.typeShip.text = $"Пассажирский";
        }
        else
        {
            but.typeShip.text = "Грузовой";
        }

        but.age.text = $"0";
        but.destination.text = $"Маршрут номер {ship.Navigator.IdRoute}";
        but.dV.text = "";
        but.id = ship.Id;
        but.mas.text = $"Mas - {ship.WeightContruction} кг";
        but.timeToReturn.text = $"";
        but.toggleRepeat.SetShip(ship);

        ship.ShipButton = but;
    }

    public void CloseBuildPanel()
    {
        panelShip.SetActive(true);
        panelBuildShip.SetActive(false);
    }

    public void BuildEmptyShip(int type, SaveLoadShip save)
    {
        if (type == 0)
        {
            mainClass.PanelShip.NewShip(Instantiate(passangerShip));
        }
        else
        {
            mainClass.PanelShip.NewShip(Instantiate(cargoShip));
        }
        Ship ship = mainClass.PanelShip.GetLastShip().GetComponent<Ship>();
        ship.gameObject.SetActive(true);
        ship.LoadData(save);
        AddToListButton(save.id, save.shipName, ship);
    }

    public void ChangeSlider()
    {
        textBreak.text = $"Перерыв между полетами {sliderDaysBreak.value} дней";
    }
}
