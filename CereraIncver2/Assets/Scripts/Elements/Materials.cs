using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materials
{
    private List<Resource> materials = new List<Resource>();
    Resource H = new Resource(0, "H", 10000, 100, 1);
    Resource He = new Resource(1, "He", 3000, 50, 2);
    Resource Li = new Resource(2, "Li", 4000, 100, 10);
    Resource Be = new Resource(3, "Be", 3000, 20, 5);
    Resource C = new Resource(4, "C", 10000, 0, 5);
    Resource N = new Resource(5, "N", 2500, 0, 5);
    Resource Mg = new Resource(6, "Mg", 5000, 50, 5);
    Resource Al = new Resource(7, "Al", 10000, 200, 6);
    Resource Si = new Resource(8, "Si", 8000, 0, 4);
    Resource Ne = new Resource(9, "Ne", 2100, 30, 1);
    Resource Ti = new Resource(10, "Ti", 8000, 100, 10);
    Resource Cr = new Resource(11, "Cr", 7000, 20, 5);
    Resource Fe = new Resource(12, "Fe", 1500, 130, 2);
    Resource Ni = new Resource(13, "Ni", 3200, 50, 6);
    Resource Cu = new Resource(14, "Cu", 12000, 50, 3);
    Resource Xe = new Resource(15, "Xe", 2500, 0, 2);
    Resource Ir = new Resource(16, "Ir", 600, 0, 8);
    Resource Pt = new Resource(17, "Pt", 700, 50, 9);
    Resource Po = new Resource(18, "Po", 100, 0, 9);
    Resource Th = new Resource(19, "Th", 700, 50, 12);
    Resource U = new Resource(20, "U", 900, 0, 7);
    Resource Pu = new Resource(21, "Pu", 100, 0, 9);
    Resource Ea = new Resource(22, "Ea", 15000, 0, 1);

    public Materials()
    {
        materials.Add(H);
        materials.Add(He);
        materials.Add(Li);
        materials.Add(Be);
        materials.Add(C);
        materials.Add(N);
        materials.Add(Mg);
        materials.Add(Al);
        materials.Add(Si);
        materials.Add(Ne);
        materials.Add(Ti);
        materials.Add(Cr);
        materials.Add(Fe);
        materials.Add(Ni);
        materials.Add(Cu);
        materials.Add(Xe);
        materials.Add(Ir);
        materials.Add(Pt);
        materials.Add(Po);
        materials.Add(Th);
        materials.Add(U);
        materials.Add(Pu);
        materials.Add(Ea);
    }

    public Resource GetMaterial(int id)
    {
        return materials[id];
    }

    public int MaterialsCount()
    {
        return materials.Count;
    }

    public Resource GetRandomMaterial()
    {
        return materials[Random.Range(0,materials.Count - 1)];
    }

    public void WriteConsumption(List<float> cons)
    {
        for(int i = 0; i < materials.Count; i++)
        {
            materials[i].Consumption = cons[i];
        }
    }

    public void CleanExcavatedLastMonth()
    {
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].ExcavatedAtLastMonth = 0;
        }
    }
}
