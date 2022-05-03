using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LoadButton : MonoBehaviour, IPointerClickHandler
{
    public Text textName, textDate;
    public string fullPath;
    public LoadGame load;
    public void OnPointerClick(PointerEventData eventData)
    {
        load.ChooseLoad(fullPath);
    }
}
