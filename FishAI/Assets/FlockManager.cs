﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockManager : MonoBehaviour
{
    public GameObject fishPrefab;
    public GameObject goalPrefab;
    public static int tankSize = 5;

    public static int numFish = 70;
    public static GameObject[] allFish = new GameObject[numFish];

    public static Vector3 goalPos = Vector3.zero;


    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos =  new Vector3(Random.Range(-tankSize, tankSize),
                                       Random.Range(-tankSize, tankSize),
                                       Random.Range(-tankSize, tankSize));

            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0,10000) < 40)
        {
            goalPos = new Vector3(Random.Range(-tankSize, tankSize),
                                  Random.Range(-tankSize, tankSize),
                                  Random.Range(-tankSize, tankSize));
            goalPrefab.transform.position = goalPos;
        }
    }
}
