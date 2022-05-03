using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource
{
    private float production;
    public string ElementName
    {
        get; private set;
    }

    public int Id { get; private set; }
    public float CompositionEquipment { get; set; }
    public float Price { get; set; }

    public float StartingPrice { get; set; }

    public float Production { get { return production * CoefFromEvent + ExcavatedAtLastMonth; } set { production = value; } }
    public float CleanProduction { get { return production; } set { production = value; } }
    //потребление ресурса за сезон
    public float Consumption { get; set; }
    public float CoefFromEvent { get; set; }

    public float ExcavatedAtLastMonth { get; set; }

    
    public Resource(int id,string name, float production, float equipComposition, float startPrice)
    {
        ElementName = name;
        this.production = production;
        Id = id;
        CompositionEquipment = equipComposition;
        StartingPrice = startPrice;
        CoefFromEvent = 1;
    }

    public void SaveData(SaveLoadResource save)
    {
        save.price = Price;
        save.production = Production;
        save.startingPrice = StartingPrice;
        save.compositionEquipment = CompositionEquipment;
        save.coefFromEvent = CoefFromEvent;
    }

    public void LoadData(SaveLoadResource save)
    {
        Price = save.price;
        Production = save.production;
        StartingPrice = save.startingPrice;
        CompositionEquipment = save.compositionEquipment;
        CoefFromEvent = save.coefFromEvent;
    }
}
