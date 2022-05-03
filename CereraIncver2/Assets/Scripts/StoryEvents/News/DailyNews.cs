using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyNews : MonoBehaviour
{
    [SerializeField] Text textDate, textNews;
    public Text TextNews { get { return textNews; } }
    public Text TextDate { get { return textDate; } }
}
