using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI : MonoBehaviour
{
    public List<GameObject> panels = new List<GameObject>();
    public GameObject mainPanel, helloPanel, resPanel;
    private int prevButton;
    public Text timeAndDate;
    public main mainClass;
    public PanelShip panelShip;
    [SerializeField] private GameObject togglesElements, turnButtonOwn, menu;
    [SerializeField] private GameObject asteroidPanel;
    private bool flagShowOnlyOwnAster = false;
    public DateTime DateTime { get; set; }

    private void Start()
    {
        DateTime = new DateTime(2030, 1, 1);
    }
    // Update is called once per frame
    void Update()
    {
       if(!mainClass.IsPaused)
        {
            //timeAndDate.text = $"{DateTime.AddDays(mainClass.CeresTime).ToString("dd.MM.yyyy")}";
            timeAndDate.text = $"{(int)mainClass.CeresTime}";
        }

       if(mainClass.GameIsStarted && Input.GetKeyDown(KeyCode.Escape))
        {
            if (menu.activeSelf)
            {
                menu.SetActive(false);
            }
            else
            {
                mainClass.IsPaused = true;
                menu.SetActive(true);
            }
        }
    }

    public void pressedMainButton(int id)
    {
        if(prevButton == 80)
        {
            panels[id].SetActive(true);
            prevButton = id;
        }
        else if (id == 79)
        {
            panels[prevButton].SetActive(true);
        }
        else if(prevButton != id)
        {
            panels[prevButton].SetActive(false);
            panels[id].SetActive(true);
            prevButton = id;
        }
        else
        {
            panels[id].SetActive(false);
            prevButton = 80;
        }
    }

    public void CloseAsterPanel()
    {
        panels[8].SetActive(false);
    }

    public void StartNewGame()
    {
        //helloPanel.SetActive(false);
        mainPanel.SetActive(true);
        resPanel.SetActive(true);
        togglesElements.SetActive(true);
        turnButtonOwn.SetActive(true);
        StartCoroutine(Prohod());
        StartCoroutine(Prohod2());
    }

    public void LoadGame(string path)
    {
        mainPanel.SetActive(true);
        resPanel.SetActive(true);
        togglesElements.SetActive(true);
        turnButtonOwn.SetActive(true);
        StartCoroutine(Prohod());
        StartCoroutine(Prohod2());
        StartCoroutine(mainClass.LoadGame.LoadData(path));
    }

    IEnumerator Prohod()
    {
        //Запускаем все панели, чтобы прогрузить их скрипты
        for (int i = 0; i < panels.Capacity; i++)
        {
            panels[i].SetActive(true);
        }
        Debug.Log($"Раз");
        asteroidPanel.SetActive(true);
        yield return new WaitForEndOfFrame(); 
       
    }

    public IEnumerator Prohod2()
    {
        //запускаем скрипт после того как закончится предыдущая корутина
        yield return StartCoroutine(Prohod());
        for (int i = 0; i < panels.Capacity; i++)
        {
            panels[i].SetActive(false);
        }
        asteroidPanel.SetActive(false);
        yield return new WaitForEndOfFrame();
    }

    public void CloseAll()
    {
        for (int i=0; i < panels.Count; i++)
        {
            panels[i].SetActive(false);            
        }
        mainPanel.SetActive(false);
        resPanel.SetActive(false);

    }

    public void OpenAll()
    {
        mainPanel.SetActive(true);
        resPanel.SetActive(true);
        pressedMainButton(79);
        
    }

    public void ShowOrHideOwnAsteroid()
    {
        flagShowOnlyOwnAster = !flagShowOnlyOwnAster;
        for (int i = 0; i < mainClass.Asteroids.AsteroidsCount(); i++)
        {
            if(mainClass.Asteroids.GetAsteroid(i).MiningStation == null)
            {
                if(flagShowOnlyOwnAster)
                mainClass.Asteroids.GetAsteroid(i).OffAsteroid();
                else
                    mainClass.Asteroids.GetAsteroid(i).OnAsteroid();
                
            }
        }
    }

    public string GetDayMonthYear()
    {
        return DateTime.AddDays(mainClass.CeresTime).ToString("dd.MM.yyyy");
    }
}
