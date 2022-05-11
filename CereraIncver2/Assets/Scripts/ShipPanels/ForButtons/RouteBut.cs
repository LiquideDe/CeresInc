using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RouteBut : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Text textName, textAsteroids, textDistance, textAmountShips;
    [SerializeField] ShipRoutes routes;
    public int Id { get; set; }
    public Route Route { get; set; }
    

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"��������� {Id} �������");
        routes.MakeChangesAtRoute(Id);
    }

    public void UpdateText()
    {
        textName.text = Route.NameRoute;
        textAsteroids.text = $" ����������{Route.CountDestination()} ��.";
        textDistance.text = $"��������� {Route.Distance} ��";
        textAmountShips.text = $"�������� {Route.AmountShipsOnRoute} ��.";
    }
}
