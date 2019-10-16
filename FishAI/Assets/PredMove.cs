using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredMove : MonoBehaviour
{
    public float predSpeed = 0.02f;

    // Start is called before the first frame update
    void Start()
    {
        //this.transform.position = this.transform.forward * predSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += this.transform.forward * predSpeed;
        if (Vector3.Distance(this.transform.position, Vector3.zero) >= FlockManager.tankSize * 3.5)
        {
            this.transform.forward = -this.transform.forward;
        }
    }
}
