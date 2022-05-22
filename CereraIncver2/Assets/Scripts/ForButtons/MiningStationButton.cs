using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MiningStationButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] PanelStation panelStation;
    [SerializeField] Text nameBut;
    public Text NameBut { get { return nameBut; } }
    public MiningCorporate Corp { get; set; }
    
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log($"Делаем тыщ, корпорация = {Corp.CorpName}");
        List<IAsteroid> list = new List<IAsteroid>();
        if (Corp == null)
        {
            list.AddRange(panelStation.mainClass.Player.GetAllAsteroids());
            
        }
        else
        {           
            list.AddRange(Corp.MiningDepartment.GetAllAsteroids());
        }
        panelStation.BuildStationList(list);

    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    
}
