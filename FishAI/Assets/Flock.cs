using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;
    public float speed = 0.001f;
    public float rotationSpeed = 4.0f;
    float neighbourDistance = 3.0f;
    Vector3 averageHeading;
    Vector3 averagePosition;

    bool turning = false;

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.5f, 1);
    }

    // Update is called once per frame
    void Update()
    {
        // testing the distance between the position of the fish and the center of the tank
        // and if its greater than the tank size the fish goes towards the center of the tank.
        if(Vector3.Distance(transform.position, Vector3.zero) >= FlockManager.tankSize)
        {
            turning = true;
        }
        else
        {
            turning = false;
        }

        if(turning)
        {
            Vector3 direction = Vector3.zero - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                  Quaternion.LookRotation(direction),
                                                  rotationSpeed * Time.deltaTime);
            speed = Random.Range(0.5f, 1);
        }
        else
        {
            if(Random.Range(0, 5) < 1)
            {
                ApplyRules();
            }
                
        }
        transform.Translate(0, 0, Time.deltaTime * speed);
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = FlockManager.allFish;

        Vector3 vcentre = Vector3.zero; // points to center of the group
        Vector3 vavoid = Vector3.zero;  // points away from potential neighbours
        float gSpeed = 0.1f;    // group speed

        Vector3 goalPos = FlockManager.goalPos; //  goal position

        float dist;
        int groupSize = 0;
        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if (dist <= neighbourDistance)
                {
                    vcentre += go.transform.position;
                    groupSize++;

                    if (dist < 1.0f)
                    {
                        vavoid += (this.transform.position - go.transform.position);
                    }

                    Flock anotherFlock = go.GetComponent<Flock>();
                    gSpeed += anotherFlock.speed;
                }
            }            
        }

        if (groupSize > 0)
        {
            // find the average center if a fish is infuelnced by another fish/group
            vcentre = vcentre / groupSize + (goalPos - this.transform.position);
            // speed itself  of the fish is set to be the gspeed divided by groupSize
            speed = gSpeed / groupSize;

            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(direction),
                                                      rotationSpeed * Time.deltaTime);
        }

    }
}
