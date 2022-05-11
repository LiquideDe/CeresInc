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
        Debug.Log($"Открываем {Id} маршрут");
        routes.MakeChangesAtRoute(Id);
    }

    public void UpdateText()
    {
        textName.text = Route.NameRoute;
        textAsteroids.text = $" Астероидов{Route.CountDestination()} шт.";
        textDistance.text = $"Дистанция {Route.Distance} км";
        textAmountShips.text = $"Кораблей {Route.AmountShipsOnRoute} шт.";
    }
}
