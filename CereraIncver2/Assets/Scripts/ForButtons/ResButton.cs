using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResButton : MonoBehaviour
{
    public Text nameElement, amountRes, textTake;
    public Slider resSlider;
    public int asterId, id;
    public CargoShip ship;

    public void ChangeSlider()
    {
        if (!ship.Navigator.IsInJourney)
        {
            ship.Navigator.GetOldDestination(asterId).ReadyForLoad(resSlider.value);
            textTake.text = $"Готов к отгрузке {ship.Navigator.GetOldDestination(asterId).AmountReadyForLoading}";
            ship.CalculatedV();
            ship.UpdateText();
        }
    }
}
