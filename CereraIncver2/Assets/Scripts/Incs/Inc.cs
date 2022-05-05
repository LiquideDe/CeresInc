using System.Collections;
using System.Collections.Generic;

public class Inc 
{
    public string NameInc { get; set; }
    public float Money { get; set; }
    public int Id { get; set; }
    private float[] amountRes = new float[23];
    public float Equipment { get; set; }
    public float Food { get; set; }
    public int SumCarcassPas { get { return CountDevice(carcassPas); } }
    public int SumCarcassCargo { get { return CountDevice(carcassCargo); } }
    public int SumFuelTank { get { return CountDevice(fuelTank); } }
    public int SumEngine { get { return CountDevice(engine); } }
    private List<AsteroidForPlayer> asteroids = new List<AsteroidForPlayer>();
    private List<int> carcassPas = new List<int>();
    private List<int> carcassCargo = new List<int>();
    private List<int> engine = new List<int>();
    private List<int> fuelTank = new List<int>();

    public Inc(string name, float money)
    {
        NameInc = name;
        Money = money;
        Equipment = 10000;
        Food = 10000;
    }

    private int CountDevice(List<int> device)
    {
        int sum = 0;
        for (int i = 0; i < device.Count; i++)
        {
            sum += device[i];
        }
        return sum;
    }

    public int CountCarcassPas(int id = -1)
    {
        //Если мы получаем id то пишем количество техники в ячейке, если не получаем, то даем количество ячеек, а не что в них.
        if (id >= 0)
        {
            return carcassPas[id];
        }
        else
            return carcassPas.Count;
    }

    public int CountCarcassCargo(int id = -1)
    {
        if (id >= 0)
        {
            return carcassCargo[id];
        }
        else
            return carcassCargo.Count;
    }

    public int CountEngine(int id = -1)
    {
        if (id >= 0)
        {
            return engine[id];
        }
        else
            return engine.Count;
    }

    public int CountFuelTank(int id = -1)
    {
        ///<summary>
        ///Без параметров общее количество типов, с параметрами количество именно по индексу 
        ///</summary>
        if (id >= 0)
        {
            return fuelTank[id];
        }
        else
            return fuelTank.Count;
    }

    public void PlusCarcassPas(int id, int value)
    {
        if(id >= carcassPas.Count)
        {
            carcassPas.Add(value);
        }
        else
            carcassPas[id] += value;
    }

    public void PlusCarcassCargo(int id, int value)
    {
        if (id >= carcassCargo.Count)
        {
            carcassCargo.Add(value);
        }
        else
            carcassCargo[id] += value;
    }

    public void PlusEngine(int id, int value)
    {
        if (id >= engine.Count)
        {
            engine.Add(value);
        }
        else
            engine[id] += value;
    }

    public void PlusFuelTank(int id, int value)
    {
        if (id >= fuelTank.Count)
        {
            fuelTank.Add(value);
        }
        else
            fuelTank[id] += value;
    }

    public float GetAmountRes(int id)
    {
        return amountRes[id];
    }

    public int CountLengthAmountRes()
    {
        return amountRes.Length;
    }

    public void PlusAmountRes(int id, float value)
    {
        amountRes[id] += value;
    }

    public AsteroidForPlayer GetAsteroid(int id)
    {
        return asteroids[id];
    }

    public void PlusAsteroid(AsteroidForPlayer asteroid)
    {
        asteroids.Add(asteroid);
    }

    public int CountAsteroids()
    {
        return asteroids.Count;
    }

    public List<AsteroidForPlayer> GetAllAsteroids()
    {
        return asteroids;
    }

    public (float, float) EquipmentLeftFor()
    {
        float equipCons = 0;
        float equipDayLeft = 0;
        for (int i = 0; i < asteroids.Count; i++)
        {
            equipCons += asteroids[i].MiningStation.EquipmentConsuption * asteroids[i].MiningStation.WorkersOnStation;
        }
        if (equipCons > 0)
            equipDayLeft = Equipment / equipCons;

        return (equipCons, equipDayLeft);
    }

    public (float, float) FoodLeftFor()
    {
        float foodCons = 0;
        float foodDayLeft = 0;
        for (int i = 0; i < asteroids.Count; i++)
        {
            foodCons += asteroids[i].MiningStation.FoodConsuption * asteroids[i].MiningStation.WorkersOnStation;
        }
        if (foodCons > 0)
            foodDayLeft = Food / foodCons;

        return (foodCons, foodDayLeft);
    }

}
