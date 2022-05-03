using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CellOfElements : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int Id { get; set; }
    private bool flagHidePush, flagLightPush;
    [SerializeField] Text textName;
    [SerializeField] Image background;
    [SerializeField] GameObject frame;
    public Text TextName { get { return textName; } }
    public main Main { get; set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            HideOrShowAsteroid();
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            LightAsteroid();
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }

    private void HideOrShowAsteroid()
    {
        if (flagHidePush)
        {
            for (int i = 0; i < Main.Asteroids.AsteroidsCount(); i++)
            {
                if (Main.Asteroids.GetAsteroid(i).Element.Id == Id)
                {
                    Main.Asteroids.GetAsteroid(i).OnAsteroid();
                    flagHidePush = false;
                    background.color = new Color32(99, 153, 227, 255);
                }                    
            }
        }
        else 
        {
            for (int i = 0; i < Main.Asteroids.AsteroidsCount(); i++)
            {
                if (Main.Asteroids.GetAsteroid(i).Element.Id == Id)
                {
                    Main.Asteroids.GetAsteroid(i).OffAsteroid();
                    flagHidePush = true;
                    background.color = new Color32(57, 88, 130, 255);
                }                    
            }
        }
    }

    private void LightAsteroid()
    {
        if (!flagLightPush)
        {
            for (int i = 0; i < Main.Asteroids.AsteroidsCount(); i++)
            {
                if (Main.Asteroids.GetAsteroid(i).Element.Id == Id)
                {
                    Main.Asteroids.GetAsteroid(i).Outline.OutlineMode = Outline.Mode.OutlineVisible;
                    flagLightPush = true;
                    frame.SetActive(true);
                }
            }
        }
        else
        {
            for (int i = 0; i < Main.Asteroids.AsteroidsCount(); i++)
            {
                if (Main.Asteroids.GetAsteroid(i).Element.Id == Id)
                {
                    Main.Asteroids.GetAsteroid(i).Outline.OutlineMode = Outline.Mode.OutlineHidden;
                    flagLightPush = false;
                    frame.SetActive(false);
                }
            }
        }
    }
}
