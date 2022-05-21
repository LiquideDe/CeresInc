using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public abstract class Asteroid : MonoBehaviour
{
    public string AsterName { get; set; } //имя
    public Resource Element { get; set; } //ресурс1
    //количество ресурса1, распространенность ресурса1. Распространенность - количество ресурса на 1 тонну грунта, добыто грунта с ресурсом1
    public float ElementCapacity { get; set; }
    public float ElementAbundance { get; set; }    
    public float Distance { get; set; }
    public int Id { get; set; }
    public Vector3 Position { get; set; }
    public bool HasMiningStation { get; set; }

    public void SaveData(SaveLoadAsteroid save)
    {
        save.asterName = AsterName;
        save.elementAbundance = ElementAbundance;
        save.idElement = Element.Id;
        save.elementCapacity = ElementCapacity;
        save.id = Id;
        save.posX = Position.x;
        save.posY = Position.y;
        save.posZ = Position.z;
        save.hasMiningStation = HasMiningStation;
        save.distance = Distance;

        SaveAnotherData(save);        
    }

    protected abstract void SaveAnotherData(SaveLoadAsteroid save);

    public void LoadData(SaveLoadAsteroid save, Resource element)
    {
        AsterName = save.asterName;
        ElementAbundance = save.elementAbundance;
        Element = element;
        ElementCapacity = save.elementCapacity;
        Id = save.id;
        Position = new Vector3(save.posX, save.posY, save.posZ);
        transform.position = new Vector3(save.posX, save.posY, save.posZ);
        HasMiningStation = save.hasMiningStation;
        Distance = save.distance;
        LoadAnotherData(save);
    }

    protected abstract void LoadAnotherData(SaveLoadAsteroid save);
}
