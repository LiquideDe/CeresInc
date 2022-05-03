using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CeresPanelWorkerButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Image imgButton;
    [SerializeField] Sprite WorkerDown, WorkerUp;
    [SerializeField] Ceres ceres;
    [SerializeField] CeresButton ceresButton;

    public void OnPointerDown(PointerEventData eventData)
    {
        imgButton.sprite = WorkerDown;
        if (Input.GetMouseButtonDown(0))
        {
            ceres.ChangeAmountWorkers(1, ceresButton.Id);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ceres.ChangeAmountWorkers(-1, ceresButton.Id);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        imgButton.sprite = WorkerUp;
    }
}
