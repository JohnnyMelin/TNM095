using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject fishPrefab;
    public int numFish = 20;
    public GameObject[] allFish;
    public Vector3 swimLimits = new Vector3(10, 10, 10);
    
    // Use this for initialization
    void Start()
    {
        allFish = new GameObject[numFish];
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = this.transform.position + new Vector3(Random.Range(-swimLimits.x, swimLimits.x),
                Random.Range(-swimLimits.y, swimLimits.y),
                Random.Range(-swimLimits.z, swimLimits.z));
            allFish[i] = (GameObject)Instantiate(fishPrefab, pos, Quaternion.identity);
            //allFish[i].GetComponent<Flock>().myManager = this;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}