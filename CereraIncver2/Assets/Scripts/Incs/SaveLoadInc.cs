using System;
using System.Collections.Generic;

[Serializable]
public class SaveLoadInc
{
    public string nameInc;
    public float money, equipment, food;
    public int id;
    public float[] amountRes = new float[23];
    public List<int> carcassPas = new List<int>();
    public List<int> carcassCargo = new List<int>();
    public List<int> fuelTank = new List<int>();
    public List<int> engine = new List<int>();
    public List<int> asteroids = new List<int>();
    

}
