using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidForPlayer : Asteroid, IAsteroid
{
    public float SizeofCircle { get; set; }
    
    //корпорация которая владеет
    [SerializeField] private GameObject asteroidGameObject, circle;
    [SerializeField] private AsteroidPanel asterPanel;
    [SerializeField] private Rigidbody rigidbodyAster;
    [SerializeField] private Outline outline;

    public Outline Outline { get { return outline; } }
    public GameObject Circle { get { return circle; } }
    [SerializeField] private Transform asteroidModel;
    public LODGroup lod;
    public MiningStation MiningStation { get; set; }
    public Transform AsteroidModel
    {
        get { return asteroidModel; }
    }
    public GameObject AsteroidGameObject
    {
        get { return asteroidGameObject; }
    }
    public AsteroidPanel AsterPanel
    {
        get { return asterPanel; }
    }

    public int Workers { get { return MiningStation.WorkersOnStation; } }
    public float Food { get { return MiningStation.Food; } }
    public float Equipment { get { return MiningStation.Equipment; } }
    public float IncomeLastMonth { get { return MiningStation.IncomeLastMonth; } }
    public float ExcavatedSoil { get { return MiningStation.ExcavatedSoil; } }
    public float AmountReadyForLoading { get { return MiningStation.AmountReadyForLoading; } }

    public int WorkersPlanned { get { return MiningStation.WorkersPlanned; } }

    public void OnAsteroid()
    {
        lod.gameObject.SetActive(true);
    }

    public void OffAsteroid()
    {
        lod.gameObject.SetActive(false);
    }

    private void Start()
    {
        HasMiningStation = false;
    }

    
    private void FixedUpdate()
    {
        Quaternion deltaRotation = Quaternion.Euler((new Vector3(0, 2, 0)) * Time.fixedDeltaTime);
        rigidbodyAster.MoveRotation(rigidbodyAster.rotation * deltaRotation);
    }

    protected override void SaveAnotherData(SaveLoadAsteroid save)
    {
        save.quatW = AsteroidModel.rotation.w;
        save.quatX = AsteroidModel.rotation.x;
        save.quatY = AsteroidModel.rotation.y;
        save.quatZ = AsteroidModel.rotation.z;
        save.scaleX = AsteroidModel.localScale.x;
        save.scaleY = AsteroidModel.localScale.y;
        save.scaleZ = AsteroidModel.localScale.z;
        save.sizeofCircle = SizeofCircle;

        if (HasMiningStation)
        {
            save.excavatedSoil = MiningStation.ExcavatedSoil;
            save.food = MiningStation.Food;
            save.equipment = MiningStation.Equipment;
            save.foodPlanned = MiningStation.FoodPlanned;
            save.equipmentPlanned = MiningStation.EquipmentPlanned;
            save.incomeLastMonth = MiningStation.IncomeLastMonth;
            save.amountReadyForLoading = MiningStation.AmountReadyForLoading;
            save.workersOnStation = MiningStation.WorkersOnStation;
            save.awaitingWorkers = MiningStation.AwaitingWorkers;
            save.workersPlanned = MiningStation.WorkersPlanned;
        }
    }

    protected override void LoadAnotherData(SaveLoadAsteroid save)
    {
        AsteroidModel.position = new Vector3(Position.x, Position.y, Position.z);
        AsteroidModel.rotation = new Quaternion(save.quatX, save.quatY, save.quatZ, save.quatW);
        AsteroidModel.localScale = new Vector3(save.scaleX, save.scaleY, save.scaleZ);
        circle.transform.localScale = new Vector3(save.sizeofCircle, save.sizeofCircle, save.sizeofCircle);

        if (save.hasMiningStation)
        {
            Debug.Log($"Настраиваем построенную станцию");
            asterPanel.CreateEmptyColony(this);
            MiningStation.ExcavatedSoil = save.excavatedSoil;
            MiningStation.Food = save.food;
            MiningStation.Equipment = save.equipment;
            MiningStation.FoodPlanned = save.foodPlanned;
            MiningStation.EquipmentPlanned = save.equipmentPlanned;
            MiningStation.IncomeLastMonth = save.incomeLastMonth;
            MiningStation.AmountReadyForLoading = save.amountReadyForLoading;
            MiningStation.WorkersOnStation = save.workersOnStation;
            MiningStation.AwaitingWorkers = save.awaitingWorkers;
            MiningStation.WorkersPlanned = save.workersPlanned;
        }
    }

    public void EmbedAgent()
    {
        IsAgentEmbedded = true;
    }

    public void Sabotage(int amount)
    {
        MiningStation.WorkersOnStation -= amount;

    }
}
