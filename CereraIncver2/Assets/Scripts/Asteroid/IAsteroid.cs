using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAsteroid
{
    public int Workers { get; }
    public int WorkersPlanned { get; }
    public float Food { get;}
    public float Equipment { get;}
    public float ElementAbundance { get; }
    public float IncomeLastMonth { get; }
    public float ExcavatedSoil { get; }
    public float AmountReadyForLoading { get; }
    public float ElementCapacity { get; }
    public string AsterName { get; }
    public int Id { get; }
    public Vector3 Position { get; }

}
