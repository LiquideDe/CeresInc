using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListRoutes : Spisok
{
    private List<RouteBut> routeButs = new List<RouteBut>();
    [SerializeField] private ShipRoutes routes;
    protected override void BuildElement(int id, string text)
    {
        RectTransform clone = Instantiate(element);
        clone.SetParent(scroll.content);
        clone.localScale = Vector3.one;
        clone.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
        RouteBut item = clone.GetComponent<RouteBut>();
        item.Route = routes.GetRoute(id);
        item.Id = id;
        item.UpdateText();
        SetNewIdToResPanels();
        buttons.Add(clone);
        routeButs.Add(item);
    }

    protected override void UpdateList(int id)
    {
        vPos = scroll.verticalNormalizedPosition; // запоминаем позицию скролла

        Destroy(routeButs[id].gameObject); // удаляем этот элемент из списка
        routeButs.RemoveAt(id); // удаляем этот элемент из массива
        buttons.RemoveAt(id);
        curY = 0;
        foreach (RouteBut b in routeButs) // сдвигаем элементы
        {
            b.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
            curY += delta.y;
        }
        size--; // минус один элемент
        RectContent(); // пересчитываем размеры окна
        SetNewIdToResPanels();
        scroll.verticalNormalizedPosition = vPos; // возвращаем позицию скролла
    }

    private void SetNewIdToResPanels()
    {
        for (int i = 0; i < routeButs.Count; i++)
        {
            routeButs[i].Id = i;
        }
    }

    public void UpdateText()
    {
        for(int i = 0; i < routeButs.Count; i++)
        {
            routeButs[i].UpdateText();
        }
    }

    public void RemoveRoute(int id)
    {
        UpdateList(id);
    }
}
