using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Market : MonoBehaviour
{
    [SerializeField] private Text textPrice, textAmount;
    [SerializeField] private main mainClass;
    [SerializeField] private Dropdown dropdownGraphY, dropdownGraphX;
    [SerializeField] private Graph graph;
    [SerializeField] private Slider slider;
    private int numberButton;
    public int FoodPrice { get; set; }
    public float EquipmentPrice { get; set; }
    private List<List<float>> listForDB = new List<List<float>>();
    private string[] namesOfTables = new string[] { "Prices", "Production", "AmountOnEarth", "AmountOnCeres", "Consumption" };
    //private string[] namesOfEconomyHealth = new string[]{ "Бурный рост", "Стабильное развитие", "Развитие", "Стагнация", "Упадок", "Депрессия" };
    // Start is called before the first frame update
    void Start()
    {
        CalculateSeason();
        dropdownGraphY.options.Add(new Dropdown.OptionData("Цены"));
        dropdownGraphY.options.Add(new Dropdown.OptionData("Производство на планете"));
        dropdownGraphY.options.Add(new Dropdown.OptionData("Запасы на Земле"));
        dropdownGraphY.options.Add(new Dropdown.OptionData("Запасы на Церере"));
        dropdownGraphY.options.Add(new Dropdown.OptionData("Потребление"));
        dropdownGraphY.RefreshShownValue();

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

        ChooseDropDown();
        CalculatePriceEquipment();
        FoodPrice = 1;

    }

    //Выведение количества ресурсов на рынке в зависимости какая кнопка была нажата.
    public void GetCurrentButton(int value)
    {
        textPrice.text = $"Цена {mainClass.Materials.GetMaterial(value).ElementName} = {mainClass.Materials.GetMaterial(value).Price}, запасов на земле = {mainClass.Earth.GetAmountRes(value)}";
        textAmount.text = $"Мы можем продать {mainClass.Player.GetAmountRes(value)} кг";
        numberButton = value;
        ChooseDropDown();
    }

    public void CalculateSeason()
    {
        for (int i = 0; i < mainClass.Materials.MaterialsCount(); i++)
        {
            CalcPrice(i);
            mainClass.Earth.PlusResAmount(i, mainClass.Materials.GetMaterial(i).Production);
        }
    }

    private void CalcPrice(int id)
    {
        //Высчитываем три цены, цену от количества, цена от потребления и цена от производства. потом складываем
        float priceAmount, priceProd, priceCons, priceFin;
        if(mainClass.Earth.GetAmountRes(id) > 0)
        {
            priceAmount = 1 - (12 * mainClass.Materials.GetMaterial(id).Consumption / mainClass.Earth.GetAmountRes(id));
        }
        else
        {
            priceAmount = 1 - (12 * mainClass.Materials.GetMaterial(id).Consumption / 1);
        }

        if(mainClass.Materials.GetMaterial(id).Production > 0)
        {
            priceCons = mainClass.Materials.GetMaterial(id).Consumption / mainClass.Materials.GetMaterial(id).Production;
        }
        else
        {
            priceCons = mainClass.Materials.GetMaterial(id).Consumption / 1;
        }
        
        
        priceProd = 100 - 0.0014f * mainClass.Materials.GetMaterial(id).Production;
        priceAmount = CheckNumber(priceAmount);
        priceCons = CheckNumber(priceCons);
        priceProd = CheckNumber(priceProd);
        priceFin = (float)Math.Round( (priceCons / priceAmount) * 100 + priceProd,2);
        float coef = mainClass.GenerateRandomInt(1, 20);
        coef = (coef - 10) / 100;
        priceFin += priceFin * coef;       
        mainClass.Materials.GetMaterial(id).Price = priceFin;
    }

    private float CheckNumber(float number)
    {
        if (number < 0 || float.IsNaN(number) || float.IsInfinity(number))
        {
            number = 0.0001f;
        }

        return number;        
    }

    public void ChooseDropDown()
    {
        if (mainClass.CeresTime > 29)
        {
            mainClass.DB.GetTableFromCommand($"SELECT Day, {mainClass.Materials.GetMaterial(numberButton).ElementName} " +
            $"FROM (SELECT * FROM (SELECT * FROM {namesOfTables[dropdownGraphY.value]} WHERE Day < {(int)mainClass.CeresTime + 1} ORDER BY Day)" +
            $" ORDER BY Day DESC LIMIT {(1 + dropdownGraphX.value) * 6}) ORDER BY Day", listForDB);
            graph.ShowGraph(listForDB, (int _i) => "Day", (int _f) => "$" + _f);
        }
    }

    private void CalculatePriceEquipment()
    {
        float price=0;
        //Посчитаем стоиомть килограмма Equipment 
        for(int i = 0; i < mainClass.Materials.MaterialsCount(); i++)
        {
            price += mainClass.Materials.GetMaterial(i).Price * mainClass.Materials.GetMaterial(i).CompositionEquipment;
        }
        //EquipmentPrice = (price / 1000)/mainClass.Earth.HighIndustry;
        EquipmentPrice = 10;
    }

    public void ChangeSlider()
    {
        if(mainClass.Player.GetAmountRes(numberButton) * slider.value != 0)
        {
            textAmount.text = $"Мы можем продать {mainClass.Player.GetAmountRes(numberButton)} кг. Продаем - {mainClass.Player.GetAmountRes(numberButton) * slider.value}, прибыль составит - {mainClass.Player.GetAmountRes(numberButton) * slider.value * mainClass.Materials.GetMaterial(numberButton).Price}";
        }
    }

    public void SellRes()
    {
        if(mainClass.Player.GetAmountRes(numberButton) * slider.value != 0)
        {
            mainClass.Player.Money += mainClass.Player.GetAmountRes(numberButton) * slider.value * mainClass.Materials.GetMaterial(numberButton).Price;
            mainClass.Player.PlusAmountRes(numberButton, -(mainClass.Player.GetAmountRes(numberButton) * slider.value));
            textAmount.text = $"Мы можем продать {mainClass.Player.GetAmountRes(numberButton)} кг";
        }        
    }
}
