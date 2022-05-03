using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelOrder : Spisok
{
    [SerializeField] Text textEqAmount, textFoodAmount, textCarcPasAmount, textCarcCargoAmount, textEngineAmount, textFuelTankAmount, textWorkers; //тексты сколько на борту
    [SerializeField] Text textEqWare, textFoodWare, textCarcPasWare, textCarcCargoWare, textEngineWare, textFuelWare, textWorkersOnBase; //тексты сколько на складе у компании уже есть
    [SerializeField] Text textMass, textCost;
    [SerializeField] main mainClass;
    [SerializeField] Text textDateTimer;
    [SerializeField] Dropdown dpdCarcPas, dpdCarcCargo, dpdTank, dpdEngine;
    public List<GameObject> buttonsRule = new List<GameObject>();
    private int chosenId = 0;
    OrderButton order;

    private void Start()
    {
        for(int i = 0; i < 3; i++)
        {
            Create_Button(i);
        }
    }

    private void Update()
    {
        if (!mainClass.IsPaused)
        {
            for (int i = 0; i < 3; i++)
            {
                buttons[i].GetComponent<OrderButton>().UpdateText();
            }
        }       
    }

    public void Arrival()
    {
        PressDelivery(0);
        Inc inc = mainClass.Player;
        inc.Food += order.food;
        inc.Equipment += order.equipment;
        mainClass.Ceres.FreeWorkers += order.workers;
        for(int i = 0; i < order.carcassPas.Count; i++)
        {
            inc.PlusCarcassPas(i, order.carcassPas[i]);
        }
        for(int i=0;i<order.carcassCargo.Count; i++)
        {
            inc.PlusCarcassCargo(i, order.carcassCargo[i]);
        }
        for(int i = 0; i < order.fuelTank.Count; i++)
        {
            inc.PlusFuelTank(i, order.fuelTank[i]);
        }
        for(int i = 0; i < order.engine.Count; i++)
        {
            inc.PlusEngine(i, order.engine[i]);
        }
        PressDelivery(1);
        UpdateList(0);
        order.atJourney = true;
        order.textJourney.text = $"At Journey to Ceres";
        UpdateText();
        mainClass.Warehouse.UpdateText();
    }

    protected override void BuildElement(int id, string text="")
    {
        RectTransform clone = Instantiate(element);
        clone.SetParent(scroll.content);
        clone.localScale = Vector3.one;
        clone.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
        OrderButton order = clone.GetComponent<OrderButton>();
        order.id = id;
        order.textMas.text = $"0/{mainClass.Earth.SpaceIndustry.GetTotalOutput()*20000} кг";            
        //order.textRemain.text = $"{mainClass.DayBeforeArrival + 460 * id}";
        if (id == 0)
        {
            order.textJourney.text = $"At Journey to Ceres";
            order.atJourney = true;
        }            
        else
            order.textJourney.text = $"At Earth, preparing to Ceres";
        order.maxMas = 3 * 20000;
        order.UpdateText();
        clone.gameObject.SetActive(true);
        buttons.Add(clone);
        
    }

    public void PressDelivery(int id)
    {
        chosenId = id;
        order = buttons[chosenId].GetComponent<OrderButton>();
        if (!order.atJourney)
        {
            for (int i = 0; i < buttonsRule.Count; i++)
            {
                buttonsRule[i].SetActive(true);
                dpdCarcPas.ClearOptions();
                dpdCarcCargo.ClearOptions();
                dpdTank.ClearOptions();
                dpdEngine.ClearOptions();
                
            }
            for (int j = 0; j < mainClass.Sciense.CountCarcass(); j++)
            {
                if (mainClass.Sciense.GetCarcass(j).IsResearched)
                {
                    dpdCarcPas.options.Add(new Dropdown.OptionData(mainClass.Sciense.GetCarcass(j).NameTech));
                    dpdCarcCargo.options.Add(new Dropdown.OptionData(mainClass.Sciense.GetCarcass(j).NameTech));
                }                    
            }

            for (int j = 0; j < mainClass.Sciense.CountFuelTank(); j++)
            {
                if(mainClass.Sciense.GetFuelTank(j).IsResearched)
                    dpdTank.options.Add(new Dropdown.OptionData(mainClass.Sciense.GetFuelTank(j).NameTech));
            }
            for (int j = 0; j < mainClass.Sciense.CountEngine(); j++)
            {
                if(mainClass.Sciense.GetEngine(j).IsResearched)
                    dpdEngine.options.Add(new Dropdown.OptionData(mainClass.Sciense.GetEngine(j).NameTech));
            }
            dpdCarcPas.RefreshShownValue();
            dpdCarcCargo.RefreshShownValue();
            dpdTank.RefreshShownValue();
            dpdEngine.RefreshShownValue();
        }
        else
        {
            for (int i = 0; i < buttonsRule.Count; i++)
            {
                buttonsRule[i].SetActive(false);
            }
        }

        UpdateText();
    }

    protected override void UpdateList(int id)
    {
        vPos = scroll.verticalNormalizedPosition; // запоминаем позицию скролла
        OrderButton item = buttons[id].GetComponent<OrderButton>();
        Destroy(item.gameObject); // удаляем этот элемент из списка
        buttons.RemoveAt(id); // удаляем этот элемент из массива
        curY = 0;
        size--; // минус один элемент
        RectContent(); // пересчитываем размеры окна
        foreach (RectTransform b in buttons) // сдвигаем элементы
        {
            b.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
            curY += delta.y;
        }
        scroll.verticalNormalizedPosition = vPos; // возвращаем позицию скролла
        RecalcId();
        Create_Button(buttons.Count);
    }

    public void UpdateText()
    {
        Inc inc = mainClass.Player;
        textEqAmount.text = $"{order.equipment} кг";
        textFoodAmount.text = $"{order.food} кг";
        textCarcPasAmount.text = $"{CountDevices(order.carcassPas)} шт";
        textCarcCargoAmount.text = $"{CountDevices(order.carcassCargo)} шт";
        textEngineAmount.text = $"{CountDevices(order.engine)} шт";
        textFuelTankAmount.text = $"{CountDevices(order.fuelTank)} шт";

        textEqWare.text = $"{inc.Equipment} кг";
        textFoodWare.text = $"{inc.Food} кг";
        textCarcPasWare.text = $"{inc.SumCarcassPas} шт";
        textCarcCargoWare.text = $"{inc.SumCarcassCargo} шт";
        textFuelWare.text = $"{inc.SumFuelTank} шт";
        textEngineWare.text = $"{inc.SumEngine} шт";

        textMass.text = $"{order.masAll}/{order.maxMas}";
        
    }

    private int CountDevices(List<int> device)
    {
        int sum = 0;
        for (int i = 0; i < device.Count; i++)
        {
            sum += device[i];
        }
        return sum;
    }

    private void RecalcId()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            buttons[i].GetComponent<OrderButton>().id = i;
        }
    }

    public void PlusEqFood(int id)
    {
        if (id == 0)
        {
            order.masAll +=  PlusEF(ref order.equipment, order.maxMas, order.masAll, mainClass.Market.EquipmentPrice);
        }
        else
        {
            order.masAll += PlusEF(ref order.food, order.maxMas, order.masAll, 1);
        }
        
        textEqAmount.text = $"{order.equipment} кг";
        textFoodAmount.text = $"{order.food} кг";
        textMass.text = $"{order.masAll}/{order.maxMas}";
        order.textMas.text = $"{order.masAll}/{order.maxMas}";
        mainClass.UpdateText();
    }

    private int PlusEF(ref int supplies, int maxmas, int masall, float price)
    {
        bool pressedShift, pressedCtrl;
        float money = mainClass.Player.Money;
        pressedShift = (Input.GetKey(KeyCode.LeftShift) ? true : false);
        pressedCtrl = (Input.GetKey(KeyCode.LeftControl) ? true : false);

        if (pressedCtrl && pressedShift && masall + 10000 <= maxmas && 10000 * price <= money)
        {
            supplies += 10000;
            mainClass.Player.Money -= price * 10000;
            return 10000;
        }
        else if (!pressedCtrl && pressedShift && masall + 1000 <= maxmas && 1000 * price <= money)
        {
            supplies += 1000;
            mainClass.Player.Money -= price * 1000;
            return 1000;
        }
        else if (pressedCtrl && !pressedShift && masall + 100 <= maxmas && 100 * price <= money)
        {
            supplies += 100;
            mainClass.Player.Money -= price * 100;
            return 100;
        }
        else if( masall + 10 <= maxmas && 10 * price <= money)
        {
            supplies += 10;
            mainClass.Player.Money -= price * 10;
            return 10;
        }
        else
        {
            return 0;
        }
    }

    public void MinusEqFood(int id)
    {
        if (id == 0)
        {
            order.masAll -= MinusEF(ref order.equipment, order.masAll, mainClass.Market.EquipmentPrice);
        }
        else
        {
            order.masAll -= MinusEF(ref order.food, order.masAll, 1);
        }

        textEqAmount.text = $"{order.equipment} кг";
        textFoodAmount.text = $"{order.food} кг";
        textMass.text = $"{order.masAll}/{order.maxMas}";
        order.textMas.text = $"{order.masAll}/{order.maxMas}";
        mainClass.UpdateText();
    }

    private int MinusEF(ref int supplies, int masall, float price)
    {
        bool pressedShift, pressedCtrl;

        pressedShift = (Input.GetKey(KeyCode.LeftShift) ? true : false);
        pressedCtrl = (Input.GetKey(KeyCode.LeftControl) ? true : false);
        if (pressedCtrl && pressedShift && masall - 10000 >= 0)
        {
            supplies -= 10000;
            mainClass.Player.Money += 10000 * price;
            return 10000;
        }
        else if (!pressedCtrl && pressedShift && masall - 1000 >= 0)
        {
            supplies -= 1000;
            mainClass.Player.Money += 1000 * price;
            return 1000;
        }
        else if (pressedCtrl && !pressedShift && masall - 100 >= 0)
        {
            supplies -= 100;
            mainClass.Player.Money += 100 * price;
            return 100;
        }
        else if(masall - 10 >= 0)
        {
            supplies -= 10;
            mainClass.Player.Money += 10 * price;
            return 10;
        }
        else
        {
            return 0;
        }
    }

    public void HireWorkers()
    {
        order.workers += 1;
        order.masAll += 80;
        textWorkers.text = $"{order.workers} человек";
        textMass.text = $"{order.masAll}/{order.maxMas}";
        order.textMas.text = $"{order.masAll}/{order.maxMas}";
        mainClass.Player.Money -= 1000;
        mainClass.UpdateText();
    }

    public void FireWorker()
    {
        order.workers -= 1;
        order.masAll -= 80;
        textWorkers.text = $"{order.workers} человек";
        textMass.text = $"{order.masAll}/{order.maxMas}";
        order.textMas.text = $"{order.masAll}/{order.maxMas}";
        mainClass.Player.Money -= 1000;
        mainClass.UpdateText();
    }

    public void BuyCarcassPas()
    {
        if(mainClass.Sciense.GetCarcass(dpdCarcPas.value).Weight + order.masAll <= order.maxMas && mainClass.Sciense.GetCarcass(dpdCarcPas.value).Cost <= mainClass.Player.Money)
        {
            if(dpdCarcPas.value >= order.carcassPas.Count)
            {
                order.carcassPas.Add(0);
            }
            order.carcassPas[dpdCarcPas.value] += 1;            
            order.masAll += (int)mainClass.Sciense.GetCarcass(dpdCarcPas.value).Weight;

            textCarcPasAmount.text = $"{CountDevices(order.carcassPas)} шт";
            textMass.text = $"{order.masAll}/{order.maxMas}";
            order.textMas.text = $"{order.masAll}/{order.maxMas}";
            mainClass.Player.Money -= mainClass.Sciense.GetCarcass(dpdCarcPas.value).Cost;
            mainClass.UpdateText();
        }
        
    }

    public void SellCarcassPas()
    {
        if (dpdCarcPas.value >= order.carcassPas.Count)
        {
            order.carcassPas.Add(0);
        }
        if (order.carcassPas[dpdCarcPas.value] > 0)
        {
            order.carcassPas[dpdCarcPas.value] -= 1;            
            order.masAll -= (int)mainClass.Sciense.GetCarcass(dpdCarcPas.value).Weight;

            textCarcPasAmount.text = $"{CountDevices(order.carcassPas)} шт";
            textMass.text = $"{order.masAll}/{order.maxMas}";
            order.textMas.text = $"{order.masAll}/{order.maxMas}";
            mainClass.Player.Money += mainClass.Sciense.GetCarcass(dpdCarcPas.value).Cost;
            mainClass.UpdateText();
        }
        
    }

    public void BuyCarcassCargo()
    {
        if (mainClass.Sciense.GetCarcass(dpdCarcCargo.value).Weight + order.masAll <= order.maxMas && mainClass.Sciense.GetCarcass(dpdCarcCargo.value).Cost <= mainClass.Player.Money)
        {
            if (dpdCarcCargo.value >= order.carcassCargo.Count)
            {
                order.carcassCargo.Add(0);
            }
            order.carcassCargo[dpdCarcCargo.value] += 1;
            order.masAll += (int)mainClass.Sciense.GetCarcass(dpdCarcCargo.value).Weight;

            textCarcCargoAmount.text = $"{CountDevices(order.carcassCargo)} шт";
            textMass.text = $"{order.masAll}/{order.maxMas}";
            order.textMas.text = $"{order.masAll}/{order.maxMas}";
            mainClass.Player.Money -= mainClass.Sciense.GetCarcass(dpdCarcCargo.value).Cost;
            mainClass.UpdateText();
        }

    }

    public void SellCarcassCargo()
    {
        if (dpdCarcCargo.value >= order.carcassCargo.Count)
        {
            order.carcassCargo.Add(0);
        }
        if (order.carcassCargo[dpdCarcCargo.value] > 0)
        {
            order.carcassCargo[dpdCarcCargo.value] -= 1;
            order.masAll -= (int)mainClass.Sciense.GetCarcass(dpdCarcCargo.value).Weight;

            textCarcCargoAmount.text = $"{CountDevices(order.carcassCargo)} шт";
            textMass.text = $"{order.masAll}/{order.maxMas}";
            order.textMas.text = $"{order.masAll}/{order.maxMas}";
            mainClass.Player.Money += mainClass.Sciense.GetCarcass(dpdCarcCargo.value).Cost;
            mainClass.UpdateText();
        }

    }

    public void BuyFuelTank()
    {
        if (mainClass.Sciense.GetFuelTank(dpdTank.value).Weight + order.masAll <= order.maxMas && mainClass.Sciense.GetFuelTank(dpdTank.value).Cost <= mainClass.Player.Money)
        {
            if (dpdTank.value >= order.fuelTank.Count)
            {
                order.fuelTank.Add(0);
            }
            order.fuelTank[dpdTank.value] += 1;
            order.masAll += (int)mainClass.Sciense.GetFuelTank(dpdTank.value).Weight;

            textFuelTankAmount.text = $"{CountDevices(order.fuelTank)} шт";
            textMass.text = $"{order.masAll}/{order.maxMas}";
            order.textMas.text = $"{order.masAll}/{order.maxMas}";
            mainClass.Player.Money -= mainClass.Sciense.GetFuelTank(dpdTank.value).Cost; 
            mainClass.UpdateText();
        }
    }

    public void SellFuelTank()
    {
        if (dpdTank.value >= order.fuelTank.Count)
        {
            order.fuelTank.Add(0);
        }
        if (order.fuelTank[dpdTank.value] > 0)
        {
            order.fuelTank[dpdTank.value] -= 1;
            order.masAll -= (int)mainClass.Sciense.GetFuelTank(dpdTank.value).Weight;

            textFuelTankAmount.text = $"{CountDevices(order.fuelTank)} шт";
            textMass.text = $"{order.masAll}/{order.maxMas}";
            order.textMas.text = $"{order.masAll}/{order.maxMas}";
            mainClass.Player.Money += mainClass.Sciense.GetFuelTank(dpdTank.value).Cost;
            mainClass.UpdateText();
        }
    }

    public void BuyEngine()
    {
        if (mainClass.Sciense.GetEngine(dpdEngine.value).Weight + order.masAll <= order.maxMas && mainClass.Sciense.GetEngine(dpdEngine.value).Cost <= mainClass.Player.Money)
        {
            if (dpdEngine.value >= order.engine.Count)
            {
                order.engine.Add(0);
            }
            order.engine[dpdEngine.value] += 1;
            order.masAll += (int)mainClass.Sciense.GetEngine(dpdEngine.value).Weight;

            textEngineAmount.text = $"{CountDevices(order.engine)} шт";
            textMass.text = $"{order.masAll}/{order.maxMas}";
            order.textMas.text = $"{order.masAll}/{order.maxMas}";
            mainClass.Player.Money -= mainClass.Sciense.GetEngine(dpdEngine.value).Cost;
            mainClass.UpdateText();
        }
    }

    public void SellEngine()
    {
        if (dpdEngine.value >= order.engine.Count)
        {
            order.engine.Add(0);
        }
        if (order.engine[dpdEngine.value] > 0)
        {
            order.engine[dpdEngine.value] -= 1;
            order.masAll -= (int)mainClass.Sciense.GetEngine(dpdEngine.value).Weight;

            textEngineAmount.text = $"{CountDevices(order.engine)} шт";
            textMass.text = $"{order.masAll}/{order.maxMas}";
            order.textMas.text = $"{order.masAll}/{order.maxMas}";
            mainClass.Player.Money += mainClass.Sciense.GetEngine(dpdEngine.value).Cost;
            mainClass.UpdateText();
        }
    }

    public OrderButton GetOrder(int id)
    {
        return buttons[id].GetComponent<OrderButton>();
    }


}
