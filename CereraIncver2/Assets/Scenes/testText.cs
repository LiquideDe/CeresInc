using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class testText : MonoBehaviour
{
    [SerializeField] Text text;
    [SerializeField] Text label;
    RectTransform scroll, textRect;
    float wid;
    // Start is called before the first frame update
    void Start()
    {
        text.text = "Рождённые драконами, о чём говорит их название, драконорождённые идут гордо подняв голову по миру, который встречает их со страхом и непониманием. ";
        textRect = text.GetComponent<RectTransform>();
        Debug.Log($"Длинна текста {text.preferredWidth}, позиция текста {textRect.anchoredPosition}");
        textRect.anchoredPosition = new Vector2(text.preferredWidth, 0);

    }

    // Update is called once per frame
    void Update()
    {

        //scrollRect.normalizedPosition = Vector2.Lerp(scrollRect.normalizedPosition, new Vector2(wid, 0), 0.1f * Time.deltaTime);
        textRect.anchoredPosition = Vector2.Lerp(textRect.anchoredPosition, new Vector2(textRect.anchoredPosition.x - 20f, 0), Time.deltaTime * 10f);
    }

    public void StartScroll()
    {

    }
}
