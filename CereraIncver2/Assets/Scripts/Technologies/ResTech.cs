using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResTech : Technology
{
    public Resource Element { get; set; }

    public ResTech(int costResearch, int updateType, int updateAmount, Resource resource, Sciense sciense)
    {
        Element = resource;
        NameTech = $"Improvement {Element.ElementName}";
        IsResearched = false;
        CostResearch = costResearch;
        Description = "";
        Sciense = sciense;
        FirstAmountUpdate = updateType;
        switch (updateType)
        {
            case 0:
                Description = $"���������� ����������������� {Element.ElementName} � �������������� ������� �� {updateAmount}%";
                break;
            case 1:
                Description = $"���������� ����������������� {Element.ElementName} � ������� �������������� �� {updateAmount}%";
                break;
            case 2:
                Description = $"���������� ����������������� {Element.ElementName} � ������ �������������� �� {updateAmount}%";
                break;
            case 3:
                Description = $"���������� ����������������� {Element.ElementName} � ������� �������������� �� {updateAmount}%";
                break;
            case 4:
                Description = $"���������� ����������������� {Element.ElementName} � ����������� �������������� �� {updateAmount}%";
                break;
        }
        SecondAmountUpdate = updateAmount;
        IsOutdated = false;
    }
    public ResTech(string techName, int costResearch, int firstUpdate, int secondUpdate, Resource resource, Sciense sciense)
    {
        Element = resource;
        NameTech = techName;
        CostResearch = costResearch;
        FirstAmountUpdate = firstUpdate;
        SecondAmountUpdate = secondUpdate;
        Sciense = sciense;
        Description = $"��������� ����� ���������� � ������ {Element.ElementName}, �� �������� �������� ������ �� {firstUpdate} � �������� ����� �������� �� ���������� �� {secondUpdate}%";

        IsOutdated = false;
    }

    public ResTech(Sciense sciense)
    {
        Sciense = sciense;
    }

    protected override void Research()
    {
        IsResearched = true;
        //Element.NewKpd(FirstAmountUpdate, SecondAmountUpdate);
        Sciense.ResTechResearched(Id);
    }

    protected override void SaveAnotherData(SaveLoadTechnology save)
    {
        save.elementId = Element.Id;
    }

    protected override void LoadAnotherData(SaveLoadTechnology save)
    {
        Element = Sciense.mainClass.Materials.GetMaterial(save.elementId);
    }
}
