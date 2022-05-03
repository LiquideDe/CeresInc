using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class Spisok : MonoBehaviour
{
    [SerializeField] protected ScrollRect scroll;
    [SerializeField] protected RectTransform element; // ������ �� ������� ����� ��������� ������
    protected int offset = 10; // ���������� ����� ����������
    protected Vector2 delta;
    protected Vector2 e_Pos;
    protected List<RectTransform> buttons = new List<RectTransform>();
    protected int size;
    protected float curY, vPos;

    public void Create_Button(int id, string text = "")
    {
        delta = element.sizeDelta;
        delta.y += offset;
        //e_Pos = new Vector2(0, -delta.y / 2);
        e_Pos = new Vector2(0, 0);
        AddToList(id, text);

    }

    protected void AddToList(int id, string text)
    {
        element.gameObject.SetActive(true);
        vPos = scroll.verticalNormalizedPosition;
        curY = 0;
        size++;
        RectContent();
        foreach (RectTransform b in buttons)
        {
            b.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
            curY += delta.y;
        }
        BuildElement(id, text);
        element.gameObject.SetActive(false);
    }

    protected void RectContent() // ����������� ������� ���� � ����������
    {
        float height = delta.y * size;
        scroll.content.sizeDelta = new Vector2(scroll.content.sizeDelta.x, height);
        scroll.content.anchoredPosition = Vector2.zero;
    }

    protected abstract void BuildElement(int id, string text);

    protected abstract void UpdateList(int id); // ������� �������� ��������

        /*
    {
        vPos = scroll.verticalNormalizedPosition; // ���������� ������� �������
        int j = 0;
        ShipButton item = null;
        foreach (RectTransform b in buttons)
        {
            item = b.GetComponent<ShipButton>();
            if (item.nameShip.text == name) break; // ������� ������ �������
            j++;
        }
        Destroy(item.gameObject); // ������� ���� ������� �� ������
        buttons.RemoveAt(j); // ������� ���� ������� �� �������
        curY = 0;
        size--; // ����� ���� �������
        RectContent(); // ������������� ������� ����
        foreach (RectTransform b in buttons) // �������� ��������
        {
            b.anchoredPosition = new Vector2(e_Pos.x, e_Pos.y - curY);
            curY += delta.y;
        }
        scroll.verticalNormalizedPosition = vPos; // ���������� ������� �������
        
    }*/

    public void ClearList()
    {
        vPos = scroll.verticalNormalizedPosition;
        for (int i = 0; i < buttons.Count; i++)
        {
            Destroy(buttons[i].gameObject);
        }
        buttons.Clear();
        curY = 0;
        size = 0;
        RectContent();
    }

}
