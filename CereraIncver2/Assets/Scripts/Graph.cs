using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Graph : MonoBehaviour
{
    [SerializeField] private RectTransform graphContainer, labelTemplateX, labelTemplateY, dashTemplateX, dashTemplateY, tooltip;
    [SerializeField] private Sprite circleSprite;
    private List<GameObject> listGameObjects = new List<GameObject>();

    //Создание точки. Создаем объект изображение, присваиваем родителя, спрайт и задаем координаты
    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject gameObject = new GameObject("circle", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().sprite = circleSprite;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(20, 20);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        return gameObject;
    }

    //Очистка всех точек
    private void ClearList()
    {
        foreach(GameObject value in listGameObjects)
        {
            Destroy(value);
        }
        listGameObjects.Clear();
    }

    //Создание графика
    public void ShowGraph(List<List<float>> valueList, Func<int, string> getAxislabelX = null, Func<int, string> getAxislabelY = null, string type = "dot", string color = "", bool deleteOld = true, float yMax = 0, float yMin = 0)
    {
        //Очищаем сначала список, если он не пустой
        if(listGameObjects.Count != 0 && deleteOld)
            ClearList();
        if(getAxislabelX == null && type == "dot")
        {
            getAxislabelX = delegate (int _i) { return _i.ToString(); };
        }
        if (getAxislabelY == null && type == "dot")
        {
            getAxislabelY = delegate (int _f) { return _f.ToString(); };
        }
        float yMaximum = valueList[0][1];
        float yMinimum = valueList[0][1];
        if (yMax != 0)
        {
            yMaximum = yMax;
            yMinimum = yMin;
        }
        
        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;
        float xSize = graphWidth / valueList.Count;
        
        if( yMax == 0)
        {
            //Находим максимум и минимум по оси Y
            for (int i = 0; i < valueList.Count; i++)
            {
                if (valueList[i][1] > yMaximum)
                {
                    yMaximum = valueList[i][1];
                }
                if (valueList[i][1] < yMinimum)
                {
                    yMinimum = valueList[i][1];
                }
            }
        }
        
        //Делаем запас в 20% снизу и сверху от реальных значений, для красоты
        if (yMinimum == yMaximum)
        {
            yMinimum = 0;
        }
        yMaximum = yMaximum + (yMaximum - yMinimum) * 0.2f;
        if(yMinimum != 0)
        {
            yMinimum = yMinimum - (yMaximum - yMinimum) * 0.2f;
        }
        


        //Рисуем легенду и линии. сепаратор это какой шаг у линий Х
        int separator = 10;
        int offsetX;
        if(type == "bar" && valueList.Count < 12)
        {
            offsetX = 60;
        }
        else if(type == "bar")
        {
            offsetX = 20;
        }
        else
        {
            offsetX = 0;
        }
        if (deleteOld)
        {
            for (int i = 0; i <= separator; i++)
            {
                RectTransform labelY = Instantiate(labelTemplateY);
                labelY.SetParent(graphContainer, false);
                labelY.gameObject.SetActive(true);
                labelY.anchoredPosition = new Vector2(-50f, i * 1f / separator * graphHeight - 10);
                //labelY.anchoredPosition = new Vector2(-50f,((i - yMinimum)/(yMaximum - yMinimum))*graphHeight);
                labelY.GetComponent<Text>().text = getAxislabelY((int)(yMinimum + (i * 1f / separator * (yMaximum - yMinimum))));
                listGameObjects.Add(labelY.gameObject);

                RectTransform dashX = Instantiate(dashTemplateX);
                dashX.SetParent(graphContainer, false);
                dashX.gameObject.SetActive(true);
                dashX.anchoredPosition = new Vector2(0, i * 1f / separator * graphHeight);
                //dashX.anchoredPosition = new Vector2(0, Math.Abs(((i - yMinimum) / (yMaximum - yMinimum)) * graphHeight));
                listGameObjects.Add(dashX.gameObject);
            }
            
            for (int i = 0; i < valueList.Count; i++)
            {
                float xPosition = i * xSize + offsetX;
                RectTransform labelX = Instantiate(labelTemplateX);
                labelX.SetParent(graphContainer, false);
                labelX.gameObject.SetActive(true);
                labelX.anchoredPosition = new Vector2(xPosition, -30f);
                labelX.GetComponent<Text>().text = $"{valueList[i][0]} {getAxislabelX(i)}";
                listGameObjects.Add(labelX.gameObject);

                RectTransform dashY = Instantiate(dashTemplateY);
                dashY.SetParent(graphContainer, false);
                dashY.gameObject.SetActive(true);
                dashY.anchoredPosition = new Vector2(xPosition, -10);
                listGameObjects.Add(dashY.gameObject);
            }
        }
        
        //А вот тут уже создаем наши точки
        if (type == "dot")
            DrawDots(xSize, yMinimum, yMaximum, graphHeight, valueList);
        else
        {
            if (color == "Blue" && valueList.Count < 12)
                DrawBar(xSize, yMinimum, yMaximum, graphHeight, valueList, new Color32(78, 109, 177, 201), 40);
            else if(color == "Blue")
                DrawBar(xSize, yMinimum, yMaximum, graphHeight, valueList, new Color32(78, 109, 177, 201));
            else if(color == "Green" && valueList.Count < 12)
                DrawBar(xSize, yMinimum, yMaximum, graphHeight, valueList, new Color32(92, 229, 102, 255), 45);
            else if(color == "Green")
                DrawBar(xSize, yMinimum, yMaximum, graphHeight, valueList, new Color32(92, 229, 102, 255), 5);
            else if(color == "Purple" && valueList.Count < 12)
                DrawBar(xSize, yMinimum, yMaximum, graphHeight, valueList, new Color32(139, 0, 255, 255), 45);
            else if(color == "Purple")
                DrawBar(xSize, yMinimum, yMaximum, graphHeight, valueList, new Color32(139, 0, 255, 255), 5);
        }
            
        
        
        tooltip.SetAsLastSibling();
    }

    private void DrawDots(float xSize, float yMinimum, float yMaximum, float graphHeight, List<List<float>> valueList)
    {
        GameObject lastCircle = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = i * xSize;
            if (yMinimum == yMaximum)
            {
                yMinimum = 0;
            }
            float yPosition = ((valueList[i][1] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
            GameObject circleGameObject = CreateCircle(new Vector2(xPosition, yPosition));
            listGameObjects.Add(circleGameObject);
            InstallComponentTooltip(circleGameObject, valueList[i][1], xPosition + 10f, yPosition + 10f);
            if (lastCircle != null)
            {
                GameObject dotConnection = CreateDotConnection(circleGameObject.GetComponent<RectTransform>().anchoredPosition, lastCircle.GetComponent<RectTransform>().anchoredPosition);
                listGameObjects.Add(dotConnection);
                dotConnection.transform.SetSiblingIndex(2);
            }
            lastCircle = circleGameObject;
        }
    }

    private void DrawBar(float xSize, float yMinimum, float yMaximum, float graphHeight, List<List<float>> valueList, Color32 color, int offset = 0)
    {
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPosition = i * xSize + 20f;            
            float yPosition = ((valueList[i][1] - yMinimum) / (yMaximum - yMinimum)) * graphHeight;
            GameObject circleGameObject = CreateBar(new Vector2(xPosition + offset, yPosition), xSize * .3f, color);
            listGameObjects.Add(circleGameObject);
            InstallComponentTooltip(circleGameObject, valueList[i][1], xPosition, yPosition);
        }
    }

    private void InstallComponentTooltip(GameObject gameObject, float number, float xPosition, float yPosition )
    {
        var TooltipHelp = gameObject.AddComponent<TooltipHelp>();
        if (number - Math.Truncate(number) != 0)
        {
            TooltipHelp.txt = $"{Math.Round(number, 1)}";
        }
        else
        {
            TooltipHelp.txt = $"{number}";
        }
        TooltipHelp.pos = new Vector2(xPosition + 10f, yPosition + 10f);
        TooltipHelp.tooltip = tooltip;
    }

    private GameObject CreateBar(Vector2 graphPosition, float barWidth, Color32 color)
    {
        GameObject gameObject = new GameObject("bar", typeof(Image));
        gameObject.GetComponent<Image>().color = color;
        gameObject.transform.SetParent(graphContainer, false);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
        rectTransform.sizeDelta = new Vector2(barWidth, graphPosition.y);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0.5f, 0f);
        return gameObject;
    }

    private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB)
    {
        GameObject gameObject = new GameObject("dotConnection", typeof(Image));
        gameObject.transform.SetParent(graphContainer, false);
        gameObject.GetComponent<Image>().color = new Color(0, .7f, 1, .5f);
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        Vector2 dir = (dotPositionB - dotPositionA).normalized;
        float distance = Vector2.Distance(dotPositionA, dotPositionB);
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.sizeDelta = new Vector2(distance, 3f);
        rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
        Vector3 rot = Quaternion.LookRotation(dotPositionB - dotPositionA).eulerAngles;
        rectTransform.localEulerAngles = new Vector3(0,0,rot.x);
        return gameObject;
    }
}
