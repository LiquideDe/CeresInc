using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewsPanel : Spisok
{
    [SerializeField] private main mainClass;
    protected override void BuildElement(int id, string text)
    {
        RectTransform clone = Instantiate(element);
        clone.SetParent(scroll.content);
        clone.localScale = Vector3.one;
        clone.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
        DailyNews item = clone.GetComponent<DailyNews>();
        item.TextDate.text = $"{mainClass.GUI.GetDayMonthYear()}";
        item.TextNews.text = text;
        buttons.Add(clone);
    }

    protected override void UpdateList(int id)
    {
        throw new System.NotImplementedException();
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
