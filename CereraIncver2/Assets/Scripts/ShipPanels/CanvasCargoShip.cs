using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCargoShip : Spisok, IToggleShip
{
    [SerializeField] ToggleStartForShip toggleStart;
    [SerializeField] ToggleRepeatForShip toggleRepeat;
    [SerializeField] CargoShip ship;
    [SerializeField] Text textCargoDestination, textDvAndOther;
    public ToggleForShip ToggleStart { get { return toggleStart; } }
    public ToggleForShip ToggleRepeat { get { return toggleRepeat; } }

    private List<ResButton> resPanels = new List<ResButton>();

    protected override void BuildElement(int id, string text)
    {
        RectTransform clone = Instantiate(element);
        clone.SetParent(scroll.content);
        clone.localScale = Vector3.one;
        clone.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
        ResButton item = clone.GetComponent<ResButton>();
        item.nameElement.text = ship.Navigator.GetOldDestination(id).Asteroid.AsterName;
        item.amountRes.text = $"{ship.Navigator.GetOldDestination(id).Asteroid.ElementCapacity}";
        item.asterId = id;
        item.id = resPanels.Count;
        item.ship = ship;
        Debug.Log($"item.id = {item.id}, asterId = {item.asterId}");
        resPanels.Add(item);
        buttons.Add(clone);
    }

    protected override void UpdateList(int id)
    {
        vPos = scroll.verticalNormalizedPosition; // запоминаем позицию скролла

        Destroy(resPanels[id].gameObject); // удаляем этот элемент из списка
        resPanels.RemoveAt(id); // удаляем этот элемент из массива
        buttons.RemoveAt(id);
        curY = 0;
        foreach (ResButton b in resPanels) // сдвигаем элементы
        {
            b.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
            curY += delta.y;
        }
        size--; // минус один элемент
        RectContent(); // пересчитываем размеры окна
        SetNewIdToResPanels();
        scroll.verticalNormalizedPosition = vPos; // возвращаем позицию скролла
    }

    public void DeleteResButton(int id)
    {
        UpdateList(id);
    }
    private void SetNewIdToResPanels()
    {
        for (int i = 0; i < resPanels.Count; i++)
        {
            resPanels[i].id = i;
        }
    }

    public void UpdateText()
    {
            textCargoDestination.text = $"";
        if (ship.Navigator.OldDestinationsCount() > 0)
        {
            for (int i = 0; i < ship.Navigator.OldDestinationsCount(); i++)
            {
                textCargoDestination.text += $"{ship.Navigator.GetOldDestination(i).Asteroid.AsterName}, ";
            }
            for (int i = 0; i < resPanels.Count; i++)
            {
                resPanels[i].amountRes.text = $"{ship.Navigator.GetOldDestination(resPanels[i].asterId).ExcavatedSoil}";
            }
        }            
        textDvAndOther.text = $"dV равен {ship.Navigator.DvToOperation}, корабль вернется через {ship.Navigator.TimeToJourney}";       

    }
}
