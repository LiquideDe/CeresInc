using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StationButton : MonoBehaviour
{
    public Text textName, textWorker, textFood, textEquipment, textClean, textIncome, textReady, textExcavated;
    public int id;
    public PanelStation stations;
    public GameObject buttonEmbedAgent, buttonSabotage;
    
    public void EmbedAgent()
    {
        stations.EmbedAgent(id);
        buttonEmbedAgent.SetActive(false);
        buttonSabotage.SetActive(true);
    }

    public void Sabotage()
    {
        stations.Sabotage(id);
    }
}
