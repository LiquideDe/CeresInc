using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadSlot : Spisok
{
    public LoadGame loadGame;
    protected override void BuildElement(int id, string text)
    {
        RectTransform clone = Instantiate(element);
        clone.SetParent(scroll.content);
        clone.localScale = Vector3.one;
        clone.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
        LoadButton item = clone.GetComponent<LoadButton>();
        item.textName.text = text.Substring(text.LastIndexOf('\\') + 1, text.Length - 5 - text.LastIndexOf('\\'));
        item.fullPath = text;
        item.textDate.text = loadGame.LoadHeading(text);
        buttons.Add(clone);
    }

    protected override void UpdateList(int id)
    {
        
    }
}
