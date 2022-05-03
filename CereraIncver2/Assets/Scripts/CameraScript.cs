using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    private float moveSpeed = 20, zoomSpeed = 700, minZoom = 50, maxZoom = 2000;
    private Camera cam;
    RaycastHit hit;
    public PanelShip panelShip;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            Move();
        if (Input.mouseScrollDelta.y != 0 && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            Zoom();
        

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                else if (Physics.Raycast(ray, out hit))
                {
                    //hit.transform.GetComponent<AsteroidPanel>().OpenPanel(hit.transform);
                    if(hit.transform.GetComponent<Asteroid>())
                    hit.transform.GetComponent<AsteroidForPlayer>().AsterPanel.GetComponent<AsteroidPanel>().OpenPanel(hit.transform.GetComponent<AsteroidForPlayer>());
                    else if (hit.transform.GetComponent<Ship>())
                    {
                        panelShip.ShipPressed(hit.transform.GetComponent<Ship>().Id);
                    }
                    else if (hit.transform.GetComponent<Ceres>())
                    {
                        hit.transform.GetComponent<Ceres>().OpenPanel();
                    }

                }
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Ray rayRightButton = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayRightButton))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    return;

                else if (Physics.Raycast(rayRightButton, out hit))
                {
                    if(hit.transform.GetComponent<AsteroidForPlayer>() && panelShip.FlagCreateRoute)
                    panelShip.ChooseAsteroidForShip(hit.transform.GetComponent<AsteroidForPlayer>());
                }
            }
        }
    }

    void Move()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");
        Vector3 dir = transform.forward * zInput + transform.right * xInput;
        cam.transform.position += dir * moveSpeed;
    }

    void Zoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        
        if(cam.transform.position.y > minZoom && scrollInput >0)
        {
            cam.transform.position += cam.transform.forward * scrollInput * zoomSpeed;
            moveSpeed = 0.01f * cam.transform.position.y;
        }
        else if(cam.transform.position.y < maxZoom && scrollInput < 0)
        {
            cam.transform.position += cam.transform.forward * scrollInput * zoomSpeed;
            moveSpeed = 0.01f * cam.transform.position.y;
        }
        
    }
}
