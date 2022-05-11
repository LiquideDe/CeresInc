using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corporates : MonoBehaviour
{
    private List<MiningCorporate> miningCorporates = new List<MiningCorporate>();
    [SerializeField] private Transform corporateList;
    [SerializeField] private CorpPanel corpPanel;
    [SerializeField] private GameObject corporateTemplate;
    [SerializeField] private main mainClass;

    public void CreateCorporates(bool isNewGame)
    {
        /*
        for(int i=0;i<1; i++)
        {
            CreateMiningCorporate(mainClass.Materials.GetMaterial(i), isNewGame);
        }    */    
    }
    public MiningCorporate GetMiningCorporates(int id)
    {
        return miningCorporates[id];
    }
    public int CountMiningCorp()
    {
        return miningCorporates.Count;
    }

    private void CreateMiningCorporate(Resource element, bool isNewGame)
    {
        GameObject gameObject = Instantiate(corporateTemplate);
        gameObject.transform.SetParent(corporateList);
        MiningCorporate corporate = gameObject.GetComponent<MiningCorporate>();
        corporate.CorpName = $"{element.ElementName}Inc";
        corporate.OrientRes = element;
        miningCorporates.Add(corporate);
        gameObject.SetActive(true);
        corpPanel.Create_Button(miningCorporates.Count + 4, corporate.CorpName);
        corpPanel.AddToShareList(corporate);
        if (isNewGame)
        {
            corporate.StartNewGame();
        }
    } 
    
    public void NewRound()
    {
        for (int i = 0; i < miningCorporates.Count; i++)
        {
            miningCorporates[i].ShipDepartment.NewRound();
            miningCorporates[i].FinanceDepartment.CalculationPriceShare();
        }
    }

    public void Arrival()
    {
        for(int i = 0; i < miningCorporates.Count; i++)
        {
            miningCorporates[i].Arrival();
        }
    }
}
