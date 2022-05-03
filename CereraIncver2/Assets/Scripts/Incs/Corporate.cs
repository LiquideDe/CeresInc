using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Corporate: MonoBehaviour
{

    [SerializeField] main main;
    [SerializeField] Image image;
    public string CorpName { get; set; }
    public main MainClass { get { return main; } }
    public float Money { get; set; }
    public Image Logo { get { return image; } }
    
}
