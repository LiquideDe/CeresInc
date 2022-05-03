using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainResPanel : MonoBehaviour
{
    public List<Text> texts = new List<Text>();
    //0 - дата, 1 - деньги, 2 - рабочие, 3 - эквип, 4 - еда, 5 - энергия
    public void UpdateText(int id, string text)
    {
        texts[id].text = text;
    }
}
