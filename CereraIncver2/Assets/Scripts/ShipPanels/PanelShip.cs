using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class PanelShip : Spisok
{
    public main mainClass;

    private int idChosenShip;

    //����� ������ ������
    [SerializeField]
    private GUI gui;
    [SerializeField] private Text textNamePr, textPropShip;
    
    public GameObject panelShip, buttonCreateRoute;
    private Ship ship;
    private List<GameObject> ships = new List<GameObject>();
    public bool FlagCreateRoute { get; set; }
    protected override void BuildElement(int id, string text) // �������� ������ �������� � ��������� ����������
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
            textNamePr.text = $"������� {ship.ShipName}";
            textPropShip.text = $"ISP = {ship.Isp} \n" +
                $"dV = {ship.Navigator.DV} \n" +
                $"�������� �������� {ship.WeightPlayload}\n" +
                $"����������� ����� {ship.WeightContruction + ship.MaxWeightFuel} �� \n";
            if (ship.TypeShip == 0)
            {
                var pasShip = ship.GetComponent<PassangerShip>();
                textPropShip.text += $"������� �����: \n {pasShip.CargoWorkers} �������, \n {pasShip.CargoEquipment} �� ������������ \n {pasShip.CargoFood} �� ���";
            }
            else
            {
                var cargoShip = ship.GetComponent<CargoShip>();
                textPropShip.text += $"������� �����: \n";
                for (int i = 0; i < mainClass.Materials.MaterialsCount() - 1; i++)
                {
                    if (cargoShip.GetCargoElement(i) > 0)
                    {
                        textPropShip.text += $"{mainClass.Materials.GetMaterial(i).ElementName} - {cargoShip.GetCargoElement(i)} �� \n";
                    }
                }
            }
            if (ship.Navigator.IsInJourney)
            {
                textPropShip.text += $"\n ������� � ������ � {ship.Navigator.NearDestination().Asteroid.AsterName}, �������� �� ������ �������� ����� {ship.Navigator.TimeToJourney} ����";
            }
            else 
            {
                textPropShip.text += $"\n ������� �� �������";
            }
        }
        
        
    }

    public void ClosePanelShip()
    {
        panelShip.SetActive(false);
    }

    public void CreateRoute()
    {
        //������� ������� ��� ���������� ��������
        //������ ����, ��� ������ �������, ����� ���������� ������ ������� ��� �� ���������
        FlagCreateRoute = true;
        //������ ��� ���������
        for (int i = 0; i < mainClass.Asteroids.AsteroidsCount(); i++)
        {
            mainClass.Asteroids.GetAsteroid(i).gameObject.SetActive(false);

        }
        //���������� ������ ��, ��� �������
        for (int i = 0; i < mainClass.Player.CountAsteroids(); i++)
        {
            mainClass.Player.GetAsteroid(i).AsteroidGameObject.SetActive(true);
        }
        //�������� ������� ��� �����
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

