using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelWithElements : MonoBehaviour
{
    private List<CellOfElements> cellsOfElements = new List<CellOfElements>();
    [SerializeField] GameObject exampleElement;
    [SerializeField] main mainClass;
    

    public void CreateList()
    {
        for(int i=0;i<mainClass.Materials.MaterialsCount() - 1; i++)
        {
            CreateCell(mainClass.Materials.GetMaterial(i).ElementName, i);
        }
    }

    private void CreateCell(string name, int id)
    {
        GameObject gameObject = Instantiate(exampleElement);
        gameObject.SetActive(true);
        CellOfElements cell = gameObject.GetComponent<CellOfElements>();
        cell.Id = id;
        cell.TextName.text = name;
        cell.Main = mainClass;
        gameObject.transform.SetParent(this.transform);
        cellsOfElements.Add(cell);
    }
}
