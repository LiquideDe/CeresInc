using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShipButton : MonoBehaviour, IPointerClickHandler
{
	public int id; // идентификатор объекта, который храниться в списке
	public PanelShip panelShip;
	public Text nameShip, typeShip, dV, mas, destination, timeToReturn, age; // текст главной кнопки
	public GameObject red, yellow, green, greyRed, greyYellow, greyGreen;
	public ToggleForShip toggleRepeat;

	public void OnPointerClick(PointerEventData eventData)
	{
		panelShip.ButtonPressed(id);
	}

	public void UpdateText(int dv, float mas, float cost, string destination, float timeToReturn, int age)
    {
		dV.text = $"dV = {dv} м/с";
		this.mas.text = $"Mas = {mas} кг.";
		this.timeToReturn.text = $"Вернется через {timeToReturn} дней";
		this.age.text = $"Возраст - {age} г.";
    }

	public void StandAtCeres()
    {
		red.SetActive(true);
		greyRed.SetActive(false);
		yellow.SetActive(false);
		greyYellow.SetActive(true);
		green.SetActive(false);
		greyGreen.SetActive(true);
	}

	public void Docking()
    {
		red.SetActive(false);
		greyRed.SetActive(true);
		yellow.SetActive(true);
		greyYellow.SetActive(false);
		green.SetActive(false);
		greyGreen.SetActive(true);
	}

	public void AtJourney()
    {
		red.SetActive(false);
		greyRed.SetActive(true);
		yellow.SetActive(false);
		greyYellow.SetActive(true);
		green.SetActive(true);
		greyGreen.SetActive(false);
	}
}
