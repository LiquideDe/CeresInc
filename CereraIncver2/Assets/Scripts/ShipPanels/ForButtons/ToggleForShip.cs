using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class ToggleForShip : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] GameObject greenLight, redLight, pushGreen, pushRed;
    [SerializeField] protected Ship ship;   

    public void SetShip(Ship ship)
    {
        this.ship = ship;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (greenLight.activeSelf)
        {
            PushingFromGreen();
        }
        else
        {
            PushingFromRed();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (pushGreen.activeSelf)
        {
            UnPushingRed();
            TurningOff();
        }
        else if(TurningOn())
        {
            UnPushingGreen();

        }
        else
        {
            UnPushingRedFromRed();
        }
    }

    public void PushingFromGreen()
    {
        greenLight.SetActive(false);
        pushGreen.SetActive(true);
        
    }

    public void PushingFromRed()
    {
        redLight.SetActive(false);
        pushRed.SetActive(true);
    }

    private void UnPushingGreen()
    {
        pushRed.SetActive(false);
        greenLight.SetActive(true);
        if (redLight.activeSelf)
        {
            redLight.SetActive(false);
        }
    }

    public void UnPushingRed()
    {
        pushGreen.SetActive(false);
        redLight.SetActive(true);
        if (greenLight.activeSelf)
        {
            greenLight.SetActive(false);
        }
    }

    public void UnPushingRedFromRed()
    {
        pushRed.SetActive(false);
        redLight.SetActive(true);
    }

    protected abstract bool TurningOn();
    protected abstract void TurningOff();

}
