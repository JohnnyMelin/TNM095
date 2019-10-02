using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;
    float speed;



    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(myManager.minSpeed, myManager.maxSpeed);
       
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, Time.deltaTime * speed);
        ApplyRules();
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = myManager.allFish;

        Vector3 vcentre = Vector3.zero;
        Vector3 vavoid = Vector3.zero;
        float gSpeed = 0.01f;   // Global speed of the group
        float nDistance;       // how far away is each fish
        int groupSize = 0;          // number of fish that are in a group
        
        foreach (GameObject go in gos)
        {
            nDistance = Vector3.Distance(go.transform.position, this.transform.position);

            if (nDistance <= myManager.neighbourDistance)
            {
                vcentre += go.transform.position;
                groupSize++;
                if (nDistance < 1.0f)
                {
                    vavoid = vavoid + (this.transform.position - go.transform.position);
                }

            }

            if (nDistance < 1.0f)
            {
                vavoid = vavoid + (this.transform.position - go.transform.position);
            }
            // grab the fishes speed and attach is to the group speed (gspeed)
            Flock anotherFlock = go.GetComponent<Flock>();
            gSpeed = gSpeed + anotherFlock.speed;
            
        }
        if (groupSize > 0)
        {
            // find the average center if a fish is infuelnced by another fish/group
            vcentre = vcentre / groupSize;
            // speed itself  of the fish is set to be the gspeed divided by groupSize
            speed = gSpeed / groupSize;
            Vector3 direction = (vcentre + vavoid) - transform.position;
            if (direction != Vector3.zero)
                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    Quaternion.LookRotation(direction),
                    myManager.rotationSpeed * Time.deltaTime
                    );
        }

    }
}
