using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelStation : Spisok
{
    public main mainClass;

    protected override void BuildElement(int id, string text) // �������� ������ �������� � ��������� ����������
    {
        RectTransform clone = Instantiate(element);
        clone.SetParent(scroll.content);
        clone.localScale = Vector3.one;
        clone.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
        StationButton item = clone.GetComponent<StationButton>();
        item.id = id;
        item.textName.text = text;
        item.textWorker.text = $"������� \n {mainClass.Asteroids.GetAsteroid(id).MiningStation.WorkersOnStation}/{mainClass.Asteroids.GetAsteroid(id).MiningStation.WorkersPlanned}";
        item.textFood.text = $"��� \n {mainClass.Asteroids.GetAsteroid(id).MiningStation.Food}";
        item.textEquipment.text = $"������������ \n {mainClass.Asteroids.GetAsteroid(id).MiningStation.Equipment}";
        item.textClean.text = $"��������� ������ \n {mainClass.Asteroids.GetAsteroid(id).ElementAbundance}";
        item.textIncome.text = $"�������� � ���� \n {mainClass.Asteroids.GetAsteroid(id).MiningStation.IncomeLastMonth}";
        item.textExcavated.text = $"����� ������ \n {mainClass.Asteroids.GetAsteroid(id).MiningStation.ExcavatedSoil}";
        item.textReady.text = $"������ � �������� \n {mainClass.Asteroids.GetAsteroid(id).MiningStation.AmountReadyForLoading}";
        item.textAmount.text = $"����� �������� ������� \n {mainClass.Asteroids.GetAsteroid(id).ElementCapacity}";

        buttons.Add(clone);
    }

    public void UpdateText()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            StationButton item = buttons[i].GetComponent<StationButton>();
            item.textWorker.text = $"������� \n {mainClass.Asteroids.GetAsteroid(item.id).MiningStation.WorkersOnStation}/{mainClass.Asteroids.GetAsteroid(item.id).MiningStation.WorkersPlanned}";
            item.textFood.text = $"��� \n {mainClass.Asteroids.GetAsteroid(item.id).MiningStation.Food}";
            item.textEquipment.text = $"������������ \n {mainClass.Asteroids.GetAsteroid(item.id).MiningStation.Equipment}";
            item.textIncome.text = $"������ �� ������� ����� \n {mainClass.Asteroids.GetAsteroid(item.id).MiningStation.IncomeLastMonth}";
            item.textExcavated.text = $"����� ������ \n {mainClass.Asteroids.GetAsteroid(item.id).MiningStation.ExcavatedSoil}";
            item.textReady.text = $"������ � �������� \n {mainClass.Asteroids.GetAsteroid(item.id).MiningStation.AmountReadyForLoading}";
            item.textAmount.text = $"����� �������� ������� \n {mainClass.Asteroids.GetAsteroid(item.id).ElementCapacity}";
        }
    }

    public void CloseStationPanel()
    {
        transform.gameObject.SetActive(false);
    }

    protected override void UpdateList(int id)
    {
        throw new System.NotImplementedException();
    }

}
