using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CorpButton : MonoBehaviour, IPointerDownHandler
{
    public Text textName;
    public CorpPanel corpPanel;
    public int id;

    public void OnPointerDown(PointerEventData eventData)
    {
        corpPanel.UpdateText(id);
    }
}
