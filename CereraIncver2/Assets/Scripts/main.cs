using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class main : MonoBehaviour
{   
    private DataBase dataBase;
    [SerializeField] private GameObject helloPanel, panelCreateGame;
    [SerializeField] private Text moneyText;
    [SerializeField] private InputField gameName;
    [SerializeField] private Button buttonX1, buttonX2, buttonX3, buttonPause;

    private Button prevBut, prevButBeforePause;
    public GameObject HelloPanel { get { return helloPanel; } }
    public float Day { get; set; }
    private float dayBeforeArrival;
    public float DayBeforeArrival { get { return dayBeforeArrival; } set { dayBeforeArrival = value; } }
    public DataBase DB { get { return dataBase; } }
    public string SavePath { get; set; }

    private bool isInputField;
    
    
    //Создаем экземпляры класс земля 
    [SerializeField] private Earth earth;
    public Earth Earth { get { return earth; } }
    //Массив корпораций
    private List<Inc> incs = new List<Inc>();
    [SerializeField] private Market market;
    [SerializeField] private PanelStation stations;
    [SerializeField] private GUI gUI;
    [SerializeField] private WarehousePanel warehouse;
    [SerializeField] private PanelOrder panelOrder;
    [SerializeField] private ScienseForPlayer sciense;
    [SerializeField] private PanelShip panelShip;
    [SerializeField] private LoadGame loadGame;
    [SerializeField] private Ceres ceres;
    [SerializeField] private MainResPanel resPanel;
    [SerializeField] private AsteroidCenter asteroidCenter;
    private Materials materials;
    [SerializeField] private Corporates corporates;
    [SerializeField] private CorpPanel corpPanel;
    [SerializeField] private PanelWithElements panelWithElements;
    [SerializeField] private EventPanel eventPanel;

    public LoadGame LoadGame { get { return loadGame; } }
    public PanelShip PanelShip { get { return panelShip; } }
    public GUI GUI { get { return gUI; } }
    public Market Market { get { return market; } }
    public WarehousePanel Warehouse { get { return warehouse; } }
    public PanelStation Station { get { return stations; } }
    public PanelOrder PanelOrder { get { return panelOrder; } }
    public ScienseForPlayer Sciense { get { return sciense; } }
    public Ceres Ceres { get { return ceres; } }
    public MainResPanel ResPanel { get { return resPanel; } }
    public Inc Player { get { return incs[0]; } }
    public bool IsPaused { get; set; }
    public float CeresTime { get; set; }
    public int TimeSpeed { get; set; }
    public bool GameIsStarted { set; get; }
    public bool SeasonIsCalculated { get; set; }
    public AsteroidCenter Asteroids { get { return asteroidCenter; } }
    public Materials Materials { get { return materials; } }
    public Corporates Corporates { get { return corporates; } }
    public CorpPanel CorpPanel { get { return corpPanel; } }
    public EventPanel EventPanel { get { return eventPanel; } }

    // Start is called before the first frame update
    void Start()
    {
        IsPaused = true;
        TimeSpeed = 1;
        dataBase = new DataBase(this);
        materials = new Materials();
        prevBut = buttonPause;
        prevButBeforePause = buttonX1;
        incs.Add(new Inc("player", 1000000));
    }

    public void CreateNewGame(int id=0)
    {
        if(id == 0)
        {
            helloPanel.SetActive(false);
            panelCreateGame.SetActive(true);
        }
        else
        {
            if(gameName.text != "")
            {
                SavePath = $"{Application.dataPath}/SaveGames/{gameName.text}/";
                int k = 0;
                while (Directory.Exists(SavePath))
                {
                    SavePath = $"{Application.dataPath}/SaveGames/{gameName.text}{k}/";
                    k = k + 1;
                }
                Directory.CreateDirectory(SavePath);
                StartNewGame();
            }
            
        }

    }
    
    public void StartNewGame()
    {
        prevButBeforePause = buttonX1;
        dayBeforeArrival = 460;        
        Player.PlusCarcassPas(0, 1);
        Player.PlusCarcassCargo(0, 1);
        Player.PlusFuelTank(0, 2);
        Player.PlusEngine(0, 2);
        ceres.CreateStation();
        Earth.StartGame(true);
        Earth.CalculationSeason();
        Asteroids.CreateAsteroids(250);
        market.CalculateSeason();
        Sciense.StartGame();
        //Создаем базу данных

        string path = SavePath + "Ceres.db";        
        
        //DB.SaveInSql();
        UpdateText();
        gUI.StartNewGame();
        panelCreateGame.SetActive(false);
        corporates.CreateCorporates(true);
        CreatePanels();
        dataBase.CreateDB(path);

        GameIsStarted = true;
    } 

    public void CreatePanels()
    {
        resPanel.UpdateText(1, $"${Player.Money}");
        resPanel.UpdateText(2, $"{Ceres.FreeWorkers}");
        resPanel.UpdateText(3, $"{Player.Equipment}");
        resPanel.UpdateText(4, $"{Player.Food}");
        resPanel.UpdateText(5, $"{Ceres.FreeEnergy}");

        panelWithElements.CreateList();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameIsStarted && !isInputField) {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                SetPause();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SetSpeedX1();
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetSpeedX2();
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetSpeedX3();
            }
        }
        

        if (!IsPaused)
        {
            CeresTime += Time.deltaTime * TimeSpeed;
            Day += Time.deltaTime * TimeSpeed;
            dayBeforeArrival -= Time.deltaTime * TimeSpeed;
            if(GUI.DateTime.AddDays(CeresTime).Day > 1)
            {
                SeasonIsCalculated = false;
            }
            if (GUI.DateTime.AddDays(CeresTime).Day == 1 && !SeasonIsCalculated)
            {
                Day = 0;
                SeasonIsCalculated = true;
                Earth.CalculationSeason();
                market.CalculateSeason();
                Materials.CleanExcavatedLastMonth();
                Asteroids.CalculationMonth();
                Corporates.NewRound();
                DB.SaveInSql();
                stations.UpdateText();
                warehouse.UpdateText();
                sciense.CalculationMonth();
                ceres.CalculationSeason();
                EventPanel.CreateEvents();
                EventPanel.EventHappen();
                UpdateText();
            }
            if(460 - dayBeforeArrival >= 460)
            {
                dayBeforeArrival = 460;
                //gUI.pressedMainButton(5);
                warehouse.ArrivalFromEarth();
                panelOrder.Arrival();
                Corporates.Arrival();
            }
        
        }

    }

    public void UpdateText()
    {
        moneyText.text = $"{incs[0].Money}";
        resPanel.UpdateText(1, $"${Player.Money}");
        resPanel.UpdateText(2, $"{Ceres.FreeWorkers}");
        resPanel.UpdateText(3, $"{Player.Equipment}");
        resPanel.UpdateText(4, $"{Player.Food}");
        resPanel.UpdateText(5, $"{Ceres.FreeEnergy}");
    }
    
    public int GenerateRandomInt(int min, int max)
    {
        return UnityEngine.Random.Range(min, max);
    }

    public void SetSpeedX1()
    {
        TimeSpeed = 1;
        IsPaused = false;
        ChangeColor(buttonX1, prevBut);
        prevBut = buttonX1;
    }
    public void SetSpeedX2()
    {
        TimeSpeed = 2;
        IsPaused = false;
        ChangeColor(buttonX2, prevBut);
        prevBut = buttonX2;
    }
    public void SetSpeedX3()
    {
        TimeSpeed = 3;
        IsPaused = false;
        ChangeColor(buttonX3, prevBut);
        prevBut = buttonX3;
    }

    public void SetPause()
    {
        IsPaused = !IsPaused;
        if (IsPaused)
        {
            ChangeColor(buttonPause, prevBut);
            prevButBeforePause = prevBut;
            prevBut = buttonPause;
        }
        else
        {
            prevButBeforePause.onClick.Invoke();
        }
    }

    private void ChangeColor(Button but, Button prevB)
    {
        ColorBlock theColor = but.colors;
        theColor.normalColor = Color.white;
        but.colors = theColor;

        if(but != prevB)
        {            
            theColor.normalColor = new Color32(78, 109, 177, 255);
            prevB.colors = theColor;
        }        
    }

    public void TypingInInputField()
    {
        isInputField = true;
    }

    public void StopTypingInInputField()
    {
        isInputField = false;
    }
}
