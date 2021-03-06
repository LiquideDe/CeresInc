using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EventPanel : MonoBehaviour, IPointerDownHandler
{
    private List<StoryEvent> events = new List<StoryEvent>();
    [SerializeField] private Text textNews;
    [SerializeField] main mainClass;
    [SerializeField] private GameObject panel;
    [SerializeField] private NewsPanel newsPanel;
    public main Main { get { return mainClass; } }
    public Text TextNews { get { return textNews; } }
    private bool isShowNews;

    public void SetNews(string news)
    {
        textNews.text += news;
        
    }

    public void CreateEvents()
    {
        TextNews.text = "";
        int rollOfDiceFirst = Main.GenerateRandomInt(1, 20);
        int rollOfDiceSecond = Main.GenerateRandomInt(1, 20);
        int rollOfDiceThird = Main.GenerateRandomInt(1, 20);
        int[] masRoll = new int[] { rollOfDiceFirst, rollOfDiceSecond, rollOfDiceThird, rollOfDiceFirst, rollOfDiceSecond, rollOfDiceThird };
        int summ = GetFirstNumber(rollOfDiceFirst) + GetFirstNumber(rollOfDiceSecond) + GetFirstNumber(rollOfDiceThird);
        for(int i=0; i < summ; i++)
        {
            CreateNewEvent(masRoll[i]);            
        }
                
    }

    private int GetFirstNumber(int number)
    {
        float answ = (number / 10);
        if(answ < 1)
        {
            answ = 0;
        }
        else if(answ < 2)
        {
            answ = 1;
        }
        else
        {
            answ = 2;
        }
        return (int)answ;
    }

    private void CreateNewEvent(int rollOfDice)
    {
        float strengthOfDown = Main.GenerateRandomInt(1, 15);
        strengthOfDown /= 100;
        int duration = Main.GenerateRandomInt(1, 3);
        duration *= 30;
        int timeStart = (int)Main.CeresTime;
        int materialId = Main.GenerateRandomInt(1, Main.Materials.MaterialsCount() - 1);
        materialId -= 1;        

        switch (rollOfDice)
        {
            case 0:
            case 1:
            case 2:
            case 3:                
                events.Add(new CloseMine(materialId, 0, strengthOfDown, timeStart, this));
                break;

            case 4:
            case 5:
            case 6:
            case 7:
                events.Add(new AccidentAtEarthMaterial(materialId,duration, strengthOfDown,timeStart, this));
                break;

            case 8:
            case 9:
            case 10:
            case 11:
                int idCorp = Main.GenerateRandomInt(2, 5);
                EarthCorp earthCorp = GetCorpById(idCorp);
                int idBuilding = Main.GenerateRandomInt(1, earthCorp.CountBuildings());
                events.Add(new AccidentAtEarthBuilding(earthCorp, earthCorp.GetBuilding(idBuilding-1), duration, strengthOfDown, timeStart, this));
                break;

            case 15:
                events.Add(new AccidentAtWarehouse(materialId, 0, strengthOfDown, timeStart, this));
                break;

            case 19:
                var accident = GenerateMiningStationAccident();
                if(accident != null)
                {
                    events.Add(accident);
                }
                break;
            case 20:
                events.Add(new FindNewMine(materialId, 0, strengthOfDown, timeStart, this));
                break;
        }
    }

    private EarthCorp GetCorpById(int id)
    {
        switch (id)
        {
            case 1:
                return Main.Earth.Energy;
            case 2:
                return Main.Earth.HeavyIndustry;
            case 3:
                return Main.Earth.LightIndustry;
            case 4:
                return Main.Earth.HighIndustry;
            case 5:
                return Main.Earth.SpaceIndustry;
            default:
                return null;
        }
    }

    private AccidentAtMiningStation GenerateMiningStationAccident()
    {
        Inc player = Main.Player;
        if(player.CountAsteroids() > 0)
        {
            int idAster = Main.GenerateRandomInt(0, player.CountAsteroids());
            int amount = Main.GenerateRandomInt(1 , player.GetAsteroid(idAster).Workers);
            string text = $"????????? ?????????? ?????? ?? ?????????? ??????? {player.GetAsteroid(idAster).AsterName}, ??????? {amount} ???????";
            AccidentAtMiningStation accidentAtMiningStation = new AccidentAtMiningStation(player.GetAsteroid(idAster).MiningStation, amount, text, 0, 1, (int)Main.CeresTime, this);
            return accidentAtMiningStation;
        }
        else
        {
            return null;
        }
    }
    public void EventHappen()
    {

        
        for (int i = 0; i < events.Count; i++)
        {
            events[i].EventHappen();
        }
        CleanEvents();
        if(textNews.text != "")
        {
            newsPanel.Create_Button((int)mainClass.CeresTime, textNews.text);
            ShowNews();
        }                
    }

    private void CleanEvents()
    {
        for (int i = 0; i < events.Count; i++)
        {
            if (events[i].IsDone)
            {
                events.RemoveAt(i);
                CleanEvents();
                break;
            }
        }
    }

    private void ShowNews()
    {
        textNews.rectTransform.anchoredPosition = new Vector2(textNews.preferredWidth, 0);
        panel.SetActive(true);
        isShowNews = true;
    }

    private void Update()
    {
        if (isShowNews)
        {
            textNews.rectTransform.anchoredPosition = Vector2.Lerp(textNews.rectTransform.anchoredPosition, new Vector2(textNews.rectTransform.anchoredPosition.x - 20f, 0), Time.deltaTime * 10f);
            if(textNews.rectTransform.anchoredPosition.x < -100)
            {
                isShowNews = false;
                panel.SetActive(false);
                TextNews.text = "";
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            newsPanel.gameObject.SetActive(true);
            isShowNews = false;
            panel.SetActive(false);

        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            panel.SetActive(false);
            isShowNews = false;
        }
    }
    public StoryEvent GetEvent(int id)
    {
        return events[id];
    }

    public int CountEvents()
    {
        return events.Count;
    }

    public void SaveData(SaveLoadEvent save)
    {
        for(int i = 0; i < events.Count; i++)
        {
            save.accidents.Add(events[i].IdType);
        }
    }

    public void LoadData(SaveLoadEvent save)
    {
        for(int i = 0; i < save.accidents.Count; i++)
        {
            AddNewAccident(save.accidents[i]);
        }
    }

    private void AddNewAccident(int id)
    {
        switch (id)
        {
            case 0:
                events.Add(new FindNewMine(this));
                break;
            case 1:
                events.Add(new CloseMine(this));
                break;
            case 2:
                events.Add(new AccidentAtEarthBuilding(this));
                break;
            case 3:
                events.Add(new AccidentAtEarthMaterial(this));
                break;
            case 4:
                events.Add(new AccidentAtMiningStation(this));
                break;
            case 5:
                events.Add(new AccidentAtWarehouse(this));
                break;
        }
    }
}
