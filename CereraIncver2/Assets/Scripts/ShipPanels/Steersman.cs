using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steersman : MonoBehaviour
{
    [SerializeField] Rigidbody rigidb;
    [SerializeField] Ship ship;
    public bool Rotated { get; set; }
    public bool Towards { get; set; }
    public bool RotatedLikeDock { get; set; }
    public float DistanceForAcc { get; set; }

    private void MoveShipWithOMS(Vector3 direction)
    {
        direction = Vector3.ClampMagnitude(direction, 20);
        var localVel = transform.InverseTransformDirection(direction);
        ship.Engines.OMSMove(localVel);    
        rigidb.MovePosition(rigidb.position + direction * Time.deltaTime * ship.mainClass.TimeSpeed);        
    }

    private void RotateShipWithOMS(Quaternion rot)
    {
        var localVel = transform.InverseTransformDirection(rot * Vector3.forward);        
        ship.Engines.OMSRotate(localVel);
        rigidb.rotation = Quaternion.Slerp(rigidb.rotation, rot, 0.5f * Time.fixedDeltaTime * ship.mainClass.TimeSpeed);
    }

    private void MoveShipWithEngine(Vector3 direction, float accelerate)
    {
        direction = Vector3.ClampMagnitude(direction, (ship.Navigator.DvToOperation * 86.4f) / 1000f);
        rigidb.MovePosition(rigidb.position + direction * Time.deltaTime * ship.mainClass.TimeSpeed * accelerate);
    }

    public void UnDock(DockingPort Dock)
    {
        var direction = new Vector3(0f, 2f, 0f);
        if (ship.ShipsDock.position.y > Dock.transform.position.y + 5)
        {
            Dock.UnDocking();
            ship.Dock = null;
            ship.Navigator.IsDocked = false;
            ship.Engines.OMSStop();
        }
        else
        {
            MoveShipWithOMS(direction);
        }
    }
    public void Docking(DockingPort Dock, bool isToWarehouse = false)
    {
        if (Mathf.Abs(ship.ShipsDock.position.y - Dock.transform.position.y) < 0.1)
        {            
            ship.Engines.OMSStop();
            ship.Navigator.DockIsSuccessfull(isToWarehouse);
            Rotated = false;
            Towards = false;
            RotatedLikeDock = false;
            DistanceForAcc = 0;
        }
        else
        {            
            MoveShipWithOMS(new Vector3(0,-2,0));
        }
    }

    public void MoveToDock(DockingPort Dock, bool toWarehouse = false)
    {
        //Debug.Log($"К двигаемся к доку, расстояние равно = {Vector3.Distance(transform.position, Dock.transform.position)}, позиция целия {Dock.transform.position}, позиция корабля {transform.position}");
        if (Vector3.Distance(transform.position, Dock.transform.position) <= 40)
        {
            //Debug.Log($"Приближаемся на ОМС");
            OMSMove(Dock, toWarehouse);
        }
        else if(Vector3.Distance(transform.position, Dock.transform.position) > 40)
        {
            EngineMove(Dock);
            //Debug.Log($"Двигаемся к доку");
        }
    }

    private void OMSMove(DockingPort Dock, bool toWarehouse = false)
    {
        if (RotatedLikeDock)
        {
            var veloc = (Dock.transform.position - ship.ShipsDock.position).normalized * 10;
            veloc.y = 0;
            
            if (EqualityValue(ship.ShipsDock.position.x, Dock.transform.position.x) && EqualityValue(ship.ShipsDock.position.z, Dock.transform.position.z))
            {
                
                Docking(Dock, toWarehouse);
            }
            else
            {
                MoveShipWithOMS(veloc);
            }
        }
        else
        {
            RotateLikeDock(Dock);
        }
    }

    private void RotateLikeDock(DockingPort Dock)
    {
        RotateShipWithOMS(Dock.transform.rotation);
        var localVel = transform.InverseTransformDirection(Dock.transform.rotation * Vector3.forward);
        if (localVel.z + 0.0001 >= 1)
        {
            RotatedLikeDock = true;
            ship.Engines.OMSStop();
        }
    }

    private bool EqualityValue(float rot1, float rot2)
    {
        bool answer = false;
        if(rot1 <0 && rot2< 0)
        {
            rot1 = Mathf.Abs(rot1);
            rot2 = Mathf.Abs(rot2);
        }

        if (Mathf.Abs(rot1 - rot2) < 0.1)
        {
            answer = true;                
        }
        return answer;
    }    
    private void EngineMove(DockingPort Dock)
    {        
        if (Towards)
        {            
            MoveFullForward(Dock);
        }
        else
        {
            RotateToTowards(Dock);
            DistanceForAcc = Vector3.Distance(transform.position, Dock.transform.position);
        }
    }

    private void RotateToTowards(DockingPort Dock)
    {
        var lookrot = Quaternion.LookRotation((Dock.transform.position - rigidb.position).normalized);
        RotateShipWithOMS(lookrot);
        var localVel = transform.InverseTransformDirection(lookrot * Vector3.forward);        
        if (localVel.z + 0.00001 >= 1)
        {
            Towards = true;
            ship.Engines.OMSStop();
        }
    }

    private void MoveFullForward(DockingPort Dock)
    {
        Vector3 targetPoint = (Dock.transform.position - transform.position).normalized * (ship.Navigator.DV * 86.4f)/1000;
        targetPoint.y = 0;        
        var dist = Vector3.Distance(transform.position, Dock.transform.position);
        float accelerate = 1;

        if (DistanceForAcc - dist < 20)
        {
            ship.Engines.FullForward();
            accelerate = (DistanceForAcc - dist + 1) / 20;
        }
        else if(DistanceForAcc - dist > 20 && dist > 80 && !Rotated)
        {
            ship.Engines.StopEngine();
            var lookrot = Quaternion.LookRotation((transform.position - Dock.transform.position).normalized);
            var localVel = transform.InverseTransformDirection(lookrot * Vector3.forward);            
            RotateShipWithOMS(lookrot);
            if (localVel.z + 0.00001 >= 1)
            {
                Towards = true;
                ship.Engines.OMSStop();
                Rotated = true;
            }
        }
        else if(dist<60 && dist > 40)
        {
            ship.Engines.FullForward();
            accelerate = (dist - 40+ 1) / 20;
        }

        if(dist < 45 && dist >= 40)
        {
            ship.Engines.StopEngine();
        }
        MoveShipWithEngine(targetPoint, accelerate);

    }
}
