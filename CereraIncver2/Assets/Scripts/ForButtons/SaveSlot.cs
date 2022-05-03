using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveSlot : MonoBehaviour, IPointerClickHandler
{
    public Text nameText, dateText;
    public SaveGame saveGame;

    public void OnPointerClick(PointerEventData eventData)
    {
        saveGame.CanSave(nameText.text);
    }
}
