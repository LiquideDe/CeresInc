using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainResPanel : MonoBehaviour
{
    public List<Text> texts = new List<Text>();
    //0 - ����, 1 - ������, 2 - �������, 3 - �����, 4 - ���, 5 - �������
    public void UpdateText(int id, string text)
    {
        texts[id].text = text;
    }
}
