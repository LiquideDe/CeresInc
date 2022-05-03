using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasPasShip : MonoBehaviour, IToggleShip
{
    [SerializeField] private Text textDestination, textDvAndOther, textAmountWorkers, textAmountEquipment, textAmountFood, textDemandWorker, textDemandEquipment, textDemandFood;
    [SerializeField] private Button butWorkPlus, butWorkMin, butEquipPlus, butEquipMin, butFoodPlus, butFoodMin;
    [SerializeField] private PassangerShip ship;
    [SerializeField] private ToggleForShip toggleStart, toggleRepeat;
    public ToggleForShip ToggleStart { get { return toggleStart; } }
    public ToggleForShip ToggleRepeat { get { return toggleRepeat; } }

    public void SetListernerToButs()
    {
        butWorkPlus.onClick.AddListener(() => PlusMinusCargoShip(0, 1));
        butWorkMin.onClick.AddListener(() => PlusMinusCargoShip(0, 0));
        butEquipPlus.onClick.AddListener(() => PlusMinusCargoShip(1, 1));
        butEquipMin.onClick.AddListener(() => PlusMinusCargoShip(1, 0));
        butFoodPlus.onClick.AddListener(() => PlusMinusCargoShip(2, 1));
        butFoodMin.onClick.AddListener(() => PlusMinusCargoShip(2, 0));

        //passangerCompartment.GetComponent<Rigidbody>().angularVelocity = Vector3.forward * 1.0f;
    }

    public void PlusMinusCargoShip(int type, int mp)
    {
        bool shiftPressed;
        shiftPressed = (Input.GetKey(KeyCode.LeftShift) ? true : false);
        //type определяет какой тип груза 0-рабочие, 2-оборудование, 3-еда, mp означает 0-минус, 1-плюс
        if (!ship.Navigator.IsInJourney)
        {
            switch (type)
            {
                case 0:
                    if (mp == 0 && ship.AwaitingCargoWorkers > 0)
                    {
                        ship.AwaitingCargoWorkers -= 1;
                        if (shiftPressed == true)
                        {
                            ship.AwaitingCargoWorkers = 0;
                        }
                            
                    }
                    else if (mp == 1)
                    {
                        ship.AwaitingCargoWorkers += 1;
                        if (shiftPressed == true)
                        {
                            ship.AwaitingCargoWorkers = ship.DemandWorkers;
                        }
                            
                    }
                    break;
                case 1:
                    if (mp == 0 && ship.AwaitingCargoEquipment > 0)
                    {
                        ship.AwaitingCargoEquipment -= 1;
                        ship.OwnedInc.Equipment += 1;
                        if (shiftPressed == true)
                        {
                            ship.OwnedInc.Equipment += ship.AwaitingCargoEquipment;
                            ship.AwaitingCargoEquipment = 0;
                        }

                    }
                    else if (mp == 1 && ship.OwnedInc.Equipment > 1)
                    {
                        ship.AwaitingCargoEquipment += 1;
                        ship.OwnedInc.Equipment -= 1;
                        if (shiftPressed == true && ship.OwnedInc.Equipment > ship.DemandEquip)
                        {
                            ship.AwaitingCargoEquipment = ship.DemandEquip;
                            ship.OwnedInc.Equipment -= ship.DemandEquip;
                        }
                    }
                    break;
                case 2:
                    if (mp == 0 && ship.CargoFood > 0)
                    {
                        ship.AwaitingCargoFood -= 1;
                        ship.OwnedInc.Food += 1;
                        if (shiftPressed == true)
                        {
                            ship.OwnedInc.Food += ship.AwaitingCargoFood;
                            ship.AwaitingCargoFood = 0;
                        }
                    }
                    else if (mp == 1 && ship.OwnedInc.Food > 1)
                    {
                        ship.AwaitingCargoFood += 1;
                        ship.OwnedInc.Food -= 1;
                        if (shiftPressed == true && ship.OwnedInc.Food > ship.DemandFood)
                        {
                            ship.AwaitingCargoFood = ship.DemandFood;
                            ship.OwnedInc.Food -= ship.DemandFood;
                        }
                    }
                    break;

            }
            ship.CalculateWeightPlayLoad();
            ship.CalculatedV();
            ship.Navigator.CalculateTime();
            UpdateText();
        }

    }

    public void UpdateText()
    {
        textDestination.text = $"";
        for (int i = 0; i < ship.Navigator.DestinationsCount(); i++)
        {
            textDestination.text += $"{ship.Navigator.GetOldDestination(i).Asteroid.AsterName},";
        }
        textDemandWorker.text = $"/требуется {ship.DemandWorkers}";
        textDemandEquipment.text = $"/требуется {ship.DemandEquip}";
        textDemandFood.text = $"/требуется {ship.DemandFood}";

        textDvAndOther.text = $"dV равен {ship.Navigator.DvToOperation}, корабль вернется через {ship.Navigator.TimeToJourney}, общее расстояние {ship.Navigator.Distance}";
        textAmountWorkers.text = $"{ship.AwaitingCargoWorkers}";
        textAmountEquipment.text = $"{ship.AwaitingCargoEquipment}";
        textAmountFood.text = $"{ship.AwaitingCargoFood}";
        if (ship.Navigator.DestinationsCount() > 0)
        {
            ship.ShipButton.UpdateText((int)ship.Navigator.DvToOperation, ship.CalculateAllMass(), ship.CostOfJourney, ship.Navigator.GetOldDestination(0).Asteroid.AsterName, ship.Navigator.TimeToJourney, ship.Age);
        }
        else
        {
            ship.ShipButton.UpdateText((int)ship.Navigator.DvToOperation, ship.CalculateAllMass(), ship.CostOfJourney, "", ship.Navigator.TimeToJourney, ship.Age);
        }
    }

    private void Start()
    {
        SetListernerToButs();
    }
}
