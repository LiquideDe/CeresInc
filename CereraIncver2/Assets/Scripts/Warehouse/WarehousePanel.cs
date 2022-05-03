using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarehousePanel : MonoBehaviour
{
    [SerializeField] private GameObject cellElement;
    [SerializeField] private main mainClass;
    [SerializeField] private Text textFoodAmount, textFoodCons, textEquipAmount, textEquipCons, textPasCarcass, textCargoCarcass, textFuelTank, textEngine;
    [SerializeField] private Text textWorkersPanel, textFoodPanel, textEquipmentPanel;
    private List<GameObject> cells = new List<GameObject>();
    private float[] res = new float[23];
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < mainClass.Materials.MaterialsCount() - 1; i++)
        {
            CreateCell(i);
        }
        
        textFoodAmount.text = $"{mainClass.Player.Food}";
        textEquipAmount.text = $"{mainClass.Player.Equipment}";
        textPasCarcass.text = $"{mainClass.Player.SumCarcassPas}";
        textCargoCarcass.text = $"{mainClass.Player.SumCarcassCargo}";
        textFuelTank.text = $"{mainClass.Player.SumFuelTank}";
        textEngine.text = $"{mainClass.Player.SumEngine}";
    }

    private void CreateCell(int id)
    {
        GameObject gameObject = Instantiate(cellElement);
        var cell = gameObject.GetComponent<ArrivalButton>();
        cell.transform.SetParent(transform.GetChild(0));
        cell.nameText.text = mainClass.Materials.GetMaterial(id).ElementName;
        cell.id = id;
        cell.amount.text = $"{res[id]}";        
        cells.Add(gameObject);
        gameObject.SetActive(true);
    }

    public void UpdateText()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            var cell = cells[i].GetComponent<ArrivalButton>();
            cell.amount.text = $"{res[cell.id]}";        
        }
        textFoodAmount.text = $"{mainClass.Player.Food}";
        textEquipAmount.text = $"{mainClass.Player.Equipment}";
        textEquipCons.text = $"-{mainClass.Player.EquipmentLeftFor().Item1} in day/ All left for {mainClass.Player.EquipmentLeftFor().Item2}";
        textFoodCons.text = $"-{mainClass.Player.FoodLeftFor().Item1} in day/ All left for {mainClass.Player.FoodLeftFor().Item2}";
        textPasCarcass.text = $"{mainClass.Player.SumCarcassPas}";
        textCargoCarcass.text = $"{mainClass.Player.SumCarcassCargo}";
        textFuelTank.text = $"{mainClass.Player.SumFuelTank}";
        textEngine.text = $"{mainClass.Player.SumEngine}";
        textWorkersPanel.text = $"";
        textFoodPanel.text = $"{mainClass.Player.Food}";
        textEquipmentPanel.text = $"{mainClass.Player.Equipment}";
    }

    public void ArrivalFromEarth()
    {
        float maxMas = 20000 * mainClass.Earth.SpaceIndustry.GetTotalOutput();
        float mas = 0;
        for(int i=0; i<res.Length; i++)
        {
            if(mas < maxMas)
            {
                mainClass.Player.PlusAmountRes(i, res[i]);
                mas += res[i];
                res[i] = 0;
            }
        }
    }

    public void PlusRes(int id, float value)
    {
        res[id] += value;
        UpdateText();
    }

    public float GetRes(int id)
    {
        return res[id];
    }
}
