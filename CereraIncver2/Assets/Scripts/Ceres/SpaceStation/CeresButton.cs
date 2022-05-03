using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CeresButton : MonoBehaviour
{
    public int Id { get; set; }
    public SpaceStation Module { get; set; }

    public Image imgLogo, imgRes;
    [SerializeField] private Text textName, textAmountWorker, textAmountEnergy, textAmountOutput, textAmountOxygen;
    
    public void UpdateText()
    {        
        textAmountWorker.text = $"{Module.Workers}/{Module.Workplaces}";
        textAmountEnergy.text = $"{Module.Energy}/{Module.MaxEnergy}";
        textAmountOxygen.text = $"{Module.Oxygen}";
        textAmountOutput.text = $"{Module.Output}/{Module.MaxOutput}";
    }

    public void SetName(string naming)
    {
        textName.text = naming;
    }
}
