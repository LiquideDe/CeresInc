using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICorporateConsumption
{
    public float GetConsumption(int id);
    public float Price { get; set; }
    public float GoodsAtEarth { get; set; }
    public float GetTotalOutput();
    public float GetReservedOutput();
    public void ReservedOutput(float value);
}
