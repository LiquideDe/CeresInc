using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class PanelShip : Spisok
{
    public main mainClass;

    private int idChosenShip;

    //далее другие кнопки
    [SerializeField]
    private GUI gui;
    [SerializeField] private Text textNamePr, textPropShip;
    
    public GameObject panelShip, buttonCreateRoute;
    private Ship ship;
    private List<GameObject> ships = new List<GameObject>();
    public bool FlagCreateRoute { get; set; }
    protected override void BuildElement(int id, string text) // создание нового элемента и настройка параметров
    {
        RectTransform clone = Instantiate(element) as RectTransform;
        clone.SetParent(scroll.content);
        clone.localScale = Vector3.one;
        clone.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
        ShipButton item = clone.GetComponent<ShipButton>();
        item.nameShip.text = text;
        item.id = id;
        buttons.Add(clone);
    }

    public void ButtonPressed(int id)
    {
        if (!buttonCreateRoute.activeSelf)
            buttonCreateRoute.SetActive(true);
        idChosenShip = id;
        ship = ships[id].GetComponent<Ship>();
        UpdateText(id);
    }

    public void ShipPressed(int id)
    {
        gui.pressedMainButton(2);
        idChosenShip = id;
        ship = ships[id].GetComponent<Ship>();
        UpdateText(id);
    }


    public void UpdateText(int id)
    {
        if (id == idChosenShip && ship != null)
        {
            textNamePr.text = $"Корабль {ship.ShipName}";
            textPropShip.text = $"ISP = {ship.Isp} \n" +
                $"dV = {ship.Navigator.DV} \n" +
                $"Полезная нагрузка {ship.WeightPlayload}\n" +
                $"Снаряженная масса {ship.WeightContruction + ship.MaxWeightFuel} кг \n";
            if (ship.TypeShip == 0)
            {
                var pasShip = ship.GetComponent<PassangerShip>();
                textPropShip.text += $"Корабль везет: \n {pasShip.CargoWorkers} рабочих, \n {pasShip.CargoEquipment} кг оборудования \n {pasShip.CargoFood} кг еды";
            }
            else
            {
                var cargoShip = ship.GetComponent<CargoShip>();
                textPropShip.text += $"Корабль везет: \n";
                for (int i = 0; i < mainClass.Materials.MaterialsCount() - 1; i++)
                {
                    if (cargoShip.GetCargoElement(i) > 0)
                    {
                        textPropShip.text += $"{mainClass.Materials.GetMaterial(i).ElementName} - {cargoShip.GetCargoElement(i)} кг \n";
                    }
                }
            }
            if (ship.Navigator.IsInJourney)
            {
                textPropShip.text += $"\n Корабль в полете к {ship.Navigator.NearDestination().Asteroid.AsterName}, вернется на Цереру примерно через {ship.Navigator.TimeToJourney} дней";
            }
            else 
            {
                textPropShip.text += $"\n Корабль на приколе";
            }
        }
        
        
    }

    public void ClosePanelShip()
    {
        panelShip.SetActive(false);
    }

    public void CreateRoute()
    {
        //Создаем условия для построения маршрута
        //ставим флаг, что строим маршрут, чтобы срабатовал скрипт нажатия ПКМ на астероиде
        FlagCreateRoute = true;
        //Прячем все астероиды
        for (int i = 0; i < mainClass.Asteroids.AsteroidsCount(); i++)
        {
            mainClass.Asteroids.GetAsteroid(i).gameObject.SetActive(false);

        }
        //ПОказываем только те, что куплены
        for (int i = 0; i < mainClass.Player.CountAsteroids(); i++)
        {
            mainClass.Player.GetAsteroid(i).AsteroidGameObject.SetActive(true);
        }
        //Включаем фильтры для карты
        //togglesElements.SetActive(true);        
        ship.OpenDestinationPanel();
    }

    public void ChooseAsteroidForShip(AsteroidForPlayer aster)
    {
        ship.ChooseDestination(aster);
    }

    public void CloseCreateRoute()
    {
        FlagCreateRoute = false;
        for (int i = 0; i < mainClass.Asteroids.AsteroidsCount(); i++)
        {
            mainClass.Asteroids.GetAsteroid(i).gameObject.SetActive(true);

        }
        //togglesElements.SetActive(false);
        
    }

    public void DestroyShip()
    {
        
    }

    protected override void UpdateList(int id)
    {
        
    }

    public GameObject GetShip(int id)
    {
        return ships[id];
    }

    public void NewShip(GameObject ship)
    {
        ships.Add(ship);
    }

    public int ShipCount()
    {
        return ships.Count;
    }

    public GameObject GetLastShip()
    {
        return ships.Last();
    }

    public RectTransform GetLastButton()
    {
        return buttons.Last();
    }
}

