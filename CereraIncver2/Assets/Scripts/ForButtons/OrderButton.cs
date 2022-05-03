using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OrderButton : MonoBehaviour, IPointerClickHandler
{
	[SerializeField] private Text textRemain;
	private int timeRemain;
	public Text textJourney, textMas;
	public main mainClass;
	public int id, masAll, maxMas,food, equipment, workers;
	public bool atJourney = false;
	public float costAll;
	public List<int> carcassPas = new List<int>();
	public List<int> carcassCargo = new List<int>();
	public List<int> fuelTank = new List<int>();
	public List<int> engine = new List<int>();
	[SerializeField] PanelOrder panelOrder;
	public void OnPointerClick(PointerEventData eventData)
	{
		panelOrder.PressDelivery(id);
	}

	public void UpdateText()
    {
		timeRemain = (int)(mainClass.DayBeforeArrival + 460 * id);
		textRemain.text = $"{timeRemain}";
	}

	public void SaveData(SaveLoadOrder save)
    {
        save.atJourney = atJourney;
        save.costAll = costAll;
        save.equipment = equipment;
        save.food = food;
        save.id = id;
        save.masAll = masAll;
        save.maxMas = maxMas;
        save.textJourney = textJourney.text;
        save.textMas = textMas.text;
        save.textRemain = textRemain.text;
        save.timeRemain = timeRemain;
        save.worker = workers;

        CopyDateFromFirstArrayToSecond(carcassPas, save.carcassPas);
        CopyDateFromFirstArrayToSecond(carcassCargo, save.carcassCargo);
        CopyDateFromFirstArrayToSecond(fuelTank, save.fuelTank);
        CopyDateFromFirstArrayToSecond(engine, save.engine);
        
    }

    private void CopyDateFromFirstArrayToSecond(List<int> firstArray, List<int> secondArray)
    {
        for(int i = 0; i < firstArray.Count; i++)
        {
            secondArray.Add(firstArray[i]);
        }
    }

    public void LoadData(SaveLoadOrder save)
    {
        atJourney = save.atJourney;
        costAll = save.costAll;
        equipment = save.equipment;
        food = save.food;
        id = save.id;
        masAll = save.masAll;
        maxMas = save.maxMas;
        textJourney.text = save.textJourney;
        textMas.text = save.textMas;
        textRemain.text = save.textRemain;
        timeRemain = save.timeRemain;
        workers = save.worker;

        CopyDateFromFirstArrayToSecond(save.carcassPas, carcassPas);
        CopyDateFromFirstArrayToSecond(save.carcassCargo, carcassCargo);
        CopyDateFromFirstArrayToSecond(save.fuelTank, fuelTank);
        CopyDateFromFirstArrayToSecond(save.engine, engine);
    }

}
