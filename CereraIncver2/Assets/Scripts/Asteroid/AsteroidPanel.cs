using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AsteroidPanel : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private GameObject asterPanel,  buttonFoundColony,  plusWorker, minusWorker;
    private GameObject asterCircle;
    [SerializeField] private Text asterName, res1, incomeRes1, textSupply, workersAmount;
    public main mainClass;
    //[SerializeField] private LineRenderer line;
    [SerializeField] private Canvas canvas;
    private AsteroidForPlayer asteroid;
    [SerializeField] private PanelStation panelStation;
    [SerializeField] private LineRenderer line;
    [SerializeField] private GameObject prefabMiningStation;

    public void OpenPanel(AsteroidForPlayer asteroid)
    {
        ClosePanel();
        if(asterCircle)
            asterCircle.SetActive(false);
        this.asteroid = asteroid;
        asterCircle = asteroid.Circle;

        asterPanel.SetActive(true);
        asterCircle.SetActive(true);
        
        
        line.positionCount = 2;
        line.widthMultiplier = 2;
        line.SetPosition(0, asterCircle.transform.position);
        line.SetPosition(1, asterPanel.transform.position);
        if (asteroid.MiningStation == null)
        {
            buttonFoundColony.SetActive(true);
            ShowEmptyAsteroid();
        }
        else
        {
            buttonFoundColony.SetActive(false);
            ShowOwnedAsteroid();
        }
            
    }

    private void ShowEmptyAsteroid()
    {
        asterName.text = $"Астероид {asteroid.AsterName}, расстояние от Цереры - {asteroid.Distance}";
        res1.text = $"Ресурс {asteroid.Element.ElementName}, запасы в {asteroid.ElementCapacity} т., концентрация {Math.Round(asteroid.ElementAbundance * 100, 2)}%";
    }

    public void ClosePanel()
    {
        asterPanel.SetActive(false);
        if(asterCircle)
        asterCircle.SetActive(false);
        workersAmount.enabled = false;
        incomeRes1.enabled = false;
        plusWorker.SetActive(false);
        minusWorker.SetActive(false);
        textSupply.enabled = false;

        line.positionCount = 0;
    }

    public void OnMouseDrag()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log($"Проверка 2");
    }

    public void OnDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
        asterPanel.GetComponent<RectTransform>().anchoredPosition += eventData.delta / canvas.scaleFactor;
        line.SetPosition(1, asterPanel.transform.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //throw new System.NotImplementedException();
    }

    public void FoundColony()
    {

        GameObject station = Instantiate(prefabMiningStation);
        station.SetActive(true);
        station.transform.SetParent(asteroid.transform);
        station.transform.position = new Vector3(asteroid.transform.position.x, -10, asteroid.transform.position.z);
        MiningStation mstation = station.GetComponent<MiningStation>();
        asteroid.MiningStation = mstation;
        asteroid.HasMiningStation = true;
        mstation.Asteroid = asteroid;
        buttonFoundColony.SetActive(false);
        ShowOwnedAsteroid();
        mainClass.Player.PlusAsteroid(asteroid);
        //panelStation.Create_Button(asteroid.Id, asteroid.AsterName);
    }    

    private void ShowOwnedAsteroid()
    {
        asterName.text = $"Астероид {asteroid.AsterName}, расстояние от Цереры - {asteroid.Distance}";
        res1.text = $"Ресурс {asteroid.Element.ElementName}, запасы в {asteroid.ElementCapacity} т., концентрация {Math.Round(asteroid.ElementAbundance * 100, 2)}";

        //asteroid.workersOnAster = 15;

        workersAmount.enabled = true;
        incomeRes1.enabled = true;
        plusWorker.SetActive(true);
        minusWorker.SetActive(true);
        textSupply.enabled = true;
        UpdateText();

    }

    public void UpdateText()
    {
        //Пишем данные в графы
        workersAmount.text = $"{asteroid.MiningStation.WorkersOnStation}/{asteroid.MiningStation.WorkersPlanned}";
        incomeRes1.text = $"{asteroid.MiningStation.WorkersOnStation} рабочих добыли {asteroid.MiningStation.ExcavatedSoil} кг";
        textSupply.text = $"Всего запасов еды {asteroid.MiningStation.Food}, хватит на {asteroid.MiningStation.DayOffFood}, запасов оборудование {asteroid.MiningStation.Equipment}, хватит на {asteroid.MiningStation.DayOffEquipment}";
    }

    public void PlusMinusWorkers(int value)
    {
        if (value == 0 && asteroid.MiningStation.WorkersOnStation >= asteroid.MiningStation.WorkersPlanned && asteroid.MiningStation.WorkersOnStation > 0)
        {
            asteroid.MiningStation.AwaitingWorkers += 1;
            asteroid.MiningStation.WorkersPlanned -= 1;
        }
        else if (value == 0 && asteroid.MiningStation.WorkersOnStation < asteroid.MiningStation.WorkersPlanned && mainClass.Ceres.WorkersAwaiting > 0)
        {
            asteroid.MiningStation.WorkersPlanned -= 1;
            mainClass.Ceres.FreeWorkers += 1;
            mainClass.Ceres.WorkersAwaiting -= 1;
            
        }
        else if (value != 0 && asteroid.MiningStation.AwaitingWorkers > 0)
        {
            asteroid.MiningStation.AwaitingWorkers -= 1;
            asteroid.MiningStation.WorkersPlanned += 1;
        }
        else if(value != 0 && asteroid.MiningStation.AwaitingWorkers == 0 && mainClass.Ceres.FreeWorkers > 0)
        {
            asteroid.MiningStation.WorkersPlanned += 1;
            mainClass.Ceres.FreeWorkers -= 1;
            mainClass.Ceres.WorkersAwaiting += 1;
        }
        asteroid.MiningStation.CalculateSupplyConsuption();
        UpdateText();        
    }

    public void CreateEmptyColony(AsteroidForPlayer asteroidFor)
    {
        asteroid = asteroidFor;
        FoundColony();
    }
}
