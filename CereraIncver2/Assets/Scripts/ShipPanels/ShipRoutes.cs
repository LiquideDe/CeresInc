using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipRoutes : Spisok
{
    [SerializeField] Text textDescription;
    [SerializeField] InputField textName;
    [SerializeField] main mainClass;
    [SerializeField] private LineRenderer line;
    [SerializeField] private ListRoutes listRoutes;
    private List<Route> routes = new List<Route>();
    private bool createRoute;
    private Route route;    

    protected override void BuildElement(int id, string text)
    {
        RectTransform clone = Instantiate(element);
        clone.SetParent(scroll.content);
        clone.localScale = Vector3.one;
        clone.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
        ResButton item = clone.GetComponent<ResButton>();
        item.Asteroid = mainClass.Asteroids.GetAsteroid(id);
        item.UpdateText();
        buttons.Add(clone);
    }

    protected override void UpdateList(int id)
    {
        vPos = scroll.verticalNormalizedPosition; // ���������� ������� �������

        Destroy(buttons[id].gameObject); // ������� ���� ������� �� ������
        buttons.RemoveAt(id);
        curY = 0;
        foreach (RectTransform b in buttons) // �������� ��������
        {
            b.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
            curY += delta.y;
        }
        size--; // ����� ���� �������
        RectContent(); // ������������� ������� ����
        scroll.verticalNormalizedPosition = vPos; // ���������� ������� �������
    }

    public void DeleteResButton(int id)
    {
        UpdateList(id);
    }

    public void CreateNewRoute()
    {
        routes.Add(new Route(mainClass));
        createRoute = true;
        route = routes[routes.Count - 1];
        route.Id = routes.Count - 1;
        listRoutes.Create_Button(route.Id);
        textName.text = "";
    }

    public void CreateEmptyRoutes(int amount)
    {
        for(int  i = 0; i < amount; i++)
        {
            routes.Add(new Route(mainClass));
            listRoutes.Create_Button(i);
        }        
    }

    public void CloseCreateRoute()
    {
        createRoute = false;
        listRoutes.UpdateText();
        ClearList();
        line.positionCount = 0;

        if(route.CountDestination() == 0)
        {
            listRoutes.RemoveRoute(route.Id);
            routes.Remove(route);
        }
    }

    public void ChooseDestination(AsteroidForPlayer aster)
    {
        if (createRoute)
        {
            if (aster.MiningStation != null)
            {
                //�������� �������� �� �������� ����� � ��������� ��� � ������. ���� ������ ������, �� ������ ��������� ���� � ������ ����� � ������ ������� �������
                if (route.CountDestination() == 0)
                {
                    route.SetDestination(aster);
                    Create_Button(aster.Id);
                }
                //���������, ��� ����� ����� ������� ����� �������� �� ����� �� ��� � �������, ���� ����� ��, �� �������� ������� �� �� �������, ���� ������. �� ���������
                //����� ������������� � ����� ������
                else if (route.ContainMas(aster))
                {
                    DeleteResButton(route.GetIdDestination(aster));
                    route.RemoveDestination(aster);                    
                }
                else
                {
                    route.SetDestination(aster);
                    Create_Button(aster.Id);
                }
                DrawLines();
                CalculationTime();
            }
        }  
    }

    private void DrawLines()
    {
        line.positionCount = 0;
        if (route.CountDestination() <= 1)
        {
            line.positionCount = route.CountDestination() + 1;
        }
        else
        {
            line.positionCount = route.CountDestination() + 2;
        }
        line.SetPosition(0, new Vector3(0, -172, 0));
        for (int i = 0; i < route.CountDestination(); i++)
        {
            line.SetPosition(i, route.GetDestination(i).Position);
        }
        if (route.CountDestination() > 1)
        {
            line.loop = true;
        }
    }

    private void CalculationTime()
    {
        float allMass = mainClass.Sciense.GetNewestEngine().Weight + mainClass.Sciense.GetNewestFuelTank().Weight + mainClass.Sciense.GetNewestCarcass().Weight;
        float dVFree = (float)Math.Round(mainClass.Sciense.GetNewestEngine().Isp * Math.Log((allMass + mainClass.Sciense.GetNewestFuelTank().MaxFuel) / allMass), 0);
        float dvFull = (float)Math.Round(mainClass.Sciense.GetNewestEngine().Isp * Math.Log((allMass + mainClass.Sciense.GetNewestCarcass().MaxWeightPlayload + mainClass.Sciense.GetNewestFuelTank().MaxFuel) / (allMass + mainClass.Sciense.GetNewestCarcass().Weight)), 0);

        float dist = route.CalculateTotalLengthRoute();

        textDescription.text = $"������� �������� ���������� {dist} ��, ����� � ���� �������� �� {dist / (dVFree * 86.4f/1000)} ���� �� {dist / (dvFull * 86.4f/1000)} ����";
    }

    public void ChangeNameRoute()
    {
        route.NameRoute = textName.text;
    }

    public void MakeChangesAtRoute(int id)
    {
        createRoute = true;
        mainClass.GUI.CloseAll();
        gameObject.SetActive(true);
        route = routes[id];
        textName.text = route.NameRoute;
        for (int i = 0; i < route.CountDestination(); i++)
        {
            Create_Button(route.GetDestination(i).Id);
        }
        DrawLines();
        CalculationTime();
    }

    public Route GetRoute(int id)
    {
        if(id >= routes.Count)
        {
            return null;
        }
        else
        {
            return routes[id];
        }        
    }

    public int CountRoutes()
    {
        return routes.Count;
    }


}
