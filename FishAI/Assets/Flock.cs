﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public FlockManager myManager;
    public float trueSpeed = 0.015f;
    public float speed = 0.001f;
    public float rotationSpeed = 200.0f;
    public float cohesionWeight = 4f;
    public float alignmentWeight = 1f;
    public float goalposWeight = 2f;
    public float avoidWeight = 200f;
    public float fishFov = 270;

    public GameObject pred; // Predator

    float avoidDistance = 0.5f;
    float neighbourDistance = 1.0f; // was 3 
    float predator_distance; // current distance from fish to predator
    float flee_distance = 2.0f; // distance to predator where fish start fleeing

    Vector3 averageHeading;
    Vector3 averagePosition;
    //List<GameObject> neighbours;
    
    bool turning = false;
    bool fleeing = false;

    //Vector3 avoidanceMove(List<GameObject> fishes)
    //{
    //    if (fishes.Count == 0) return Vector3.zero; // if we have no neighbours add no correction

    //    Vector3 avoidance = Vector3.zero;

    //    // summarize and get average position
    //    int nAvoid = 0;
    //    foreach (GameObject fish in fishes)
    //    {
    //        if(Vector3.Distance(this.transform.position, fish.transform.position) < avoidDistance)
    //        {
    //            nAvoid++;
    //            avoidance += this.transform.position - fish.transform.position;
    //        }

    //    }
    //    if(nAvoid > 0) avoidance /= nAvoid;

    //    return avoidance;
    //}

    //Vector3 alignmentMove(List<GameObject> fishes)
    //{

    //    if (fishes.Count == 0) return this.transform.forward; // if we have no neighbours maintain current heading direction

    //    Vector3 alignment = Vector3.zero;

    //    // summarize and get average position
    //    foreach (GameObject fish in fishes)
    //    {
    //        alignment += fish.transform.forward;
    //    }
    //    alignment /= fishes.Count;

    //    return alignment;
    //}

    //Vector3 cohesionMove(List<GameObject> fishes)
    //{
    //    if (fishes.Count == 0) return Vector3.zero; // if we have no neighbours add no correction

    //    Vector3 cohesion = Vector3.zero;

    //    // summarize and get average position
    //    foreach (GameObject fish in fishes)
    //    {
    //        cohesion += fish.transform.position;
    //    }
    //    cohesion /= fishes.Count;
    //    //create offset from position
    //    cohesion -= this.transform.position;
    //    return cohesion;
    //}

    bool withinFov(Vector3 direction, Vector3 target)
    {


        if (Vector3.Angle(direction, target) <= fishFov / 2)
        {
            //Debug.Log("Inside");
            return true;
        }
        else
        {
            return false;
        }
        
    }

    // Start is called before the first frame update
    void Start()
    {
        speed = Random.Range(0.5f, 1);
        //pred.transform.position = Vector3(-FlockManager.tankSize, -FlockManager.tankSize, -FlockManager.tankSize);
    }

    // Update is called once per frame
    void Update()
    {
        // testing the distance between the position of the fish and the center of the tank
        // and if its greater than the tank size the fish goes towards the center of the tank.
        if (Vector3.Distance(transform.position, Vector3.zero) >= (FlockManager.tankSize + Random.Range(0.0f,FlockManager.tankSize/3)))
        {
            turning = true;
            
        }
        else
        {
            turning = false;
            
        }

        // Avoid preadator condition
        if (Vector3.Distance(transform.position, pred.transform.position) <= flee_distance) // if fish is close enough to predator flee
        {
            fleeing = true;
            rotationSpeed = 5.0f;
        }
        else
        {
            fleeing = false;
            rotationSpeed = 2.0f;
        }


        if (fleeing)
        {
            Vector3 direction = (transform.position - pred.transform.position);

            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(direction),
                                                      rotationSpeed * Time.deltaTime);
            }


            //speed = 6f;
            speed = (this.transform.forward * trueSpeed).magnitude +  ((flee_distance * (this.transform.forward * trueSpeed).magnitude) / Vector3.Distance(transform.position, pred.transform.position));

        }
        else
        {
            //speed = Random.Range(0.5f, 1);
            if (Random.Range(0, 5) < 1)
            {
                ApplyRules();
            }
        }

        if (turning)
        {

            Vector3 direction = new Vector3(Random.Range(-FlockManager.tankSize, FlockManager.tankSize),
                                                         Random.Range(-FlockManager.tankSize, FlockManager.tankSize),
                                                         Random.Range(-FlockManager.tankSize, FlockManager.tankSize));
            direction -= this.transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation,
                                                      Quaternion.LookRotation(direction),
                                                      rotationSpeed * Time.deltaTime);
            }
            //this.transform.position = Vector3.zero;
            speed = Random.Range(0.5f, 1);
        }
        else
        {
            if(Random.Range(0, 5) < 1)
            {
                ApplyRules();
            }
                
        }
        Vector3 Velocity = trueSpeed * transform.forward;
        transform.position += Velocity;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pred.transform.position, flee_distance);
        //Gizmos.DrawWireCube(myManager.transform.position, new Vector3(FlockManager.tankSize, FlockManager.tankSize, FlockManager.tankSize));
        Gizmos.DrawWireSphere(Vector3.zero, (FlockManager.tankSize + FlockManager.tankSize/3));
    }

    void ApplyRules()
    {
        GameObject[] gos;
        gos = FlockManager.allFish;
        Vector3 avoid = Vector3.zero;
        Vector3 align = Vector3.zero;
        Vector3 cohesion = Vector3.zero;
        float gSpeed = 0.1f;    // group speed
        int nAvoid = 0;
        Vector3 goalPos = FlockManager.goalPos; //  goal position

        float dist;
        int groupSize = 0;
        var b = new Bounds(transform.position, Vector3.one * neighbourDistance * 2);
        foreach (GameObject go in gos)
        {
            if (go != this.gameObject)
            {
                
                if (!b.Contains(go.transform.position))
                    continue;

                dist = Vector3.Distance(go.transform.position, this.transform.position);
                if (dist <= neighbourDistance)
                {
                    if (withinFov(this.transform.forward, go.transform.position - this.transform.position))
                    {
                        
                        //Debug.DrawLine(this.transform.position, go.transform.position, Color.green);
                        //neighbours.Add(go);
                        groupSize++;

                        // ==================   AVOIDANCE =======================================
                        // ======================================================================
                        //if (dist <= avoidDistance)
                        //{
                            nAvoid++;
                            avoid += this.transform.position - go.transform.position;
                        //}
                        // ======================================================================


                        // ==================   ALIGNMENT =======================================
                        // ======================================================================
                        align += go.transform.forward;
                        // ======================================================================

                        // ==================   COHESION ========================================
                        // ======================================================================
                        cohesion += go.transform.position;
                    }
                }

            }    

        }
        if (nAvoid > 0) avoid /= nAvoid;
        if (groupSize == 0) {
            align = this.transform.forward; // if we have no neighbours maintain current heading direction
            cohesion = Vector3.zero;        // if we have no neighbours add no correction
        }
        align /= groupSize;
        cohesion /= groupSize;
        //create offset from position
        cohesion -= this.transform.position;

        // speed itself  of the fish is set to be the gspeed divided by groupSize
        speed = gSpeed / groupSize;
        //trueSpeed = gSpeed / groupSize;
        //Debug.DrawLine(this.transform.position, this.transform.position + avoid);
        Vector3 direction = (cohesionWeight * cohesion + avoidWeight * avoid + alignmentWeight * align + goalposWeight*goalPos);// + 20*(goalPos - this.transform.position);
   
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation,
                                                    Quaternion.LookRotation(direction),
                                                    rotationSpeed * Time.deltaTime);
        

    }
}
