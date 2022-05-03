using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeresPanel : Spisok
{
    public List<Sprite> imgsModule = new List<Sprite>();
    public List<Sprite> imgsResources = new List<Sprite>();
    [SerializeField] Ceres ceres;
    
    protected override void BuildElement(int id, string text)
    {
        RectTransform clone = Instantiate(element);
        clone.SetParent(scroll.content);
        clone.localScale = Vector3.one;
        clone.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
        CeresButton item = clone.GetComponent<CeresButton>();
        SpaceStation module = ceres.GetSpaceStation(id);
        item.Id = id;
        item.Module = module;
        item.imgLogo.sprite = imgsModule[module.Type];
        item.imgRes.sprite = imgsResources[module.Type];
        item.SetName(text);
        buttons.Add(clone);
        item.UpdateText();
    }

    protected override void UpdateList(int id)
    {
        throw new System.NotImplementedException();
    }

    public void UpdateText()
    {
        for(int i = 0; i < buttons.Count; i++)
        {
            buttons[i].GetComponent<CeresButton>().UpdateText();
        }
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }
}
