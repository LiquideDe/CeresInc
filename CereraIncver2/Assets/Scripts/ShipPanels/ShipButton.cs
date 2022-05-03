using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShipButton : MonoBehaviour, IPointerClickHandler
{
	public int id; // ������������� �������, ������� ��������� � ������
	public PanelShip panelShip;
	public Text nameShip, typeShip, dV, mas, destination, timeToReturn, age; // ����� ������� ������
	public GameObject red, yellow, green, greyRed, greyYellow, greyGreen;
	public ToggleForShip toggleStart, toggleRepeat;

	public void OnPointerClick(PointerEventData eventData)
	{
		panelShip.ButtonPressed(id);
	}

	public void UpdateText(int dv, float mas, float cost, string destination, float timeToReturn, int age)
    {
		dV.text = $"dV = {dv} �/�";
		this.mas.text = $"Mas = {mas} ��.";
		this.destination.text = $"����� � {destination}";
		this.timeToReturn.text = $"�������� ����� {timeToReturn} ����";
		this.age.text = $"������� - {age} �.";
    }

	public void StandAtCeres()
    {
		red.SetActive(true);
		greyRed.SetActive(false);
		yellow.SetActive(false);
		greyYellow.SetActive(true);
		green.SetActive(false);
		greyGreen.SetActive(true);
		toggleStart.UnPushingRed();
		toggleStart.AnotherToggle.UnPushingRed();

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
