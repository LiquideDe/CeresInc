using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelStation : Spisok
{
    public main mainClass;
    private List<IAsteroid> asteroids;
    private List<GameObject> miningCorpStations = new List<GameObject>();
    [SerializeField] GameObject exampleButMiningCorp;
    [SerializeField] Transform buttonsPanel;

    protected override void BuildElement(int id, string text) // создание нового элемента и настройка параметров
    {
        RectTransform clone = Instantiate(element);
        clone.SetParent(scroll.content);
        clone.localScale = Vector3.one;
        clone.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
        StationButton item = clone.GetComponent<StationButton>();
        item.id = asteroids[id].Id;
        item.textName.text = text;
        item.textWorker.text = $"Рабочих \n {asteroids[id].Workers}/{asteroids[id].WorkersPlanned}";
        item.textFood.text = $"Еды \n {asteroids[id].Food}";
        item.textEquipment.text = $"Инструментов \n {asteroids[id].Equipment}";
        item.textClean.text = $"Сложность добычи \n {asteroids[id].ElementAbundance}";
        item.textIncome.text = $"Добывает в день \n {asteroids[id].IncomeLastMonth}";
        item.textExcavated.text = $"Всего добыто \n {asteroids[id].ExcavatedSoil}";
        item.textReady.text = $"Готово к отправке \n {asteroids[id].AmountReadyForLoading}";
        item.textAmount.text = $"Всего осталось ресурса \n {asteroids[id].ElementCapacity}";

        buttons.Add(clone);
    }

    public void UpdateText()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            StationButton item = buttons[i].GetComponent<StationButton>();
            item.textWorker.text = $"Рабочих \n {asteroids[i].Workers}/{asteroids[i].WorkersPlanned}";
            item.textFood.text = $"Еды \n {asteroids[i].Food}";
            item.textEquipment.text = $"Инструментов \n {asteroids[i].Equipment}";
            item.textIncome.text = $"Добыто за прошлый месяц \n {asteroids[i].IncomeLastMonth}";
            item.textExcavated.text = $"Всего добыто \n {asteroids[i].ExcavatedSoil}";
            item.textReady.text = $"Готово к отправке \n {asteroids[i].AmountReadyForLoading}";
            item.textAmount.text = $"Всего осталось ресурса \n {asteroids[i].ElementCapacity}";
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

    private void CleanList()
    {
        ClearList();
    }

    public void BuildStationList(List<IAsteroid> asteroids)
    {
        this.asteroids = new List<IAsteroid>(asteroids);
        if(buttons.Count > 0)
        {
            CleanList();
        }

        for(int i = 0; i < asteroids.Count; i++)
        {
            Create_Button(i, asteroids[i].AsterName);
        }        

    }

    public IEnumerator CreateButtons()
    {
        BuildMiningCorpButtons("Player.Inc", null);
        for(int i = 0; i < mainClass.Corporates.CountMiningCorp(); i++)
        {
            BuildMiningCorpButtons(mainClass.Corporates.GetMiningCorporates(i).CorpName, mainClass.Corporates.GetMiningCorporates(i));
        }
        yield return new WaitForSeconds(0.01f);
    }
    private void BuildMiningCorpButtons(string name, MiningCorporate corp)
    {
        GameObject gameObject = Instantiate(exampleButMiningCorp);
        gameObject.transform.SetParent(buttonsPanel);
        gameObject.SetActive(true);
        MiningStationButton stationButton = gameObject.GetComponent<MiningStationButton>();
        stationButton.NameBut.text = $"{name}";
        stationButton.Corp = corp;
        miningCorpStations.Add(gameObject);
    }
}
