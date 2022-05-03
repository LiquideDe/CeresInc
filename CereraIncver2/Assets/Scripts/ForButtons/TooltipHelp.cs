using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TooltipHelp : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string txt;
    public Vector2 pos;
    public RectTransform tooltip;
    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltip.anchoredPosition = new Vector2(pos.x, pos.y);
        tooltip.GetChild(1).GetComponent<Text>().text = txt;
        tooltip.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltip.gameObject.SetActive(false);
    }

}
