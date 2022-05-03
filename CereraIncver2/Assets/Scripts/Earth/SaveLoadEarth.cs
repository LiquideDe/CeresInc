using System;
using System.Collections.Generic;

[Serializable]
public class SaveLoadEarth
{
    public int population, economyHealth;
    public float gdp, heavyGoods, lightGoods, highGoods, spaceGoods;
    public float[] amountRes = new float[23];
    public List<float> totalConsumption = new List<float>();
}

