using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CorpPanel : Spisok
{
    [SerializeField] Text textName, textAmount, textPrice, textBuy, textAmountMoney, textPriceGoods;
    [SerializeField] Slider slider;
    [SerializeField] main mainClass;
    [SerializeField] Graph graph;
    private List<ICorporateShare> corporateShares = new List<ICorporateShare>();
    private List<int> amountSharesOnMarket = new List<int>();
    private List<int> amountPlayersSharesOnMarket = new List<int>();
    private int chosenCorpId;
    string[] moneyNames = { "", "K", "M", "B", "T" };
    private List<List<float>> listForDB = new List<List<float>>();

    protected override void BuildElement(int id, string text)
    {
        RectTransform clone = Instantiate(element);
        clone.SetParent(scroll.content);
        clone.localScale = Vector3.one;
        clone.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
        CorpButton item = clone.GetComponent<CorpButton>();
        item.id = id;
        item.corpPanel = this;
        item.textName.text = text;

        buttons.Add(clone);
    }

    public void AddToShareList(ICorporateShare corporate)
    {
        corporateShares.Add(corporate);
        amountSharesOnMarket.Add(0);
        amountPlayersSharesOnMarket.Add(0);
    }

    public void SellShareToMarket(int id, int amount)
    {
        amountSharesOnMarket[id] += amount;
        corporateShares[id].Money += amount * corporateShares[id].PriceShare;
    }

    protected override void UpdateList(int id)
    {
        
    }

    public void UpdateText(int id)
    {
        textName.text = corporateShares[id].CorpName;
        textAmountMoney.text = $"У компании в наличии {FormatMoney((decimal)corporateShares[id].Money)}$ ";
        textAmount.text = $"На рынке можно купить максимум {amountSharesOnMarket[id]} акций";
        textPrice.text = $"Цена одной акции = {corporateShares[id].PriceShare}";
        textBuy.text = $"Вы покупаете ";
        chosenCorpId = id;
        slider.maxValue = amountSharesOnMarket[id] + amountPlayersSharesOnMarket[id];
        slider.value = amountPlayersSharesOnMarket[id];
        textPriceGoods.text = $"Стоимость товара на рынке Земли = {corporateShares[id].Price}";

        if (mainClass.CeresTime > 29)
        {
            mainClass.DB.GetTableFromCommand($"SELECT * FROM(SELECT Day, {corporateShares[id].CorpName} FROM Shareprice ORDER BY Day DESC LIMIT 12) WHERE Day < {(int)mainClass.CeresTime} ORDER BY Day", listForDB);
            Debug.Log($"Длинна массива {listForDB.Count},");
            graph.ShowGraph(listForDB, (int _i) => "Day", (int _f) => "$" + _f);
        }
    }

    public void UpdateTextBuy()
    {
        if(slider.value > amountPlayersSharesOnMarket[chosenCorpId])
        {
            int buyingShare = (int)(slider.value - amountPlayersSharesOnMarket[chosenCorpId]);
            textBuy.text = $"Вы покупаете {buyingShare} акций на сумму {buyingShare * corporateShares[chosenCorpId].PriceShare}";
        }
        else if(slider.value < amountPlayersSharesOnMarket[chosenCorpId])
        {
            int sellingShare = (int)(amountPlayersSharesOnMarket[chosenCorpId] - slider.value);
            textBuy.text = $"Вы продаете {sellingShare} акций на сумму {sellingShare * corporateShares[chosenCorpId].PriceShare}";
        }
        else
        {
            textBuy.text = $"Вы обладаете {amountPlayersSharesOnMarket[chosenCorpId]} акций на сумму {amountPlayersSharesOnMarket[chosenCorpId] * corporateShares[chosenCorpId].PriceShare}";
        }        
    }

    public float GetPriceShare(int id)
    {
        return corporateShares[id].PriceShare;
    }

    public string GetNameCorp(int id)
    {
        return corporateShares[id].CorpName;
    }

    public int CountCorp()
    {
        return corporateShares.Count;
    }

    string FormatMoney(decimal digit)
    {
        int n = 0;
        while (n + 1 < moneyNames.Length && digit >= 100m)
        {
            digit /= 1000m;
            n++;
        }
        return string.Format("{0}{1}", Math.Round(digit,2), moneyNames[n]);
    }

    public void BuySellShareFromMarket()
    {
        if (slider.value > amountPlayersSharesOnMarket[chosenCorpId])
        {
            int buyingShare = (int)(slider.value - amountPlayersSharesOnMarket[chosenCorpId]);
            amountPlayersSharesOnMarket[chosenCorpId] += buyingShare;
            mainClass.Player.Money -= buyingShare * corporateShares[chosenCorpId].PriceShare;
            amountSharesOnMarket[chosenCorpId] -= buyingShare;
        }
        else if (slider.value < amountPlayersSharesOnMarket[chosenCorpId])
        {
            int sellingShare = (int)(amountPlayersSharesOnMarket[chosenCorpId] - slider.value);
            amountPlayersSharesOnMarket[chosenCorpId] -= sellingShare;
            mainClass.Player.Money += sellingShare * corporateShares[chosenCorpId].PriceShare;
            amountSharesOnMarket[chosenCorpId] += sellingShare;
        }
    }
}
