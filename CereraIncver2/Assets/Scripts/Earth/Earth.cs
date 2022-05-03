using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class Earth : MonoBehaviour
{
    public int Population { get; set; }
    public float GDP { get; set; }
    public bool IsBuildAllowed { get; set; }
    private float[] amountRes = new float[23];
    private List<float> totalConsumption = new List<float>();
    private List<List<float>> listForDB = new List<List<float>>();
    private List<List<float>> listForDB2 = new List<List<float>>();
    private List<ICorporateConsumption> corporateConsumptions = new List<ICorporateConsumption>();
    [SerializeField] private Graph graph;
    [SerializeField] private Dropdown dropdownGraphX;
    [SerializeField] private main mainClass;
    [SerializeField] private Text textGraph, textPopulation, textGDP, textHealthEconomic, textAmountDay;
    private string[] namesOfEconomyHealth = new string[] { "Бурный рост", "Стабильное развитие", "Развитие", "Стагнация", "Упадок", "Депрессия" };
    private string chosenGraph = "Population";
    private int prevGraph = 0, intChosenGraph=0;
    public List<EarthButton> earthButtons;
    public int EconomyHealth { get; set; }
    public EnergyCorp Energy { get; set; }
    public HeavyCorp HeavyIndustry { get; set; }
    public LightCorp LightIndustry { get; set; }
    public HighCorp HighIndustry { get; set; }
    public SpaceCorp SpaceIndustry { get; set; }
    public float HeavyGoods { get; set; }
    public float LightGoods { get; set; }
    public float HighGoods { get; set; }
    public float SpaceGoods { get; set; }
    public float EnergyGoodsCoef { get; set; }
    public float HeavyGoodsCoef { get; set; }
    public float LightGoodsCoef { get; set; }
    public float HighGoodsCoef { get; set; }
    public float SpaceGoodsCoef { get; set; }
    //System.Random rand = new System.Random();

    public Earth()
    {
        for (int i = 0; i < 23; i++)
        {
            amountRes[i] = 100000;
        }
        Population = 7000;
        EconomyHealth = 1;
        EnergyGoodsCoef = 0.003f;
        HeavyGoodsCoef = 0.002f;
        LightGoodsCoef = 0.003f;
        HighGoodsCoef = 0.0075f;
        SpaceGoodsCoef = 0.001f;
    }   

    public void StartGame(bool isNewGame)
    {
        Energy.StartGame(isNewGame);
        HeavyIndustry.StartGame(isNewGame);
        LightIndustry.StartGame(isNewGame);
        HighIndustry.StartGame(isNewGame);
        SpaceIndustry.StartGame(isNewGame);
    }

    private void Start()
    {
        dropdownGraphX.options.Add(new Dropdown.OptionData("Полгода"));
        dropdownGraphX.options.Add(new Dropdown.OptionData("Год"));
        dropdownGraphX.options.Add(new Dropdown.OptionData("Полтора года"));
        dropdownGraphX.options.Add(new Dropdown.OptionData("2 года"));
        dropdownGraphX.options.Add(new Dropdown.OptionData("2,5 года"));
        dropdownGraphX.options.Add(new Dropdown.OptionData("3 года"));
        dropdownGraphX.options.Add(new Dropdown.OptionData("3,5 года"));
        dropdownGraphX.options.Add(new Dropdown.OptionData("4 года"));
        dropdownGraphX.options.Add(new Dropdown.OptionData("4,5 года"));
        dropdownGraphX.options.Add(new Dropdown.OptionData("5 лет"));
        dropdownGraphX.RefreshShownValue();
        //UpdateText();
        Energy = new EnergyCorp(mainClass);
        HeavyIndustry = new HeavyCorp(mainClass);
        LightIndustry = new LightCorp(mainClass);
        HighIndustry = new HighCorp(mainClass);
        SpaceIndustry = new SpaceCorp(mainClass);

        corporateConsumptions.Add(Energy);
        corporateConsumptions.Add(HeavyIndustry);
        corporateConsumptions.Add(LightIndustry);
        corporateConsumptions.Add(HighIndustry);
        corporateConsumptions.Add(SpaceIndustry);

        mainClass.CorpPanel.Create_Button(0, Energy.CorpName);
        mainClass.CorpPanel.AddToShareList(Energy);

        mainClass.CorpPanel.Create_Button(1, HeavyIndustry.CorpName);
        mainClass.CorpPanel.AddToShareList(HeavyIndustry);

        mainClass.CorpPanel.Create_Button(2, LightIndustry.CorpName);
        mainClass.CorpPanel.AddToShareList(LightIndustry);

        mainClass.CorpPanel.Create_Button(3, HighIndustry.CorpName);
        mainClass.CorpPanel.AddToShareList(HighIndustry);

        mainClass.CorpPanel.Create_Button(4, SpaceIndustry.CorpName);
        mainClass.CorpPanel.AddToShareList(SpaceIndustry);
    }

    public void NewPopulation()
    {
        float coef = mainClass.GenerateRandomInt(2, 8);
        Population += (int)(Population * ((coef / 1000)/12));
        UpdateText();        
    }

    public float GetAmountRes(int id)
    {
        return amountRes[id];
    }

    public void SetAmountRes(int id, float value)
    {
        amountRes[id] = value;
        if (amountRes[id] < 0)
            amountRes[id] = 0;
    }

    public void PlusResAmount(int id, float value)
    {
        amountRes[id] += value;
    }

    public void ChooseDropDown()
    {
        if (mainClass.CeresTime > 29)
        {
            if(chosenGraph != "Population" && chosenGraph != "GDP")
            {
                mainClass.DB.GetTableFromCommand($"SELECT * FROM (SELECT Day, {chosenGraph} FROM LvlIndustry WHERE Day < {(int)mainClass.CeresTime + 1} ORDER BY Day DESC LIMIT {(1 + dropdownGraphX.value) * 6}) ORDER BY Day", listForDB);
                mainClass.DB.GetTableFromCommand($"SELECT * FROM(SELECT Day, {chosenGraph} FROM LvlConsumptionIndustry WHERE Day < {(int)mainClass.CeresTime + 1} ORDER BY Day DESC LIMIT {(1 + dropdownGraphX.value) * 6}) ORDER BY Day", listForDB2);
                CalcSharedY();
                graph.ShowGraph(listForDB, (int _i) => "Day", (int _f) => "" + _f, "bar", "Blue",true,CalcSharedY().Item1, CalcSharedY().Item2);
                graph.ShowGraph(listForDB2, null, null, "bar","Green", false, CalcSharedY().Item1, CalcSharedY().Item2);
                
            }
            else if(chosenGraph == "Population")
            {
                mainClass.DB.GetTableFromCommand($"SELECT * FROM(SELECT Day, PopulationOnEarth FROM Population WHERE Day < {(int)mainClass.CeresTime + 1} ORDER BY Day DESC LIMIT {(1 + dropdownGraphX.value) * 6}) ORDER BY Day", listForDB);
                graph.ShowGraph(listForDB, (int _i) => "Day", (int _f) => "unit" + _f);
            }
            else if(chosenGraph == "GDP")
            {
                mainClass.DB.GetTableFromCommand($"SELECT Day, GdpEarth FROM GDP WHERE Day < {(int)mainClass.CeresTime + 1} ORDER BY Day DESC LIMIT {(1 + dropdownGraphX.value) * 6}", listForDB);
                graph.ShowGraph(listForDB, (int _i) => "Day", (int _f) => "$" + _f);
            }
        }
    }

    public void ChooseGraph(int id)
    {
        switch (id)
        {
            case 0:
                chosenGraph = "Population";                
                break;
            case 1:
                chosenGraph = "GDP";
                break;
            case 2:
                chosenGraph = "Energy";
                break;
            case 3:
                chosenGraph = "Heavy";
                break;
            case 4:
                chosenGraph = "Light";
                break;
            case 5:
                chosenGraph = "High";
                break;
            case 6:
                chosenGraph = "Space";
                break;
        }

        earthButtons[prevGraph].transform.position = new Vector3(earthButtons[prevGraph].transform.position.x, 105);

        earthButtons[id].transform.position = new Vector3(earthButtons[id].transform.position.x, 107.7f);
        prevGraph = id;
        intChosenGraph = id;
        UpdateText();
        ChooseDropDown();
    }

    public void UpdateText()
    {
        textGraph.text = chosenGraph;
        textPopulation.text = $"Население Земли - {Population}m";
        textGDP.text = $"ВВП Земли - {GDP}";
        textHealthEconomic.text = $"Состояние экономики: {namesOfEconomyHealth[EconomyHealth-1]}";
        if (intChosenGraph > 1)
            textAmountDay.text = $"При текущей инфраструктуре и текущих запасах, выбранная промышленность сможет еще работать минимум {CalcRemainDay(intChosenGraph - 2)} месяцев";
        else
            textAmountDay.text = "";
    }

    private (float,float) CalcSharedY()
    {
        float yMax = 0, yMin = listForDB[0][1];
        for (int i=0; i < listForDB.Count; i++)
        {
            if(yMax < Math.Max(listForDB[i][1], listForDB2[i][1]))
            {
                yMax = Math.Max(listForDB[i][1], listForDB2[i][1]);
            }
            if(yMin > Math.Min(listForDB[i][1], listForDB2[i][1]))
            {
                yMin = Math.Min(listForDB[i][1], listForDB2[i][1]);
            }
        }
        return (yMax, yMin);
    }

    private int CalcRemainDay(int id)
    {
        
        int dayRemain = 10000;
        float dayRemain2;
        for (int i = 0; i < mainClass.Materials.MaterialsCount(); i++)
        {
            dayRemain2 = amountRes[i] / corporateConsumptions[id].GetConsumption(i);
            if (dayRemain > dayRemain2)
                dayRemain = (int)dayRemain2;
        }
        return dayRemain;
    }

    public void CalculationSeason()
    {
        NewPopulation();
        Energy.CalculateSeason();
        HeavyIndustry.CalculateSeason();
        LightIndustry.CalculateSeason();
        HighIndustry.CalculateSeason();
        SpaceIndustry.CalculateSeason();
        TotalConsumption();

        Energy.PostCalculation();
        HeavyIndustry.PostCalculation();
        LightIndustry.PostCalculation();
        HighIndustry.PostCalculation();
        SpaceIndustry.PostCalculation();

        HeavyGoods -= Population * HeavyGoodsCoef;
        LightGoods -= Population * LightGoodsCoef;
        HighGoods -= Population * HighGoodsCoef;
        SpaceGoods -= Population * SpaceGoodsCoef;
    }

    private void TotalConsumption()
    {
        totalConsumption.Clear();
        if(totalConsumption.Count != mainClass.Materials.MaterialsCount())
        {
            CreatingList(totalConsumption, mainClass.Materials.MaterialsCount());
        }

        for(int k = 0; k < corporateConsumptions.Count; k++)
        {
            for (int i = 0; i < mainClass.Materials.MaterialsCount(); i++)
            {
                totalConsumption[i] += corporateConsumptions[k].GetConsumption(i);
            }
        }        

        mainClass.Materials.WriteConsumption(totalConsumption);
    }

    public float GetProm(int id)
    {
        return corporateConsumptions[id].GetTotalOutput();
    }

    public float GetConsumptionIndustry(int id)
    {
        return corporateConsumptions[id].GetReservedOutput();
    }

    public float GetTotalResConsumption(int id)
    {
        return totalConsumption[id];
    }

    protected void CreatingList(List<float> list, int capacity)
    {
        for (int i = 0; i < capacity; i++)
        {
            list.Add(0);
        }
    }

    public EarthCorp GetEarthCorp(int id)
    {
        switch (id)
        {
            case 0:
                return Energy;
            case 1:
                return HeavyIndustry;
            case 2:
                return LightIndustry;
            case 3:
                return HighIndustry;
            case 4:
                return SpaceIndustry;
            default:
                return null;
        }
    }

    public void SaveData(SaveLoadEarth save)
    {
        save.economyHealth = EconomyHealth;
        save.population = Population;
        save.gdp = GDP;
        save.lightGoods = LightGoods;
        save.heavyGoods = HeavyGoods;
        save.highGoods = HighGoods;
        save.spaceGoods = SpaceGoods;

        for (int i = 0; i < mainClass.Materials.MaterialsCount(); i++)
        {
            save.amountRes[i] = amountRes[i];
        }

        for(int i = 0; i < totalConsumption.Count; i++)
        {
            save.totalConsumption.Add(totalConsumption[i]);
        }
    }

    public void LoadData(SaveLoadEarth save)
    {
        EconomyHealth = save.economyHealth;
        Population = save.population;
        GDP = save.gdp;
        LightGoods = save.lightGoods;
        HeavyGoods = save.heavyGoods;
        HighGoods = save.highGoods;
        SpaceGoods = save.spaceGoods;

        for(int i = 0; i < save.amountRes.Length; i++)
        {
            amountRes[i] = save.amountRes[i];
        }

        if(save.totalConsumption.Count != totalConsumption.Count)
        {
            CreatingList(totalConsumption, save.totalConsumption.Count);
        }
        for (int i = 0; i < totalConsumption.Count; i++)
        {
            save.totalConsumption[i] = totalConsumption[i];
        }
    }
}
