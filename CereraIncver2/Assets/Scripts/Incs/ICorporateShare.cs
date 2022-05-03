using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICorporateShare
{
    public string CorpName { get;}
    public float Money { get; set; }
    public float PriceShare { get;}
    public float Price { get; }
}
