using System;

[Serializable]
public class SaveLoadAsteroid
{
    public float posX, posY, posZ, elementCapacity, elementAbundance, distance, sizeofCircle, quatW, quatX, quatY, quatZ, scaleX, scaleY, scaleZ;
    public bool hasMiningStation;
    public string asterName;
    public int idElement, id;

    public float excavatedSoil, food, equipment, equipmentPlanned, incomeLastMonth, amountReadyForLoading;
    public int workersOnStation, awaitingWorkers, workersPlanned, foodPlanned;
    public bool isInRoute;

}
