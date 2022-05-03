using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 50f) * 0.01f;
    }

}
