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

    public IEnumerator CreateCorporates(bool isNewGame)
    {
        
        for(int i = 0;i < mainClass.Materials.MaterialsCount() - 1; i++)
        {
            CreateMiningCorporate(mainClass.Materials.GetMaterial(i), isNewGame);
        }
        Debug.Log($"?????????? ???????");
        yield return new WaitForSeconds(0.01f);
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
    public MiningCorporate GetMiningCorporates(int id)
    {
        return miningCorporates[id];
    }
    public int CountMiningCorp()
    {
        return miningCorporates.Count;
    }
}
