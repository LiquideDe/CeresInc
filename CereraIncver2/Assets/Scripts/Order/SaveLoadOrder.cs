using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SaveLoadOrder 
{
	public List<int> carcassPas = new List<int>();
	public List<int> carcassCargo = new List<int>();
	public List<int> fuelTank = new List<int>();
	public List<int> engine = new List<int>();
	public string textRemain, textJourney, textMas;
	public int id, masAll, maxMas, food, equipment, timeRemain, worker;
	public bool atJourney = false;
	public float costAll;
}
